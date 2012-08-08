//  .NET StockTrader Sample WCF Application for Benchmarking, Performance Analysis and Design Considerations for Service-Oriented Applications
//                   4/10/2011: Updated to version 5.0, with notable enhancements for optional Windows Azure hosting, cross-browser and mobile-browser compatibility, and 
//                   new performance ehancements  See: 
//                                  1. Technical overview paper: http://download.microsoft.com/download/7/C/9/7C9F7B89-8AF0-4433-AB3A-B615C8EF9484/Trade5Overview.pdf
//                                  2. MSDN Site with downloads, additional information: http://msdn.microsoft.com/stocktrader
//                                  3. Discussion Forum: http://social.msdn.microsoft.com/Forums/en-US/dotnetstocktradersampleapplication
//                                  4. Live on Windows Azure: https://azurestocktrader.cloudapp.net
//                                   
//
//  Configuration Service 5 Notes:
//                      The application implements Configuration Service 5.0, for which the source code also ships in the sample. However, the .NET StockTrader 5
//                      sample is a general-purpose performance sample application for Windows Server and Windows Azure even if you are not implementing the Configuration Service. 
//                      
//

//======================================================================================================
// The Order Processor Service Poison message handler implementation logic.
//======================================================================================================



using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.Threading;
using System.Transactions;
using ConfigService.ServiceConfigurationUtility;
using ConfigService.ServiceNodeCommunication.DataContract;
using ConfigService.ServiceConfigurationHelper;
using Trade.OrderProcessorServiceConfigurationSettings;
using Trade.Utility;

namespace Trade.OrderProcessorPoisonMessageHandler
{
    public sealed class PoisonErrorBehaviorAttribute : Attribute, IServiceBehavior
    {
        #region PoisonErrorBehaviorAttribute Members
        PoisonErrorHandler poisonErrorHandler;

        public PoisonErrorBehaviorAttribute()
        {
            this.poisonErrorHandler = new PoisonErrorHandler();
        }

        void IServiceBehavior.Validate(ServiceDescription description, ServiceHostBase serviceHostBase)
        {
        }

        void IServiceBehavior.AddBindingParameters(ServiceDescription description, ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, BindingParameterCollection parameters)
        {
        }

        void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription description, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcherBase channelDispatcherBase in serviceHostBase.ChannelDispatchers)
            {
                ChannelDispatcher channelDispatcher = channelDispatcherBase as ChannelDispatcher;
                channelDispatcher.ErrorHandlers.Add(poisonErrorHandler);
            }
        }
        #endregion
    }

    public class PoisonErrorHandler : IErrorHandler
    {

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            // no-op -we are not interested in this.
        }

        // Handle poison message exception by moving the offending message out of the way for regular processing to go on.
        public bool HandleError(Exception error)
        {
            MsmqPoisonMessageException poisonException = error as MsmqPoisonMessageException;
            //Note--we only want to handle poison messages coming from a durable/transacted MSMQ here.
            //So if this is not the exception type, don't do anything.  Note that errors on
            //TCP, HTTP and non-transacted (volatile) MSMQ bindings for the Trade Service will attempt,
            //on their own, to send an order to the poison queue if an exception occurs.  However, a poison
            //message in a durable queue is a special, important case that is handled here such that the
            //order message is never lost, yet is removed from the main processing queue so processing
            //is not interrupted and the queue is kept "clean."
            if (null != poisonException)
            {
                // Use a new transaction scope to remove the message from the main application queue and add it to the poison queue.
                // The poison message service processes messages from the poison queue.
                using (TransactionScope txScope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    long lookupId = poisonException.MessageLookupId;
                    ConfigUtility.writeErrorConsoleMessage(" Poisoned message: message look up id = " + lookupId + "\n",EventLogEntryType.Information,true,new Settings());
                    // Get MSMQ queue name from app settings in configuration.
                    if (!System.Messaging.MessageQueue.Exists(Settings.QUENAMEPOISON))
                    {
                        ConfigUtility.writeErrorConsoleMessage("Creating Poison Message Queue: \n", EventLogEntryType.Information, true, new Settings());
                        ConfigUtility.writeErrorConsoleMessage(Settings.QUENAMEPOISON + "\n\n", EventLogEntryType.Information, true, new Settings());
                        System.Messaging.MessageQueue.Create(Settings.QUENAMEPOISON, true);
                    }
                    HostedServices opsService = Settings.hostedServices.Find(delegate(HostedServices hs) { return (hs.BindingType.Equals(ConfigUtility.NET_MSMQ_BINDING)); });
                    string tempqueuenametx = @".\private$\" + opsService.VirtualPath;
                    System.Messaging.MessageQueue orderQueue = new System.Messaging.MessageQueue(tempqueuenametx);
                    System.Messaging.MessageQueue poisonMessageQueue = new System.Messaging.MessageQueue(Settings.QUENAMEPOISON);
                    System.Messaging.Message message = null;
               
                    int retryCount = 0;
                    while (retryCount < 4)
                    {
                        retryCount++;
                        try
                        {
                            // Look up the poison message using the look up id.
                            message = orderQueue.ReceiveByLookupId(lookupId);
                            if (message != null)
                            {
                                // Send the message to the poison message queue.
                                if (Settings.NOPOISONQUEUE)
                                {
                                    ConfigUtility.writeErrorConsoleMessage("Running in /nopoisonq mode:  message is being removed and discarded!\n", EventLogEntryType.Warning, true, new Settings());
                                    txScope.Complete();
                                    break;
                                }
                                else
                                {
                                    poisonMessageQueue.Send(message, System.Messaging.MessageQueueTransactionType.Automatic);
                                    // complete transaction scope
                                    txScope.Complete();
                                    string writemessage = string.Format("Moved poisoned message with look up id: {0} to poison queue: {1} ", lookupId, Settings.QUENAMEPOISON);
                                    ConfigUtility.writeErrorConsoleMessage(writemessage + "\n\n", EventLogEntryType.Information, true, new Settings());
                                    break;
                                }
                            }
                        }
                        catch (InvalidOperationException)
                        {
                            //Code for the case when the message may still not be available in the queue because of a race in transaction or 
                            //another node in the farm may actually have taken the message.
                            if (retryCount < 2)
                            {
                                ConfigUtility.writeErrorConsoleMessage("Trying to move poison message but message is not available.  Will retry in 500 ms. \n", EventLogEntryType.Warning, true, new Settings());
                                Thread.Sleep(1000);
                            }
                            else
                            {
                               //The message will remain in the main queue in this condition. 
                                ConfigUtility.writeErrorConsoleMessage("Giving up on trying to move the message:  May have been automatically moved already to the local retry queue on Vista/Win Server 2008 depending on binding setting!\n", EventLogEntryType.Warning, true, new Settings());
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }
    }
}