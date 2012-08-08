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
// The StockTrader Web App ConfigurationActions class, defining actions that get executed on Configuration 
// Updates.
//======================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using ConfigService.ServiceConfigurationBase;
using ConfigService.ServiceConfiguration.DataContract;
using ConfigService.ServiceConfigurationHelper;
using ConfigService.LoadBalancingClient;
using ConfigService.ServiceNodeCommunication.DataContract;
using ConfigService.ServiceConfigurationUtility;
using ConfigService.ServiceNodeCommunicationImplementation;
using ConfigService.ServiceNodeCommunicationContract;
using Trade.BusinessServiceContract;
using Trade.OrderProcessorContract;
using Trade.StockTraderWebApplicationSettings;
using Trade.Utility;

namespace Trade.StockTraderWebApplicationConfigurationImplementation
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
             //This is the key call that will startup all services defined in the repository.  In this case, just the config and node services since the Simple Web app does not host any custom services.
                ServiceConfigHelper.MasterServiceWebHost MasterHost = new ServiceConfigHelper.MasterServiceWebHost(new Settings(), new ConfigurationService(), new NodeCommunication(),null, this,null,null,new object[]{typeof(ITradeServices), typeof(IOrderProcessor)}, null);
                setAccessMode(Trade.StockTraderWebApplicationSettings.Settings.ACCESS_MODE);
                if (Settings.ACCESS_MODE == StockTraderUtility.BSL_INPROCESS)
                    InitConfigInProcessBusinessService.initConfigBusinessService(ConfigUtility.masterServiceWebHostSyncObject, false);
        }

        private ConfigurationKeyValues refreshBSL(out ConfigurationKeyValues thisOldbusinessServiceKey, string fieldName, ServiceUsers csUser)
        {
            Trade.StockTraderWebApplicationConfigurationImplementation.ConfigurationService configService = new ConfigurationService();
            List<TraverseNode> traversePath;
            ConfigurationKeyValues businessServiceKey = new ConfigurationKeyValues();
            businessServiceKey.ConfigurationKeyFieldName = fieldName;
            businessServiceKey.OriginatingConfigServiceName = StockTraderUtility.BUSINESS_SERVICES_CONFIG;
            traversePath = configService.getTraversePath(null, Settings.BSL_CLIENT_ACTIVE_SERVICE_HOST_IDENTIFIER, StockTraderUtility.BUSINESS_SERVICES_CONFIG, csUser);
            businessServiceKey = configService.getServiceConfigurationKey(Settings.BSL_CLIENT_ACTIVE_SERVICE_HOST_IDENTIFIER, StockTraderUtility.BUSINESS_SERVICES_CONFIG, businessServiceKey.ConfigurationKeyFieldName, traversePath, csUser);
            thisOldbusinessServiceKey = businessServiceKey;
            return businessServiceKey;
        }
       
        /// <summary>
        /// Overrides the base method, performs app-specific logic on specific app-specific setting changes.  For the
        /// Web app there are few app specific routines we need to run when OPS-specific app settings are changed in the repository.
        /// MAKE SURE TO CALL THE BASE checkChangedSetting method FIRST!
        /// </summary>
        /// <param name="settingsInstance">Instance of this host's Settings class.</param>
        /// <param name="updatedConfigurationKey">The key that has been updated.</param>
        /// <param name="block">True to block on any multi-threaded calls, typically the node receiving via
        /// Config Service endpoint blocks, nodes receiving via Node Service do not, if any threads are spun up.</param>
        /// <returns>bool as success code.</returns>
        public override bool checkChangedSetting(object settingsInstance, ConfigurationKeyValues updatedConfigurationKey,bool block,ServiceUsers csUser)
        {
            bool success = false;
            Trade.StockTraderWebApplicationConfigurationImplementation.ConfigurationService configService = new ConfigurationService();
            ServiceConfigHelper configHelper = new ServiceConfigHelper(settingsInstance);
            success = base.checkChangedSetting(settingsInstance, updatedConfigurationKey, block, csUser);
            if (success)
            {
                  switch (updatedConfigurationKey.ConfigurationKeyFieldName)
                  {
                      case "ACCESS_MODE":
                          {
                              setAccessMode(updatedConfigurationKey.ConfigurationKeyValue);
                              ConfigurationKeyValues businessServiceKey = new ConfigurationKeyValues();
                              switch (Settings.ACCESS_MODE)
                              {
                                  
                                  case StockTraderUtility.BSL_IISHOST_BSL_HTTP:
                                      {
                                          if (block)
                                          {
                                              businessServiceKey.ConfigurationKeyFieldName = "REFRESHCONFIG";
                                              businessServiceKey.ConfigurationKeyValue = "RefreshConfig";
                                              businessServiceKey.OriginatingConfigServiceName = StockTraderUtility.BUSINESS_SERVICES_CONFIG;
                                              refreshBSLConfig(businessServiceKey, configService, block, csUser);
                                          }
                                          InitCommunicationChannels(settingsInstance, null, ConfigUtility.INIT_ALL_CONNECTED_INSTANCES);
                                          break;
                                      }

                                  case StockTraderUtility.BSL_IISHOST_BSL_TCP:
                                      {
                                          goto case StockTraderUtility.BSL_IISHOST_BSL_HTTP;
                                      }

                                  case StockTraderUtility.BSL_IISHOST_TCP_TMSEC_CLIENTCERT:
                                      {
                                          goto case StockTraderUtility.BSL_IISHOST_BSL_HTTP;
                                      }

                                  case StockTraderUtility.BSL_IISHOST_TCP_TMSEC_USERNAME:
                                      {
                                          goto case StockTraderUtility.BSL_IISHOST_BSL_HTTP;
                                      }

                                  case StockTraderUtility.BSL_IISHOST_WSHTTP_MSEC_CLIENTCERT:
                                      {
                                          goto case StockTraderUtility.BSL_IISHOST_BSL_HTTP;
                                      }

                                  case StockTraderUtility.BSL_IISHOST_WSHTTP_TMSEC_CLIENTCERT:
                                      {
                                          goto case StockTraderUtility.BSL_IISHOST_BSL_HTTP;
                                      }
                                  case StockTraderUtility.BSL_IISHOST_WSHTTP_TMSEC_USERNAME:
                                      {
                                          goto case StockTraderUtility.BSL_IISHOST_BSL_HTTP;
                                      }

                                  case StockTraderUtility.BSL_AZURE_TCP_TMSEC_CLIENTCERT:
                                      {
                                          goto case StockTraderUtility.BSL_IISHOST_BSL_HTTP;
                                      }

                                  case StockTraderUtility.BSL_AZURE_TCP_TMSEC_USERNAME:
                                      {
                                          goto case StockTraderUtility.BSL_IISHOST_BSL_HTTP;
                                      }

                                  case StockTraderUtility.BSL_AZURE_WSHTTP_TMSEC_CLIENTCERT:
                                      {
                                          goto case StockTraderUtility.BSL_IISHOST_BSL_HTTP;
                                      }

                                  case StockTraderUtility.BSL_AZURE_WSHTTP_TMSEC_USERNAME:
                                      {
                                          goto case StockTraderUtility.BSL_IISHOST_BSL_HTTP;
                                      }
                                 
                                  case StockTraderUtility.BSL_INPROCESS:
                                      {
                                          InitConfigInProcessBusinessService.initConfigBusinessService(ConfigUtility.masterServiceWebHostSyncObject,true);
                                          break;
                                      }

                                  default:
                                      {
                                          break;
                                      }
                                  }
                                  success = true;
                                  break;
                              }

                          case "MAX_DISPLAY_ORDERS":
                          {
                              List<TraverseNode> traversePath=null;
                              int successBSL = ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS;
                              if (Settings.ACCESS_MODE == StockTraderUtility.BSL_SOAP)
                              {
                                  success = true;
                                  break;
                              }
                              ConfigurationKeyValues businessServiceKey = null;
                              ConfigurationKeyValues thisOldbusinessServiceKey = null;
                              businessServiceKey = refreshBSL(out thisOldbusinessServiceKey, "MAX_QUERY_ORDERS", csUser);
                              if (businessServiceKey != null && block)
                              {
                                  businessServiceKey.ConfigurationKeyValue = updatedConfigurationKey.ConfigurationKeyValue;
                                  traversePath = configService.getTraversePath(null, Settings.BSL_CLIENT_ACTIVE_SERVICE_HOST_IDENTIFIER, StockTraderUtility.BUSINESS_SERVICES_CONFIG, csUser);
                                  successBSL = configService.receiveConfigurationKey(Settings.BSL_CLIENT_ACTIVE_SERVICE_HOST_IDENTIFIER,StockTraderUtility.BUSINESS_SERVICES_CONFIG, thisOldbusinessServiceKey,businessServiceKey,block, ConfigUtility.UPDATE_KEY_VALUE,traversePath,csUser);
                                  if (successBSL == ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS)
                                      success = true;
                              }
                              else
                                  success = false;
                              break;
                          }

                      case "MAX_DISPLAY_TOP_ORDERS":
                          {
                              List<TraverseNode> traversePath = null;
                              int successBSL = ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS;
                              if (Settings.ACCESS_MODE == StockTraderUtility.BSL_SOAP)
                              {
                                  success = true;
                                  break;
                              }
                              ConfigurationKeyValues businessServiceKey = null;
                              ConfigurationKeyValues thisOldbusinessServiceKey = null;
                              businessServiceKey = refreshBSL(out thisOldbusinessServiceKey, "MAX_QUERY_TOP_ORDERS", csUser);
                              if (businessServiceKey != null && block)
                              {
                                  businessServiceKey.ConfigurationKeyValue = updatedConfigurationKey.ConfigurationKeyValue;
                                  traversePath = configService.getTraversePath(null, Settings.BSL_CLIENT_ACTIVE_SERVICE_HOST_IDENTIFIER, StockTraderUtility.BUSINESS_SERVICES_CONFIG, csUser);
                                  successBSL = configService.receiveConfigurationKey(Settings.BSL_CLIENT_ACTIVE_SERVICE_HOST_IDENTIFIER, StockTraderUtility.BUSINESS_SERVICES_CONFIG, thisOldbusinessServiceKey, businessServiceKey, block, ConfigUtility.UPDATE_KEY_VALUE, traversePath, csUser);
                                  if (successBSL == ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS)
                                      success = true;
                              }
                              else
                                  success = false;
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


        /// <summary>
        /// This is where you could implemement any additional validation logic for any config key.  For the Web app, there are a few
        /// app-specific settings we need to validate.  If a setting update from ConfigWeb fails validation, the update to
        /// the repository (and other nodes) will be rejected automatically.  MAKE SURE TO CALL THE BASE VALIDATION FIRST.
        /// </summary>
        /// <param name="settingsInstance">Instance of this host's Settings class.</param>
        /// <param name="configurationKey">Key to be validated.</param>
        /// <returns>Bool return code, true if passes validation.</returns>
        public override bool validateConfigurationKey(object settingsInstance, ConfigurationKeyValues configurationKey)
          {
              //call base method first!
              bool success = base.validateConfigurationKey(settingsInstance, configurationKey);
              if (success)
              {
                  switch (configurationKey.ConfigurationKeyFieldName)
                  {
                      case "ACCESS_MODE":

                          
                          {
                              if (configurationKey.ConfigurationKeyValue != StockTraderUtility.BSL_AZURE_TCP_TMSEC_CLIENTCERT &&
                              configurationKey.ConfigurationKeyValue != StockTraderUtility.BSL_AZURE_TCP_TMSEC_USERNAME &&
                              configurationKey.ConfigurationKeyValue != StockTraderUtility.BSL_AZURE_WSHTTP_TMSEC_CLIENTCERT &&
                              configurationKey.ConfigurationKeyValue != StockTraderUtility.BSL_AZURE_WSHTTP_TMSEC_USERNAME &&
                              configurationKey.ConfigurationKeyValue != StockTraderUtility.BSL_IISHOST_BSL_HTTP &&
                              configurationKey.ConfigurationKeyValue != StockTraderUtility.BSL_IISHOST_BSL_TCP &&
                              configurationKey.ConfigurationKeyValue != StockTraderUtility.BSL_IISHOST_TCP_TMSEC_CLIENTCERT &&
                              configurationKey.ConfigurationKeyValue != StockTraderUtility.BSL_IISHOST_TCP_TMSEC_USERNAME &&
                              configurationKey.ConfigurationKeyValue != StockTraderUtility.BSL_IISHOST_WSHTTP_MSEC_CLIENTCERT &&
                              configurationKey.ConfigurationKeyValue != StockTraderUtility.BSL_IISHOST_WSHTTP_TMSEC_CLIENTCERT &&
                              configurationKey.ConfigurationKeyValue != StockTraderUtility.BSL_IISHOST_WSHTTP_TMSEC_USERNAME &&
                              configurationKey.ConfigurationKeyValue != StockTraderUtility.BSL_INPROCESS &&
                               configurationKey.ConfigurationKeyValue != StockTraderUtility.BSL_SELFHOST_HTTP &&
                              configurationKey.ConfigurationKeyValue != StockTraderUtility.BSL_SELFHOST_TCP &&
                              configurationKey.ConfigurationKeyValue != StockTraderUtility.BSL_SELFHOST_TCP_TMSEC_CLIENTCERT &&
                              configurationKey.ConfigurationKeyValue != StockTraderUtility.BSL_SELFHOST_TCP_TMSEC_USERNAME &&
                              configurationKey.ConfigurationKeyValue != StockTraderUtility.BSL_SELFHOST_WSHTTP_TMSEC_CLIENTCERT &&
                              configurationKey.ConfigurationKeyValue != StockTraderUtility.BSL_SELFHOST_WSHTTP_TMSEC_USERNAME &&
                              configurationKey.ConfigurationKeyValue != StockTraderUtility.BSL_SOAP )
                                 success = false;
                              else
                                 success = true;
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

        private void setAccessMode(string accessMode)
        {
            switch (accessMode)
            {
                case StockTraderUtility.BSL_INPROCESS: { goto case StockTraderUtility.BSL_IISHOST_BSL_HTTP; }
                case StockTraderUtility.BSL_AZURE_TCP_TMSEC_CLIENTCERT: { Settings.BSL_CLIENT_ACTIVE_SERVICE_HOST_IDENTIFIER = StockTraderUtility.AZURE_BUSINESS_SERVICES; break; }
                case StockTraderUtility.BSL_AZURE_TCP_TMSEC_USERNAME: { goto case StockTraderUtility.BSL_AZURE_TCP_TMSEC_CLIENTCERT; }
                case StockTraderUtility.BSL_AZURE_WSHTTP_TMSEC_CLIENTCERT: { goto case StockTraderUtility.BSL_AZURE_TCP_TMSEC_CLIENTCERT; }
                case StockTraderUtility.BSL_AZURE_WSHTTP_TMSEC_USERNAME: { goto case StockTraderUtility.BSL_AZURE_TCP_TMSEC_CLIENTCERT; }
                case StockTraderUtility.BSL_IISHOST_BSL_HTTP: { Settings.BSL_CLIENT_ACTIVE_SERVICE_HOST_IDENTIFIER = StockTraderUtility.IISHOST_BUSINESS_SERVICES; break; }
                case StockTraderUtility.BSL_IISHOST_BSL_TCP: { goto case StockTraderUtility.BSL_IISHOST_BSL_HTTP; }
                case StockTraderUtility.BSL_IISHOST_TCP_TMSEC_CLIENTCERT: { goto case StockTraderUtility.BSL_IISHOST_BSL_HTTP; }
                case StockTraderUtility.BSL_IISHOST_TCP_TMSEC_USERNAME: { goto case StockTraderUtility.BSL_IISHOST_BSL_HTTP; }
                case StockTraderUtility.BSL_IISHOST_WSHTTP_MSEC_CLIENTCERT: { goto case StockTraderUtility.BSL_IISHOST_BSL_HTTP; }
                case StockTraderUtility.BSL_IISHOST_WSHTTP_TMSEC_CLIENTCERT: { goto case StockTraderUtility.BSL_IISHOST_BSL_HTTP; }
                case StockTraderUtility.BSL_IISHOST_WSHTTP_TMSEC_USERNAME: { goto case StockTraderUtility.BSL_IISHOST_BSL_HTTP; }
                case StockTraderUtility.BSL_SELFHOST_HTTP: { Settings.BSL_CLIENT_ACTIVE_SERVICE_HOST_IDENTIFIER = StockTraderUtility.SELF_HOSTBUSINESS_SERVICES; break; }
                case StockTraderUtility.BSL_SELFHOST_TCP: { goto case StockTraderUtility.BSL_SELFHOST_HTTP; }
                case StockTraderUtility.BSL_SELFHOST_TCP_TMSEC_CLIENTCERT: { goto case StockTraderUtility.BSL_SELFHOST_HTTP; }
                case StockTraderUtility.BSL_SELFHOST_TCP_TMSEC_USERNAME: { goto case StockTraderUtility.BSL_SELFHOST_HTTP; }
                case StockTraderUtility.BSL_SELFHOST_WSHTTP_TMSEC_CLIENTCERT: { goto case StockTraderUtility.BSL_SELFHOST_HTTP; }
                case StockTraderUtility.BSL_SELFHOST_WSHTTP_TMSEC_USERNAME: { goto case StockTraderUtility.BSL_SELFHOST_HTTP; }
                case StockTraderUtility.BSL_SOAP: { Settings.BSL_CLIENT_ACTIVE_SERVICE_HOST_IDENTIFIER = StockTraderUtility.SOAP_BUSINESS_SERVICES; break; }
               
            }
        }


          /// <summary>
          /// Refreshes the BSL from repository.
          /// </summary>
          /// <param name="key"></param>
          /// <param name="configService"></param>
          /// <param name="notifyNodes"></param>
          /// <param name="csUser"></param>
          /// <returns></returns>
          public int refreshBSLConfig(ConfigurationKeyValues key, ConfigurationService configService, bool notifyNodes,ServiceUsers csUser)
          {
              int returnCode = ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS;
              string value = key.ConfigurationKeyValue;
              List<TraverseNode> traversePath = configService.getTraversePath(null, Settings.BSL_CLIENT_ACTIVE_SERVICE_HOST_IDENTIFIER, StockTraderUtility.BUSINESS_SERVICES_CONFIG, csUser);
              key = configService.getServiceConfigurationKey(Settings.BSL_CLIENT_ACTIVE_SERVICE_HOST_IDENTIFIER, StockTraderUtility.BUSINESS_SERVICES_CONFIG, key.ConfigurationKeyFieldName, traversePath, csUser);
              if (key != null)
              {
                  key.ConfigurationKeyValue = value;
                  traversePath = configService.getTraversePath(null, Settings.BSL_CLIENT_ACTIVE_SERVICE_HOST_IDENTIFIER, StockTraderUtility.BUSINESS_SERVICES_CONFIG, csUser);
                  if (traversePath!=null)
                      returnCode = configService.receiveConfigurationKey(Settings.BSL_CLIENT_ACTIVE_SERVICE_HOST_IDENTIFIER, StockTraderUtility.BUSINESS_SERVICES_CONFIG, key, key, notifyNodes, ConfigUtility.UPDATE_KEY_VALUE, traversePath, csUser);
              }
              return returnCode;
          }
    }
}
