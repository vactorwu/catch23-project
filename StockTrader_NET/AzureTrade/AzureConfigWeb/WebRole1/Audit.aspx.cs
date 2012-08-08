//  Configuration Service 5 Sample Code. Can implement and re-distribute in custom applications per contained End User License Agreement.
//                   4/10/2011: Updated to version 5.0, with notable enhancements for optional Windows Azure hosting, cross-browser and mobile-browser compatibility, and 
//                   new performance ehancements  See: 
//                                  1. Technical overview paper: http://download.microsoft.com/download/7/C/9/7C9F7B89-8AF0-4433-AB3A-B615C8EF9484/Trade5Overview.pdf
//                                  2. MSDN Site with downloads, additional information: http://msdn.microsoft.com/stocktrader
//                                  3. Discussion Forum: http://social.msdn.microsoft.com/Forums/en-US/dotnetstocktradersampleapplication
//                                  4. Live on Windows Azure: https://azureconfigweb.cloudapp.net
//                                   

//============================================================================================
//Logic to display audit pages from service configuration database.
//============================================================================================

//====================Update History===========================================================
// 3/31/2011: V5.0.0.0: A brand new release, updated for private/public clouds, other major changes.
//=============================================================================================

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;
using System.Diagnostics;
using ConfigService.ServiceConfigurationRemote;
using ConfigService.ServiceConfiguration.DataContract;
using ConfigService.ServiceNodeCommunication.DataContract;
using ConfigService.ServiceConfigurationUtility;
using ConfigService.ServiceConfigurationHelper;

namespace ConfigService.ServiceConfiguration.Web
{
    public partial class Audit : System.Web.UI.Page
    {

        string userid;
        string hostNameIdentifier;
        string configName;
        string address;
        string binding;
        string platform;
        string version;
        string hoster;
        ServiceUsers user;
        ServiceConfigurationClient configProxy;
        List<AuditCluster> clusterAuditData;
        List<AuditConfigSetting> configAuditData;
        List<LogInfo> logData;
        List<TraverseNode> traversePath;

        string atype;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.DefaultFocus = ErrorLog.ClientID;
            Input.getHostData(IsPostBack, ViewState, out userid, out address, out user, out binding, out hostNameIdentifier, out configName, out version, out platform, out hoster, false);
            atype = Request["type"];
            if (atype == null)
            {
                atype = ConfigUtility.CLUSTER_AUDIT;
                configpanel.Visible = true;
                clusterpanel.Visible = false;
                ErrorPanel.Visible = false;
            }
            MessageError.Text = "";
            MessageConfig.Text = "";
            MessageCluster.Text = "";
            string time = "<span style=\"color:#FFFFFF\">" + DateTime.Now.ToUniversalTime().ToString("f") + " (UTC)</span>";
            UTC.Text = time;
            UTC2.Text = time;
            UTC3.Text = time;
            ClusterRepeater.ItemDataBound += new RepeaterItemEventHandler(Cluster_ItemDataBound);
            ConfigRepeater.ItemDataBound += new RepeaterItemEventHandler(Config_ItemDataBound);
            ErrorRepeater.ItemDataBound += new RepeaterItemEventHandler(Error_ItemDataBound);
            ClusterLog.PostBackUrl = ConfigSettings.PAGE_AUDIT + "?type=" + ConfigUtility.CLUSTER_AUDIT + "&name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster;
            ErrorLog.PostBackUrl = ConfigSettings.PAGE_AUDIT + "?type=" + ConfigUtility.ERROR_AUDIT + "&name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster;
            ConfigLog.PostBackUrl = ConfigSettings.PAGE_AUDIT + "?type=" + ConfigUtility.CONFIG_AUDIT + "&name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster;
            TopNode.PostBackUrl = ConfigSettings.PAGE_NODES;
            switch (atype)
            {
                case ConfigUtility.CONFIG_AUDIT:
                    {
                        traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy,  address, binding, user);
                        configpanel.Visible = true;
                        ConfigLog.CssClass = "AuditButton";
                        ClusterLog.CssClass = "AuditButtonNonSelected";
                        ErrorLog.CssClass = "AuditButtonNonSelected";
                        LogType.Text = "<span style=\"font-size:1.4em\">" + ConfigUtility.CONFIG_AUDIT + "</span>";
                        clusterpanel.Visible = false;
                        ErrorPanel.Visible = false;
                        configAuditData = configProxy.getConfigAudit(hostNameIdentifier, configName, traversePath, user);
                        if (configAuditData != null)
                        {
                            ConfigRepeater.DataSource = configAuditData;
                            ConfigRepeater.DataBind();
                        }
                        PurgeConfig.PostBackUrl = "audit.aspx?type=" + ConfigUtility.CONFIG_AUDIT + "&name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster;
                        break;
                    }

                case ConfigUtility.CLUSTER_AUDIT:
                    {
                        traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, binding, user);
                        clusterpanel.Visible = true;
                        ConfigLog.CssClass = "AuditButtonNonSelected";
                        ClusterLog.CssClass = "AuditButton";
                        ErrorLog.CssClass = "AuditButtonNonSelected";
                        LogType.Text = ConfigUtility.CLUSTER_AUDIT;
                        configpanel.Visible = false;
                        ErrorPanel.Visible = false;
                        LogType.Text = "<span style=\"font-size:1.4em\">" + ConfigUtility.CLUSTER_AUDIT + "</span>";
                        clusterAuditData = configProxy.getClusterAudit(hostNameIdentifier, configName, traversePath, user);
                        if (clusterAuditData != null)
                        {
                            ClusterRepeater.DataSource = clusterAuditData;
                            ClusterRepeater.DataBind();
                        }
                        PurgeCluster.PostBackUrl = "audit.aspx?type=" + ConfigUtility.CLUSTER_AUDIT + "&name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster;
                        break;
                    }

                case ConfigUtility.ERROR_AUDIT:
                    {
                        getErrorLog();
                        break;
                    }
            }
            TopNode.PostBackUrl = ConfigSettings.PAGE_NODES;
            ServiceVersion.Text = version;
            ServicePlatform.Text = platform;
            ServiceHoster.Text = hoster;
            TopNodeName.Text = hostNameIdentifier;
            ReturnLabel.Text = "<a class=\"Return\" href=\"" + ConfigSettings.PAGE_NODES + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName +  "&version=" + version + "&platform=" + platform + "&hoster=" + hoster + "\">Return to Home Page</a>";
            GetImageButton.runtimePoweredBy(platform, RuntimePlatform);
        }

        private void getErrorLog()
        {
            string errortext = "<span style=\"color:Maroon;\">Please make sure you have configured your logging database, and turned on database logging for this service. " +
                "via the LOG_TO_DATABASE setting in the Configuration Repository.  You must first create the database and modify .config with the connection string to your " +
                "logging database. The sample database schema can be found in StockTrader web.config.</span>";
            try
            {
                string level = RadioButtonType.SelectedValue;
                traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, binding, user);
                ErrorPanel.Visible = true;
                ConfigLog.CssClass = "AuditButtonNonSelected";
                ClusterLog.CssClass = "AuditButtonNonSelected";
                ErrorLog.CssClass = "AuditButton";
                LogType.Text = "<span style=\"font-size:1.4em\">" + ConfigUtility.ERROR_AUDIT + "</span>";
                configpanel.Visible = false;
                clusterpanel.Visible = false;
                PurgeError.PostBackUrl = "audit.aspx?type=" + ConfigUtility.ERROR_AUDIT + "&name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster; 
                logData = configProxy.getErrorLog(hostNameIdentifier, configName, traversePath, user, level);
                if (logData != null)
                {
                    ErrorRepeater.DataSource = logData;
                    ErrorRepeater.DataBind();
                }
                else 
                    MessageError.Text = errortext;
            }
            catch(Exception)
            {
                MessageError.Text = errortext;
            }

        }

        protected void Purge()
        {
            traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy,  address, binding, user);
            switch (atype)
            {
                case ConfigUtility.CONFIG_AUDIT:
                    {
                        int returnValue = configProxy.purgeConfigAudit(hostNameIdentifier, configName, traversePath, user);
                        if (returnValue.Equals(ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS))
                        {
                            configAuditData = new List<AuditConfigSetting>();
                            ConfigRepeater.DataSource = configAuditData;
                            ConfigRepeater.DataBind();
                            MessageConfig.Text = "<br/><span style=\"color:#1B9427;font-size:13px;font-weight:normal;font-family:Verdana;\">The log was sucessfully purged.<br/></span>";
                            return;
                        }
                        string message;
                        if (returnValue.Equals(ConfigUtility.CLUSTER_UPDATE_FAIL_PERSISTED))
                            message = " The database operation failed.";
                        else
                            if (returnValue.Equals(ConfigUtility.CLUSTER_UPDATE_FAIL_REMOTE))
                                message = " There was a problem sending the request to the remote service.";
                            else
                                message = ConfigSettings.EXCEPTION_MESSAGE_FAIL_AUTHORIZATION;
                        MessageConfig.Text = "<br/><span style=\"color:Maroon;font-size:13px;font-weight:normal;font-family:Verdana;\">The log could not be purged: " + message + "<br/></span>";
                        break;
                    }
                case ConfigUtility.CLUSTER_AUDIT:
                    {
                        int returnValue = configProxy.purgeClusterAudit(hostNameIdentifier, configName, traversePath, user);
                        if (returnValue.Equals(ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS))
                        {
                            clusterAuditData = new List<AuditCluster>();
                            ClusterRepeater.DataSource = clusterAuditData;
                            ClusterRepeater.DataBind();
                            MessageCluster.Text = "<br/><span style=\"color:#1B9427;font-size:13px;font-weight:normal;font-family:Verdana;\">The log was sucessfully purged.</span><br/>";
                            return;
                        }
                        string message;
                        if (returnValue.Equals(ConfigUtility.CLUSTER_UPDATE_FAIL_PERSISTED))
                            message = " The database operation failed.";
                        else
                            if (returnValue.Equals(ConfigUtility.CLUSTER_UPDATE_FAIL_REMOTE))
                                message = " There was a problem sending the request to the remote service.";
                            else
                                message = ConfigSettings.EXCEPTION_MESSAGE_FAIL_AUTHORIZATION;
                        MessageCluster.Text = "<br/><span style=\"color:Maroon;font-size:13px;font-weight:normal;font-family:Verdana;\">The log could not be purged: " + message + " <br/></span>";
                        break;
           
                    }

                case ConfigUtility.ERROR_AUDIT:
                    {
                        try
                        {
                            int returnValue = configProxy.purgeErrorLog(hostNameIdentifier, configName, traversePath, user);
                            if (returnValue.Equals(ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS))
                            {
                                logData = new List<LogInfo>();
                                ErrorRepeater.DataSource = logData;
                                ErrorRepeater.DataBind();
                                MessageError.Text = "<br/><span style=\"color:#1B9427;font-size:13px;font-weight:normal;font-family:Verdana;\">The log was sucessfully purged.</span><br/>";
                                return;
                            }
                            string message;
                            if (returnValue.Equals(ConfigUtility.CLUSTER_UPDATE_FAIL_PERSISTED))
                                message = " The database operation failed.";
                            else
                                if (returnValue.Equals(ConfigUtility.CLUSTER_UPDATE_FAIL_REMOTE))
                                    message = " There was a problem sending the request to the remote service.";
                                else
                                    message = ConfigSettings.EXCEPTION_MESSAGE_FAIL_AUTHORIZATION;
                            MessageError.Text = "<br/><span style=\"color:Maroon;font-size:13px;font-weight:normal;font-family:Verdana;\">The log could not be purged: " + message + " <br/></span>";
                            break;
                        }
                        catch (Exception)
                        {
                                MessageError.Text= "<span style=\"color:Maroon;\">Please make sure you have configured your logging database, and turned on database logging for this service. " +
                                                    "via the LOG_TO_DATABASE setting in the Configuration Repository.  You must first create the database and modify .config with the connection string to your " +
                                                    "logging database. The sample database schema can be found in StockTrader web.config.</span>";
                        }
                        break;
                    }
            }
        }
        public void Cluster_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                AuditCluster item = ((AuditCluster)e.Item.DataItem);
                ((Label)e.Item.FindControl("HostAddress")).Text = item.HostAddress;
                ((Label)e.Item.FindControl("EventTime")).Text = item.EventTime.ToString();
                string chunked = item.EventMessage;
                ((Label)e.Item.FindControl("EventMessage")).Text = chunked;
                if (chunked.ToLower().Contains("remote connected client"))
                    ((Label)e.Item.FindControl("EventMessage")).ForeColor = System.Drawing.Color.LightGray;
                else
                    if (chunked.ToLower().Contains("remote connected config"))
                        ((Label)e.Item.FindControl("EventMessage")).ForeColor = System.Drawing.Color.Black;
                    else
                        if (chunked.ToLower().Contains("deactivated remote connected primary"))
                            ((Label)e.Item.FindControl("EventMessage")).ForeColor = System.Drawing.Color.Navy;
                        else
                            if (chunked.ToLower().Contains("activated remote connected primary"))
                                ((Label)e.Item.FindControl("EventMessage")).ForeColor = System.Drawing.Color.Blue;
                            else
                                if (chunked.ToLower().Contains("deactivated"))
                                    ((Label)e.Item.FindControl("EventMessage")).ForeColor = System.Drawing.Color.Maroon;
                                else
                                    if (chunked.ToLower().Contains("activated"))
                                        ((Label)e.Item.FindControl("EventMessage")).ForeColor = System.Drawing.Color.PaleGreen;
            }
        }

        public void Config_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                
                AuditConfigSetting item = ((AuditConfigSetting)e.Item.DataItem);
                string newValue = item.NewValue;
                string oldValue = item.OldValue;
                if (newValue.Length>50)
                    newValue = ChunkText.chunk(48,newValue);
                if (oldValue.Length>50)
                    oldValue = ChunkText.chunk(48,oldValue);
                ((Label)e.Item.FindControl("UpdateTime")).Text = item.UpdateTime.ToString();
                ((Label)e.Item.FindControl("UserId")).Text = item.UserId;
                ((Label)e.Item.FindControl("Message")).Text = item.Message;
                ((Label)e.Item.FindControl("KeyName")).Text = item.KeyName;
                ((Label)e.Item.FindControl("NewValue")).Text = newValue;
                ((Label)e.Item.FindControl("OldValue")).Text = oldValue;

            }
        }

        public void Error_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string colorstr="#000000";
                string typestr = "";
                LogInfo item = ((LogInfo)e.Item.DataItem);
                switch (item.Severity)
                {
                    case 0: { colorstr = "Black"; typestr = "Information"; break; }
                    case 1: { colorstr = "Yellow"; typestr = "Warning"; break; }
                    case 2: { colorstr = "Maroon"; typestr = "Error"; break; }
                    default: { break; }
                }
                ((Label)e.Item.FindControl("Source")).Text = item.Source;
                ((Label)e.Item.FindControl("Severity")).Text = "<span style=\"color:" + colorstr + ";\">" + typestr + "</span>";

                ((Label)e.Item.FindControl("MessageError")).Text = "<span style=\"color:" + colorstr+ ";\">" + item.Message + "</span>";
                ((Label)e.Item.FindControl("Time")).Text = item.Time.ToString();
            
            }
        }

        protected void ErrorLog_Click(object sender, EventArgs e)
        {

        }
        protected void ClusterLog_Click(object sender, EventArgs e)
        {

        }
        protected void ConfigLog_Click(object sender, EventArgs e)
        {

        }
        protected void PurgeCluster_Click(object sender, EventArgs e)
        {
            Purge();
        }

        protected void PurgeConfig_Click(object sender, EventArgs e)
        {
            Purge();
        }

        protected void PurgeError_Click(object sender, EventArgs e)
        {
            Purge();
        }

        protected void RadioButtonType_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

    }
}