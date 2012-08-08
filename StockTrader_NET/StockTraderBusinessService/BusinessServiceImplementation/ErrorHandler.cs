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
// A sample custom error handler.  Used by the BSL, as an example.  In this case,
// simply writing the exception information into the WindowsConsoleHost for display in realtime.
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
using ConfigService.ServiceConfigurationUtility;
using Trade.BusinessServiceConfigurationSettings;

namespace Trade.BusinessServiceImplementation
{
    public class ErrorBehaviorAttribute : Attribute, IServiceBehavior
    {
        #region ErrorBehaviorAttribute Members
        ErrorHandler ErrorHandler;

        public ErrorBehaviorAttribute()
        {
            this.ErrorHandler = new Trade.BusinessServiceImplementation.ErrorHandler();
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
                channelDispatcher.ErrorHandlers.Add(ErrorHandler);
            }
        }
        #endregion
    }

    public class ErrorHandler : IErrorHandler
    {
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            // no-op -we are not interested in this.
        }

        public bool HandleError(Exception error)
        {
            //Note these first are idle timeouts based on the tcp binding setting receiveTimeout.  They will always happen
            //when a tcp connection is idle beyond the timeout value.  They are expected, and hence not logged.  
            //Also, Config Service clients automatically re-establish tcp connections if broken by the host.
            if (error.Message.Contains("receive timeout being exceeded by the remote host"))
                return true;
            //OK we have a real exception.  Log it!
            ConfigUtility.writeErrorConsoleMessage("\nError! Exception is: " + error.ToString(), EventLogEntryType.Error, true, new Trade.BusinessServiceConfigurationSettings.Settings());
            return true;
        }
    }
}