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
// The Business Services ConfigurationActions class, defining actions that get executed on Configuration 
// Updates.
//======================================================================================================


using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Configuration;
using System.Security;
using System.Configuration;
using System.Diagnostics;
using ConfigService.ServiceConfigurationBase;
using ConfigService.ServiceConfiguration.DataContract;
using ConfigService.ServiceConfigurationHelper;
using ConfigService.ServiceConfigurationUtility;
using ConfigService.RuntimeHostData;
using ConfigService.ServiceNodeCommunication.DataContract;
using ConfigService.ServiceNodeCommunicationImplementation;
using Trade.OrderProcessorContract;
using Trade.BusinessServiceImplementation;
using Trade.BusinessServiceConfigurationSettings;
using Trade.BusinessServiceContract;
using Trade.OrderProcessorAsyncClient;
using Trade.Utility;

namespace Trade.BusinessServiceHostConfigurationImplementation 
{
    /// <summary>
    /// This class is a core part of an implementation of the configuration management service/system.  
    /// This class should inherit from the base implementation ConfigurationActiponsBase. The class,
    /// including methods defined in the base, performs configuration operations (actions) required to
    /// maintain the live running configuration system.
    /// </summary>
    public class ConfigurationActions : ConfigurationActionsBase
    {
        /// <summary>
        /// Constructor, which is also an entry point into creating the IIS-host MasterServiceWebHost and
        /// associated WCF ServiceHosts.  
        /// </summary>
        public ConfigurationActions()
        {
            lock (ConfigUtility.masterServiceWebHostSyncObject)
            {
                List<ServiceHostInfo> startupList = null;
                //This is the key call that will startup all services defined in the repository.  In this case, just the config and node services since the Simple Web app does not host any custom services.
                startupList = new List<ServiceHostInfo>(new ServiceHostInfo[] { new ServiceHostInfo(false, null, new object[] { new Trade.BusinessServiceImplementation.ErrorBehaviorAttribute() }, new TradeServiceBSL()) });
                ServiceConfigHelper.MasterServiceWebHost MasterHost = new ServiceConfigHelper.MasterServiceWebHost(new Settings(), new ConfigurationService(), new NodeCommunication(), null, this, startupList, null, new object[] { typeof(IOrderProcessor) }, null);
                setOrderMode(Trade.BusinessServiceConfigurationSettings.Settings.ORDER_PROCESSING_MODE);
            }
        }

        /// <summary>
        /// Constructor called when in self-host mode, since this logic is also
        /// used in IIS-host mode.  
        /// </summary>
        /// <param name="selfhost">True if in self-host mode.</param>
        public ConfigurationActions(bool selfhost)
        {
           
        }

        /// <summary>
        /// For BSL, this call initializes the connection string to the StockTraderDB, and the order mode as set
        /// in the repository.
        /// </summary>
        public void initBusinessService(object settingsInstance)
        {
            buildConnString();
            Settings.setTxModel();
            if (Settings.ORDER_PROCESSING_MODE == StockTraderUtility.OPS_INPROCESS)
            {
                try
                {
                    InitConfigInProcessOrderService.initConfigOrderProcessService(ConfigUtility.masterServiceWebHostSyncObject, true);
                }
                catch
                {
                    ConfigUtility.writeConsoleMessage("\nERROR INITIALIZING ORDER PROCESS SERVICE FROM REPOSITORY!  PLEASE EXAMINE EXCEPTION AND EXIT!", EventLogEntryType.Error, true, settingsInstance);
                    return;
                }
            }
        }

        /// <summary>
        /// This ensures we call required init procedure for BSL before opening a service host for business.  You can
        /// override this method in your own implementation for similar purposes, including making any manual code 
        /// adjustments to the ServiceHost description itself, if you want to.
        /// </summary>
        /// <param name="serviceHost">The created service host, all settings/endpoints already applied from repository.</param>
        /// <param name="VHostName">The name of the VHost that ties this to information in the repository.</param>
        /// <param name="settingsInstance">Instance of this host's Settings class.</param>
        /// <returns>Created ServiceHost, ready for Open()</returns>
        public override ServiceHost afterCreateServiceHostFromRepositoryBeforeOpen(ServiceHost serviceHost, string VHostName, object settingsInstance)
        {
            initBusinessService(settingsInstance);
            return serviceHost;
        }

        /// <summary>
        /// Overrides the base method, performs app-specific logic on specific app-specific setting changes.  For BSL,
        /// there are few app specific routines we need to run when OPS-specific app settings are changed in the repository.
        /// MAKE SURE TO CALL THE BASE checkChangedSetting method FIRST!
        /// </summary>
        /// <param name="settingsInstance">Instance of this host's Settings class.</param>
        /// <param name="updatedConfigurationKey">The key that has been updated.</param>
        /// <param name="block">True to block on any multi-threaded calls, typically the node receiving via
        /// Config Service endpoint blocks, nodes receiving via Node Service do not, if any threads are spun up.</param>
        /// <returns>bool as success code.</returns>
        public override bool checkChangedSetting(object settingsInstance, ConfigurationKeyValues updatedConfigurationKey, bool block, ServiceUsers csUser)
        {
            bool success = false;
            success = base.checkChangedSetting(settingsInstance, updatedConfigurationKey, block, csUser); 
            if (success)
            {
                switch (updatedConfigurationKey.ConfigurationKeyFieldName)
                {

                    case "ORDER_PROCESSING_MODE":
                        {
                            setOrderMode(updatedConfigurationKey.ConfigurationKeyValue);
                            ConfigUtility.invokeGenericConsoleMethod("PostInitProcedure", new object[] { Settings.ORDER_PROCESSING_MODE });
                            ConfigurationKeyValues orderServiceKey = new ConfigurationKeyValues();
                            switch (Settings.ORDER_PROCESSING_MODE)
                            {
                                case StockTraderUtility.OPS_INPROCESS:
                                    {
                                        InitConfigInProcessOrderService.initConfigOrderProcessService(ConfigUtility.masterServiceWebHostSyncObject, true);
                                        break;
                                    }

                                    //********************* Self Host OPS
                                case StockTraderUtility.OPS_SELF_HOST_MSMQ:
                                    {
                                        if (block)
                                        {
                                            orderServiceKey.ConfigurationKeyFieldName = "REFRESHCONFIG";
                                            orderServiceKey.ConfigurationKeyValue = "RefreshConfig";
                                            orderServiceKey.OriginatingConfigServiceName = "Trade.OrderProcessorHostConfigurationImplementation.ConfigurationService";
                                            refreshOPSConfig(orderServiceKey, new ConfigurationService(), block,csUser);
                                        }
                                        break;
                                    }
                               
                                case StockTraderUtility.OPS_SELF_HOST_TCP:
                                    {
                                        goto case StockTraderUtility.OPS_SELF_HOST_MSMQ;
                                    }

                                case StockTraderUtility.OPS_SELF_HOST_HTTP:
                                    {
                                        goto case StockTraderUtility.OPS_SELF_HOST_MSMQ;
                                    }

                                case StockTraderUtility.OPS_SELF_HOST_TCP_TMSEC_CLIENTCERT:
                                    {
                                        goto case StockTraderUtility.OPS_SELF_HOST_MSMQ;
                                    }

                                case StockTraderUtility.OPS_SELF_HOST_TCP_TMSEC_USERNAME:
                                    {
                                        goto case StockTraderUtility.OPS_SELF_HOST_MSMQ;
                                    }

                                case StockTraderUtility.OPS_SELF_HOST_HTTP_TMSEC_CLIENTCERT:
                                    {
                                        goto case StockTraderUtility.OPS_SELF_HOST_MSMQ;
                                    }

                                case StockTraderUtility.OPS_SELF_HOST_HTTP_TMSEC_USERNAME:
                                    {
                                        goto case StockTraderUtility.OPS_SELF_HOST_MSMQ;
                                    }

                                case StockTraderUtility.OPS_SOAP:
                                    {
                                        goto case StockTraderUtility.OPS_SELF_HOST_MSMQ;
                                    }

                            }

                            success = true;
                            break;
                        }

                    case "DAL":
                        {
                            buildConnString();
                            if (block)
                            {
                                ConfigurationKeyValues orderServiceKey = new ConfigurationKeyValues();
                                orderServiceKey.ConfigurationKeyFieldName = "DAL";
                                orderServiceKey.ConfigurationKeyValue = updatedConfigurationKey.ConfigurationKeyValue;
                                orderServiceKey.OriginatingConfigServiceName = "Trade.OrderProcessorHostConfigurationImplementation.ConfigurationService";
                                refreshOPSConfig(orderServiceKey, new ConfigurationService(), block, csUser);
                            }
                            break;
                        }

                    case "DBServer":
                        {
                            buildConnString();
                            if (block)
                            {
                                ConfigurationKeyValues orderServiceKey = new ConfigurationKeyValues();
                                orderServiceKey.ConfigurationKeyFieldName = "DBServer";
                                orderServiceKey.ConfigurationKeyValue = updatedConfigurationKey.ConfigurationKeyValue;
                                orderServiceKey.OriginatingConfigServiceName = "Trade.OrderProcessorHostConfigurationImplementation.ConfigurationService";
                                refreshOPSConfig(orderServiceKey, new ConfigurationService(), block, csUser);
                            }
                            break;
                        }

                    case "Database":
                        {
                            buildConnString();
                            if (block)
                            {
                                ConfigurationKeyValues orderServiceKey = new ConfigurationKeyValues();
                                orderServiceKey.ConfigurationKeyFieldName = "Database";
                                orderServiceKey.ConfigurationKeyValue = updatedConfigurationKey.ConfigurationKeyValue;
                                orderServiceKey.OriginatingConfigServiceName = "Trade.OrderProcessorHostConfigurationImplementation.ConfigurationService";
                                refreshOPSConfig(orderServiceKey, new ConfigurationService(),block, csUser);
                            }
                            break;
                        }

                    case "UserID":
                        {
                            buildConnString();
                            break;
                        }

                    case "Password":
                        {
                            buildConnString();
                            break;
                        }

                    case "ENABLE_GLOBAL_SYSTEM_DOT_TRANSACTIONS_CONFIGSTRING":
                        {
                            Settings.setTxModel();
                            break;
                        }

                    default:
                        {
                            success = true;
                            break;
                        }
                }
            }
            return success;
        }


        private void setOrderMode(string orderMode)
        {
            switch (orderMode)
            {
                case StockTraderUtility.OPS_INPROCESS:{Settings.OPS_CLIENT_ACTIVE_SERVICE_HOST_IDENTIFIER=StockTraderUtility.ORDER_PROCESSOR_SERVICE_SELFHOST; break;}
                case StockTraderUtility.OPS_AZURE_HTTP_TMSEC_CLIENTCERT: {Settings.OPS_CLIENT_ACTIVE_SERVICE_HOST_IDENTIFIER=StockTraderUtility.ORDER_PROCESSOR_SERVICE_AZURE; break;}
                case StockTraderUtility.OPS_AZURE_HTTP_TMSEC_USERNAME: { goto case StockTraderUtility.OPS_AZURE_HTTP_TMSEC_CLIENTCERT; }
                case StockTraderUtility.OPS_AZURE_TCP_TMSEC_CLIENTCERT: { goto case StockTraderUtility.OPS_AZURE_HTTP_TMSEC_CLIENTCERT; }
                case StockTraderUtility.OPS_AZURE_TCP_TMSEC_USERNAME: { goto case StockTraderUtility.OPS_AZURE_HTTP_TMSEC_CLIENTCERT; }
                case StockTraderUtility.OPS_SELF_HOST_HTTP: { goto case StockTraderUtility.OPS_INPROCESS; }
                case StockTraderUtility.OPS_SELF_HOST_HTTP_TMSEC_CLIENTCERT: { goto case StockTraderUtility.OPS_INPROCESS; }
                case StockTraderUtility.OPS_SELF_HOST_HTTP_TMSEC_USERNAME: { goto case StockTraderUtility.OPS_INPROCESS; }
                case StockTraderUtility.OPS_SELF_HOST_MSMQ: { goto case StockTraderUtility.OPS_INPROCESS; }
                case StockTraderUtility.OPS_SELF_HOST_TCP: { goto case StockTraderUtility.OPS_INPROCESS; }
                case StockTraderUtility.OPS_SELF_HOST_TCP_TMSEC_CLIENTCERT: { goto case StockTraderUtility.OPS_INPROCESS; }
                case StockTraderUtility.OPS_SELF_HOST_TCP_TMSEC_USERNAME: { goto case StockTraderUtility.OPS_INPROCESS; }
                case StockTraderUtility.OPS_SOAP: { Settings.OPS_CLIENT_ACTIVE_SERVICE_HOST_IDENTIFIER = StockTraderUtility.ORDER_PROCESSOR_SERVICE_SOAP; break; }
                

            }
        }

        /// <summary>
        /// Refreshes the Config of the OPS when running in-process with BSL.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="configService"></param>
        /// <param name="csUser"></param>
        /// <returns>Int success code.</returns>
        public int refreshOPSConfig(ConfigurationKeyValues key, ConfigurationService configService, bool notifyNodes, ServiceUsers csUser)
        {
            int returnCode= ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS;
            string value = key.ConfigurationKeyValue;
            string targetHostID = null;
            if (ConfigUtility.onAzure)
                targetHostID = Trade.Utility.StockTraderUtility.ORDER_PROCESSOR_SERVICE_AZURE;
            else
                targetHostID = Trade.Utility.StockTraderUtility.ORDER_PROCESSOR_SERVICE_SELFHOST;
            List<TraverseNode> traversePath = configService.getTraversePath(null, targetHostID, StockTraderUtility.ORDER_PROCESSOR_SERVICE_CONFIG, csUser);
            key = configService.getServiceConfigurationKey(targetHostID, StockTraderUtility.ORDER_PROCESSOR_SERVICE_CONFIG, key.ConfigurationKeyFieldName, traversePath, csUser);
            if (key != null)
            {
                key.ConfigurationKeyValue = value;
                traversePath = configService.getTraversePath(null, targetHostID, StockTraderUtility.ORDER_PROCESSOR_SERVICE_CONFIG, csUser);
                if (traversePath!=null || Trade.BusinessServiceConfigurationSettings.Settings.ORDER_PROCESSING_MODE.Equals(StockTraderUtility.OPS_INPROCESS))
                    returnCode = configService.receiveConfigurationKey(targetHostID, StockTraderUtility.ORDER_PROCESSOR_SERVICE_CONFIG, key, key, notifyNodes, ConfigUtility.UPDATE_KEY_VALUE, traversePath, csUser);
            }
            return returnCode;
        }

        /// <summary>
        /// This method builds a connection string based on DAL setting and settings for the database name, location, uid and password.
        /// Called on host initialization and also when the DAL or DB connection parameters are changed via ConfigWeb.
        /// </summary>
        private void buildConnString()
        {
            switch (Settings.DAL)
            {
                case Trade.Utility.StockTraderUtility.DAL_SQLSERVER:
                    {
                        if (!Settings.DBServer.Trim().ToLower().StartsWith("tcp:") && !Settings.DBServer.ToLower().Contains("sqlexpress"))
                            Settings.DBServer = "tcp:" + Settings.DBServer;
                        if (!Settings.DBServer.Trim().EndsWith(",1433") && !Settings.DBServer.ToLower().Contains("sqlexpress"))
                            Settings.DBServer = Settings.DBServer + ",1433";
                        Settings.TRADEDB_SQL_CONN_STRING = "server=" + Settings.DBServer + ";database=" + Settings.Database + ";user id=" + Settings.UserID + ";password=" + Settings.Password + ";min pool size=" + Settings.MinDBConnections + ";max pool size=" + Settings.MaxDBConnections;
                        if (Settings.DBServer.ToLower().Contains("windows.net"))
                            Settings.TRADEDB_SQL_CONN_STRING = Settings.TRADEDB_SQL_CONN_STRING + ";Trusted_Connection=False;Encrypt=True;TrustServerCertificate=True;Connect Timeout=60";
                        break;
                    }

                case Trade.Utility.StockTraderUtility.DAL_ORACLE:
                    {
                        Settings.TRADEDB_SQL_CONN_STRING = "Data Source=" + Settings.Database + ";user id=" + Settings.UserID + ";password=" + Settings.Password + ";min pool size=" + Settings.MinDBConnections + ";max pool size=" + Settings.MaxDBConnections + ";enlist=dynamic;";
                        break;
                    }

                case Trade.Utility.StockTraderUtility.DAL_DB2:
                    {
                        Settings.TRADEDB_SQL_CONN_STRING = "Network Transport Library=TCPIP;Network Address=" + Settings.DBServer + ";Initial Catalog=" + Settings.Database + ";Package Collection=" + Settings.Database + ";Default Schema=Schema;User ID=" + Settings.UserID + ";Password=" + Settings.Password + ";network port=50000;Units of Work=RUW; Connection Pooling=True;defer prepare=false;CCSID=37;PC Code Page=1252";
                        break;
                    }
            }
        }

        /// <summary>
        /// This is where you could implemement any additional validation logic for any config key.  For BSL, there are a few
        /// app-specific settings we need to validate.  If a setting update from ConfigWeb fails validation, the update to
        /// the repository (and other nodes) will be rejected automatically.  MAKE SURE TO CALL THE BASE VALIDATION FIRST.
        /// </summary>
        /// <param name="settingsInstance">Instance of this host's Settings class.</param>
        /// <param name="configurationKey">Key to be validated.</param>
        /// <returns>Bool return code, true if passes validation.</returns>
        public override bool validateConfigurationKey(object settingsInstance, ConfigurationKeyValues configurationKey)
        {
            //call base method first!
            bool success = base.validateConfigurationKey(settingsInstance,configurationKey);
            if (success)
            {
                switch (configurationKey.ConfigurationKeyFieldName)
                {
                    case "ORDER_PROCESSING_MODE":
                        {
                             if (configurationKey.ConfigurationKeyValue != StockTraderUtility.OPS_SOAP &&
                                configurationKey.ConfigurationKeyValue != StockTraderUtility.OPS_AZURE_HTTP_TMSEC_CLIENTCERT &&
                                configurationKey.ConfigurationKeyValue != StockTraderUtility.OPS_AZURE_HTTP_TMSEC_USERNAME &&
                                configurationKey.ConfigurationKeyValue != StockTraderUtility.OPS_AZURE_TCP_TMSEC_CLIENTCERT &&
                                configurationKey.ConfigurationKeyValue != StockTraderUtility.OPS_AZURE_TCP_TMSEC_USERNAME &&
                                configurationKey.ConfigurationKeyValue != StockTraderUtility.OPS_INPROCESS &&
                                configurationKey.ConfigurationKeyValue != StockTraderUtility.OPS_SELF_HOST_HTTP &&
                                configurationKey.ConfigurationKeyValue != StockTraderUtility.OPS_SELF_HOST_HTTP_TMSEC_CLIENTCERT &&
                                configurationKey.ConfigurationKeyValue != StockTraderUtility.OPS_SELF_HOST_HTTP_TMSEC_USERNAME &&
                                configurationKey.ConfigurationKeyValue != StockTraderUtility.OPS_SELF_HOST_MSMQ &&
                                configurationKey.ConfigurationKeyValue != StockTraderUtility.OPS_SELF_HOST_TCP &&
                                configurationKey.ConfigurationKeyValue != StockTraderUtility.OPS_SELF_HOST_TCP_TMSEC_CLIENTCERT &&
                                configurationKey.ConfigurationKeyValue != StockTraderUtility.OPS_SELF_HOST_TCP_TMSEC_USERNAME)
                                    success= false;
                            else
                                success= true;
                            break;
                        }
                    default:
                        {
                            success= true;
                            break;
                        }
                }
            }
            return success;
        }
    }
}                          
