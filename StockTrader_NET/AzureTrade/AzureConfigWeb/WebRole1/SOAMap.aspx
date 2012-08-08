<%@ Page Title="Configuration Service: Service Map" Buffer="true" Language="C#" EnableSessionState="false" AutoEventWireup="true" EnableViewState="true"  MaintainScrollPositionOnPostback="true" EnableEventValidation="true" Inherits="ConfigService.ServiceConfiguration.Web.SOAMap" Codebehind="SOAMap.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>SOA Map</title>
    <link rel="stylesheet" href="~/Styles/site.css" type="text/css" />
    
    </head>

<script type="text/javascript">
    var secs
    var timerID = null
    var timerRunning = false
    var delay = 1000

    function InitializeTimer() {
        // Set the length of the timer, in seconds
        secs = 5
        StopTheClock()
        StartTheTimer()
    }

    function StopTheClock() {
        if (timerRunning)
            clearTimeout(timerID)
        timerRunning = false
    }

    function StartTheTimer() {
        if (secs == 0) {
            StopTheClock()
            SoaMapForm.submit();
        }
        else {
            self.status = secs
            secs = secs - 1
            timerRunning = true
            timerID = self.setTimeout("StartTheTimer()", delay)
        }
    }
</script>

<body style="background-color:#000000;">  
<form id="SoaMapForm" runat="server">
<asp:Label ID="TimerJS" runat="server"></asp:Label>
<br />
        <div style="text-align:center;font-size:1.6em;color:#EBDCC3;width:1200px;">Service Map</div>
        
        <div style="text-align:center;font-size:1.0em;color:#EBDCC3;width:1200px">This is a <span style="font-weight:bold;color:palegreen">live</span> 
            view of the connected service deployment.<br /><br /></div>
     <div style="text-align:center;text-align:center;font-size:1.0em;color:#EBDCC3;width:1200px">
        <asp:Button ID="ButtonRefresh" runat="server" Text="Refresh" 
                OnClick="ButtonRefresh_Click" BackColor="#666699" BorderColor="#999999" 
                BorderStyle="Double" TabIndex="4" ForeColor="White" />&nbsp;&nbsp;&nbsp; 
        <asp:Button ID="ButtonClose" runat="server" Text="&nbsp;&nbsp;Close&nbsp;&nbsp;" 
                OnClick="ButtonClose_Click" BackColor="#666699" BorderColor="#999999" 
                BorderStyle="Double" TabIndex="3" ForeColor="White" /></div>
    
    <div style="text-align:center;font-size:1.1em;color:#EBDCC3;width:1200px">
            <asp:Label ID="AlittleJS" runat="server"></asp:Label><br />
            
            <asp:CheckBox ID="AutoRefresh" runat="server" ForeColor="#E0E0E0" 
                OnCheckedChanged="AutoRefresh_CheckedChanged" Text="AutoRefresh" 
                AutoPostBack="True" TabIndex="2" Font-Size="12px" />
             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
             
             <asp:CheckBox ID="CheckBoxEndpointDetail" 
                runat="server" ForeColor="White" Text="Show WCF Endpoint Detail" TabIndex="1" Font-Size="12px" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            
            <asp:CheckBox ID="CheckBoxNoDetail" 
                runat="server" ForeColor="White" Text="Show Just the Map" TabIndex="3" 
                Font-Size="12px" /></div>
    <br />
    <asp:Panel ID="Legend" runat="server">
    <table width ="1200px" style="table-layout:fixed;">
    <tr>
    <td width="750px" >
    
            <table class="SOAMapTableLegendStyle" width="745px" style="padding-left:30px;">
                 <tr>
                    <td style="text-align:center;padding-right:10px;padding-left:10px;padding-top:10px;"><img alt="" src="Images/soa_up.png" /></td>
                    <td style="text-align:left;padding-right:10px;padding-left:10px;padding-top:10px;font-size:.9em">Service Root</td>
                    <td style="text-align:right;padding-right:10px;padding-top:10px;"><img alt="" 
                            src="Images/primary.png" /></td>
                    <td style="text-align:left;padding-right:10px;padding-left:10px;padding-top:10px;font-size:.9em">Primary Endpoints</td>
                </tr>
                <tr>
                    <td style="text-align:center;padding-right:10px;padding-left:10px"><img alt="" src="Images/sql_server_up.png" /><img alt="" src="Images/sql_azure_up.png" /></td>
                    <td style="text-align:left;padding-right:10px;padding-left:10px;font-size:.9em">SQL Server / SQL Azure</td>
                    <td style="text-align:right;padding-right:10px"><img alt="" src="Images/config.png" /></td>
                    <td style="text-align:left;padding-right:10px;padding-left:10px;font-size:.9em">Configuration Service Endpoints</td>
                </tr>
                <tr>
                    <td style="text-align:center;padding-right:10px;padding-left:10px"><img alt="" src="Images/sql_server_db_up.png" /><img alt="" src="Images/sql_azure_db_up.png" /></td>
                    <td style="text-align:left;padding-right:10px;padding-left:10px;font-size:.9em">SQL Server / SQL Azure Databases</td>
                    <td style="text-align:right;padding-right:10px"><img alt="" src="Images/wincacheup.png" /><img alt="" src="Images/azurecacheup.png" /></td>
                    <td style="text-align:left;padding-right:10px;padding-left:10px;font-size:.9em">AppFabric Distributed Caches</td>
                </tr>
                <tr>
                    <td style="text-align:right;padding-right:10px"><img alt="" 
                            src="Images/vhostup.png" height="35" width="35"/><img alt="" 
                            src="Images/hyperv_vhostup.png" /><img alt="" 
                            src="Images/azure_vhostup.png" /></td>
                    <td style="text-align:left;padding-right:10px;padding-left:10px;font-size:.9em">Bare Metal / Hyper-V / Azure Deployment</td>
                    <td style="text-align:right;padding-right:10px"><img alt="" 
                            src="Images/endpointdown.bmp" /><img alt="" 
                            src="Images/endpointup.bmp" /></td>
                    <td style="text-align:left;padding-right:10px;padding-left:10px;font-size:.9em">Endpoint Online / Offline</td>
                </tr>
                <tr>
                    <td style="text-align:right;padding-right:10px;padding-bottom:10px;"><img alt="" 
                            src="Images/serverup.png" /><img alt="" 
                            src="Images/hypervup.png" />
                            <img alt="" src="Images/azure_serverup.png" /></td>
                    <td style="text-align:left;padding-right:10px;padding-left:10px;padding-bottom:10px;font-size:.9em">Bare Metal / Hyper-V/ Azure Instances</td>
                    <td style="text-align:right;padding-right:10px;padding-bottom:10px;"><img alt="" src="Images/stats.png" /></td>
                    <td style="text-align:left;padding-right:10px;padding-left:10px;padding-bottom:10px;font-size:.9em">Database Performance Metrics</td>
                </tr>
           </table>

            </td>
            <td style="color:Silver;text-align:left;width:1200px;padding-left:15px;" >
        <p style="font-size:.9em;">Hover over icons below and also expand tree nodes for complete information. This page begins with the service domain you logged into from the ConfigWeb login page. Each 'globe' (service domain) has
        four elements ConfigWeb can explore: (1) SQL databases that this service domain utilizes; (2) AppFabric distributed caches this domain utilizes; (3) service nodes (or Azure Instances) this service domain is running on; (4) remote service domains this service connects to. 
        This is a <span style="color:palegreen;">live</span> map that reflects realtime information, including the online/offline status of the elements in the legend, down to the service endpoint level.  
        Configuration Service makes it possible to traverse connected services in an efficient fashion, across network domains and across the Internet.  You can watch the performance metrics change with a page 
        refresh.  Note the deployment below for the StockTrader Web Application shows that we have the Business Service tier and the Order
        Processing Tier deployed both on Windows Azure and in an on-premise private lab. A single code base is running throughout to demonstrate a hybrid deployment.
        The StockTrader Web application can be toggled to use the Azure-hosted services or on-premise services via a simple point-and-click in ConfigWeb.  Any application deployed on Windows Azure
        can connect securely to on-premise resources and utilize on-premise databases: through the use of secure services, through Windows Azure Connect, and through the Windows Azure AppFabric Service Bus.
        On-premise applications can also easily utilize Azure-hosted services and SQL Azure running in the cloud. </p>
                    
        <p style="font-size:.9em;">Note that the number of instances we have deployed the service domains to is for demonstration purposes; the StockTrader 5 composite application performs well even on single nodes, or could easily be deployed on many more nodes for even greater capacity.
         If a node or individual endpoint shows red, you can also hover over the red icon to see the exception message, to help in debugging. More complete exception information is available in the Service Logs page under Trace/Exception Logs.
    Azure Role Instance IDs are displayed for Azure-deployed services, so you can easily match an instance on this page to the same instance in the Windows Azure Management Console.</p>
                   
        </td>
    </tr>
    </table>
   </asp:Panel>
    <br />
    <asp:Label ID="UTC" runat="server" Text="Label"></asp:Label>
    <table>
        
    <tr>
        <td> <asp:Label ID="InValid" runat="server" Text=""></asp:Label>
       
            <asp:TreeView ID="soaTreeView" runat="server" ShowLines="True" CssClass="tree" 
            LineImagesFolder="~/TreeLineImages" Font-Size="1.1em" 
            NodeStyle-Height="30" ExpandDepth="1">
                <LeafNodeStyle CssClass="tree" />
<NodeStyle Height="30px"></NodeStyle>
                <RootNodeStyle HorizontalPadding="10px" />
            </asp:TreeView>
        
        </td>
    </tr>
</table>
</form>

</body></html>
