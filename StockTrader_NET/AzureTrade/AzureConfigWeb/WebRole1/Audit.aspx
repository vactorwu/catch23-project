<%@ Page Title="Configuration Service: Logs" Language="C#" Buffer="true" MasterPageFile="~/Site.master" EnableSessionState="false" AutoEventWireup="true" Inherits="ConfigService.ServiceConfiguration.Web.Audit" Codebehind="Audit.aspx.cs" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Service Logs
    </h2>
                <!-- todo: move common logged in header content to master page or a usercontrol  -->
                <br />
                <center>
                <asp:Label id="TopNodeName" runat="server" CssClass="ConfigServiceInfoStyle" style="font-size:1.4em;" Text="Service Name"></asp:Label>
                <br/><asp:Label id="ServiceVersion" CssClass="ConfigServiceInfoStyle" runat="server" Text="Version"> </asp:Label>
                <br/><asp:Label id="ServiceHoster"  CssClass="ConfigServiceInfoStyle" runat="server" Text="Hoster"></asp:Label>
                <br/><asp:Label id="ServicePlatform"  CssClass="ConfigServiceInfoStyle" runat="server" Text="Platform"></asp:Label>
                <br/>
                <asp:ImageButton ID="RuntimePlatform" runat="server" />
                <hr />
                <br />
                   <asp:LinkButton ID="TopNode" runat="server" 
                    CausesValidation="False" Height="20px" Width="100px" 
                    CssClass="LoginButton" TabIndex="4">Back to Top</asp:LinkButton>
                <br />
                <br />
                <div style="font-size:.8em;padding-left:35px;padding-bottom:10px;text-align:left;padding-right:35px"> 
                    Three logs are maintained.  First the Cluster Log shows when individual nodes start and stop.  It also shows when a service domain receives notifications from connected services: for on-premise deployments
                    using the Configuration Service-provided load balancing and failover, for example, you will see notifications of new remote hosts coming online so that load balancing is automatically adjusted. 
                    Second, the Configuration Log is an audit log that shows when a configuration setting is changed by an administrator--their ID, the setting changed, and the time. The most
                    interesting perhaps is the Trace/Exception log.  For StockTrader, we have all services writing to the same central logging database (a SQL Azure database), so you will see all information from all
                    services consolidated.  However, the developer can choose to have services use their own unique logging database, or not use a logging database at all. These are simple selections in
                    ConfigWeb. Besides exceptions, you can turn on detailed logging, and capture/display rich informational trace information which can be helpful during development and testing.
                    </div>
<!-- ########################################## Main Content  ############################################# -->
                <table align="center" style="border-collapse:collapse;border:0px">
                        <tr>
                        <td width="150px" style="text-align:center">
                        <asp:LinkButton ID="ErrorLog" CssClass="AuditButton" CausesValidation="False"
                        runat="server" PostBackUrl="Audit.aspx" onclick="ErrorLog_Click"   Height="50px" 
                                Width="110px" TabIndex="1" >Trace and <br />Error Logs</asp:LinkButton>
                        </td>
                        <td width="150px" style="text-align:center">
                        <span style="padding-top:20px;">
                        <asp:LinkButton ID="ClusterLog" runat="server" CssClass="AuditButton"
                        CausesValidation="False" PostBackUrl="Audit.aspx" Height="50px" 
                                Width="110px" onclick="ClusterLog_Click" TabIndex="2" >Cluster <br />Event Logs</asp:LinkButton>
                        </span>
                        </td>
                        <td width="150px" style="text-align:center">
                        <asp:LinkButton ID="ConfigLog" CssClass="AuditButton" CausesValidation="False"
                        runat="server" PostBackUrl="Audit.aspx" onclick="ConfigLog_Click"  Height="50px" 
                                Width="110px" TabIndex="3" >Configuration <br />Logs</asp:LinkButton>
                        </td>
                        
                        </tr>
                </table>
                  
               <br />
                  
               <asp:Label ID="LogType" runat="server"></asp:Label>

                <asp:Panel ID="clusterpanel" runat="server" CssClass="Panel">
                        <br />
                         <asp:LinkButton ID="PurgeCluster" CssClass="AddConnButton" 
                       runat="server" PostBackUrl="Audit.aspx" Height="30px" 
              Width="120px" onclick="PurgeCluster_Click">Purge Log</asp:LinkButton>  
                        <br />
                    <asp:Label ID="MessageCluster" runat="server"></asp:Label>
                    <br />
                        <table class="ConnectionPointTableStyle" style="background-image: url('Images/connpointfill.png');text-align:center;table-layout:fixed;" align="center" width="1100px">
                                            
                                                
                                                    <col width="350px" />
                                                    <col width="650px" />
                                                    <col width="100px" />
                                                    <tr>
                                                        <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid;">
                                                            Host Address</th>
                                                        <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid">
                                                            Event Message</th>
                                                        <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid">
                                                            Time
                                                            <br />
                                                            <asp:Label ID="UTC" runat="server" Text=""></asp:Label>
                                                        </th>
                                                    </tr>
                                                    <asp:Repeater ID="ClusterRepeater" runat="server">
                                                        <HeaderTemplate>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="TradeConfigTDSettingStyle1a" 
                                                                    style="text-align:left;border:1px #000000 solid;">
                                                                    <asp:Label ID="HostAddress" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td class="TradeConfigTDSettingStyle1a" style="border:1px #000000 solid;">
                                                                    <asp:Label ID="EventMessage" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td class="TradeConfigTDSettingStyle1a" style="border:1px #000000 solid;">
                                                                    <asp:Label ID="EventTime" runat="server" Text=""></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                               
                                           
                        </table>
                        </asp:Panel>
                        <asp:Panel ID="configpanel" runat="server"  CssClass="Panel">
                                 <br />
                                   <asp:LinkButton ID="PurgeConfig" CssClass="AddConnButton" 
                       runat="server" PostBackUrl="Audit.aspx" Height="30px" 
              Width="120px" onclick="PurgeConfig_Click">Purge Log</asp:LinkButton>  
                                 <br />
                            <asp:Label ID="MessageConfig" runat="server"></asp:Label>
                            <br />
                                 <table class="ConnectionPointTableStyle" style="background-image: url('Images/connpointfill.png');text-align:center;table-layout:fixed;" align="center" width="1100px">
                                                 
                                                     <col width="50px" />
                                                     <col width="100px" />
                                                     <col width="100px" />
                                                     <col width="185px" />
                                                     <col width="185px" />
                                                     <col width="80px" />
                                                     <tr>
                                                         <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">
                                                             User ID</th>
                                                         <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">
                                                             Message</th>
                                                         <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">
                                                             Setting Name</th>
                                                         <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">
                                                             New Value</th>
                                                         <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">
                                                             Old Value</th>
                                                         <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid;">
                                                             Time<br /><asp:Label ID="UTC2" runat="server" Text=""></asp:Label>
                                                         </th>
                                                     </tr>
                                                     <asp:Repeater ID="ConfigRepeater" runat="server">
                                                         <HeaderTemplate>
                                                         </HeaderTemplate>
                                                         <ItemTemplate>
                                                             <tr>
                                                                 <td class="TradeConfigTDSettingStyle1a" style="border:1px #000000 solid;">
                                                                     <asp:Label ID="UserId" runat="server" Text=""></asp:Label>
                                                                 </td>
                                                                 <td class="TradeConfigTDSettingStyle1a" style="border:1px #000000 solid;">
                                                                     <asp:Label ID="Message" runat="server" Text=""></asp:Label>
                                                                 </td>
                                                                 <td class="TradeConfigTDSettingStyle1a" style="border:1px #000000 solid;">
                                                                     <asp:Label ID="KeyName" runat="server" Text=""></asp:Label>
                                                                 </td>
                                                                 <td class="TradeConfigTDSettingStyle1a" style="border:1px #000000 solid;">
                                                                     <asp:Label ID="NewValue" runat="server" Text=""></asp:Label>
                                                                 </td>
                                                                 <td class="TradeConfigTDSettingStyle1a" style="border:1px #000000 solid;">
                                                                     <asp:Label ID="OldValue" runat="server" Text=""></asp:Label>
                                                                 </td>
                                                                 <td class="TradeConfigTDSettingStyle1a" 
                                                                     style="text-align:left;border:1px #000000 solid;text-align:center">
                                                                     <asp:Label ID="UpdateTime" runat="server" Text=""></asp:Label>
                                                                 </td>
                                                             </tr>
                                                         </ItemTemplate>
                                                         <FooterTemplate>
                                                         </FooterTemplate>
                                                     </asp:Repeater>
                                                 
                                   </table>
                        </asp:Panel>
                         <asp:Panel ID="ErrorPanel" runat="server"  CssClass="Panel">
                                 <br />
                                   <asp:LinkButton ID="PurgeError" CssClass="AddConnButton" 
                       runat="server" PostBackUrl="Audit.aspx" Height="30px" 
              Width="120px" onclick="PurgeError_Click">Purge Log</asp:LinkButton>  
                                
                                 <br />
                             <asp:Label ID="MessageError" runat="server"></asp:Label>
                             <br />
                                <table align="center" style="border:0px">
                                <tr>
                                        <td style="border:0px;text-align:left">
                                        <asp:RadioButtonList ID="RadioButtonType" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="RadioButtonType_SelectedIndexChanged" 
                                                RepeatDirection="Horizontal" >
                                            <asp:ListItem Selected="True">All Entries</asp:ListItem>
                                            <asp:ListItem>Errors</asp:ListItem>
                                            <asp:ListItem>Warnings</asp:ListItem>
                                            <asp:ListItem>Informational</asp:ListItem>
                                        </asp:RadioButtonList>
                                        
                                        </td>
                                </tr>
                                </table>
                                 <table width="1000px" class="ConnectionPointTableStyle" style="background-image: url('Images/connpointfill.png');text-align:center;table-layout:fixed;" align="center">
                                             
                                                
                                                     <col width="250px" />
                                                     <col width="80px" />
                                                     <col width="530px" />
                                                     <col width="140px" />
                                                     <tr>
                                                         <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid">
                                                             Source</th>
                                                         <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid">
                                                             Severity</th>
                                                         <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid">
                                                             Message</th>
                                                         <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid">
                                                             Time<br /><asp:Label ID="UTC3" runat="server" Text=""></asp:Label>
                                                         </th>
                                                     </tr>
                                                     <asp:Repeater ID="ErrorRepeater" runat="server">
                                                         <HeaderTemplate>
                                                         </HeaderTemplate>
                                                         <ItemTemplate>
                                                             <tr>
                                                                 <td class="TradeConfigTDSettingStyle1a" style="border:1px #000000 solid;">
                                                                     <asp:Label ID="Source" runat="server" Text=""></asp:Label>
                                                                 </td>
                                                                 <td class="TradeConfigTDSettingStyle1a" style="border:1px #000000 solid;">
                                                                     <asp:Label ID="Severity" runat="server" Text=""></asp:Label>
                                                                 </td>
                                                                 <td class="TradeConfigTDSettingStyle1a" 
                                                                     style="text-align:left;border:1px #000000 solid;font-size:.9em;">
                                                                     <asp:Label ID="MessageError" runat="server" Text=""></asp:Label>
                                                                 </td>
                                                                 <td class="TradeConfigTDSettingStyle1a" style="border:1px #000000 solid;">
                                                                     <asp:Label ID="Time" runat="server" Text=""></asp:Label>
                                                                 </td>
                                                             </tr>
                                                         </ItemTemplate>
                                                         <FooterTemplate>
                                                         </FooterTemplate>
                                                     </asp:Repeater>
                                                
                                            
                                   </table>
                        </asp:Panel>
                         <br />
                         <asp:Label ID="ReturnLabel" runat="server" ></asp:Label>
                    <br />
     </center>            
     <br />
</asp:Content>