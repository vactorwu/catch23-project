<%@ Register TagPrefix="controls" TagName="WebUserControl" Src="Controls/WebUserControl.ascx"  %>
<%@ Page Title="Configuration Service: Home" Buffer="true" Language="C#" MasterPageFile="~/Site.master" EnableSessionState="false" AutoEventWireup="True" Inherits="ConfigService.ServiceConfiguration.Web.Nodes" Codebehind="Nodes.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   
    <h2>
        Configuration Service Home
    </h2>
    <br />
    <center>
                <asp:Label id="TopNodeName" runat="server" CssClass="ConfigServiceInfoStyle" style="font-size:1.4em;" Text="Service Name"></asp:Label>
                <br/><asp:Label id="ServiceVersion" CssClass="ConfigServiceInfoStyle" runat="server" Text="Version"> </asp:Label>
                <br/><asp:Label id="ServiceHoster"  CssClass="ConfigServiceInfoStyle" runat="server" Text="Hoster"></asp:Label>
                <br/><asp:Label id="ServicePlatform"  CssClass="ConfigServiceInfoStyle" runat="server" Text="Platform"></asp:Label>
                <br/>
                <asp:ImageButton ID="RuntimePlatform" runat="server" />
                   <asp:Label ID="ALittleJS" runat="server"></asp:Label>
                   <br /><br />
                   <div style="font-size:1.4em;padding-bottom:5px;">Service Map</div>
                   <asp:ImageButton ID="SoaMapButton" runat="server" BorderColor="#000000" 
                    BorderWidth="3px" ImageUrl="~/Images/servicemapglobe.png" BorderStyle="double" 
                    onclick="SoaMapButton_Click" Width="156px" BackColor="White" Height="95px" 
                    TabIndex="1" PostBackUrl="~/Nodes.aspx" />
                   <br />
                   <hr />
                   <br />
                   <asp:LinkButton ID="TopNode" runat="server" 
                    CausesValidation="False" Height="20px" Width="100px" 
                    CssClass="LoginButton" TabIndex="7">Back to Top</asp:LinkButton>
                   <br />
                   <br />
                   <center>
                   <table align="center" width="700">
                   <tr>
                   <td class="NodeButtons" style="text-align:center;">
                   <asp:LinkButton ID="Users" runat="server" Width="100px" CssClass="LoginButton" 
                           Height="37px" TabIndex="2" 
                           PostBackUrl="~/Nodes.aspx">CS<br />  Users</asp:LinkButton>
                    </td>
                    <td class="NodeButtons" style="text-align:center;">
                   <asp:LinkButton ID="Hosted" runat="server" Width="100px" CssClass="LoginButton" 
                           Height="37px" TabIndex="3" PostBackUrl="~/Nodes.aspx">Hosted<br /> Services</asp:LinkButton>
                    </td>
                    <td class="NodeButtons" style="text-align:center;">
                    <asp:LinkButton ID="Connected" runat="server" Width="100px" CssClass="LoginButton" 
                           Height="37px" TabIndex="4" PostBackUrl="~/Nodes.aspx">Connected Services</asp:LinkButton>
                    </td>
                    <td class="NodeButtons" style="text-align:center;">
                    <asp:LinkButton ID="Connections" runat="server" Width="100px" CssClass="LoginButton" 
                           Height="37px" TabIndex="5" PostBackUrl="~/Nodes.aspx">Connection Points</asp:LinkButton>
                    </td>
                    <td class="NodeButtons" style="text-align:center;">
                    <asp:LinkButton ID="Logs" runat="server" Width="100px" CssClass="LoginButton" 
                           Height="37px" TabIndex="6" PostBackUrl="~/Nodes.aspx">Service <br /> Logs</asp:LinkButton>
                    </td>
                    </tr>
                    </table>          
                    </center>
                    <div style="font-size:.8em;padding-left:35px;padding-bottom:10px;text-align:left;padding-right:35px"> 
                    The buttons above operate against the currently selected service domain. The top-level menu always operates against the root service domain, which
                    is the one you logged into from the ConfigWeb login page. To change the service domain selection, choose <strong>Select</strong> from the Remote Connected Services table.  The Back To Top button will always take you back to the root service domain. All operations are routed through the network, so direct
                    network connectivity is not necessary for traversal. For example, some services may be hosted on Windows Azure, while others might reside on-premise or in other hosting environments.
                    With the Configuration Service, each service domain is completely autonomous and has exclusive access to its own configuration database. As you navigate, be aware of the currently selected service,
                    which is displayed at the top of every page in ConfigWeb, along with the platform the selected service is deployed to. One way to think about navigation in ConfigWeb
                    is it's the same concept as navigating through folders in Windows, except you are navigating across connected elements in your composite application, which are potentially connected across the world via the Internet; within a private corporate network; or both.  Once selected, you can
                    view and modify application settings for the service domain, and the Configuration Service will push the updates through the network to the correct domain, which will then update all running nodes
                    without any need to deploy new configuration files, or stop and re-start the running nodes.
                    </div>
   
 <table class="ConfigTableStyle" width="1100px" align="center">
      <col width="550px" />
      <col width="550px" />
          <tr>
                <th class="InnerTH1">Selected Service Domain</th>
                <th class="InnerTH1">Remote Connected Services</th>
          </tr>
          <tr>
                <td class="InnerTD1">
                <br />
                <table class="ConfigServiceTableStyle" style="background-image: url('Images/Backnodesinner.png');border:#c1c1c1 2px solid;table-layout:fixed;" align="center">
                <col width="175px" />
                <col width="100px" />
                <col width="100px" />
                <col width="125px" />
                    <tr>
                        <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid">Settings Group</th>
                        <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid">Hosted Service Domain</th>
                        <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid">View Nodes</th>
                        <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid">Configuration Settings</th>
                    </tr>
                  
                <asp:Repeater id="InProcessRepeater" runat="server" ClientIDMode="AutoID">
                <HeaderTemplate>        
                </HeaderTemplate>
                <ItemTemplate>
                            <controls:WebUserControl id="CurrentName" runat="server" ></controls:WebUserControl>
                            <controls:WebUserControl id="Status1" runat="server" ></controls:WebUserControl>
                            <controls:WebUserControl id="Deployment1" runat="server" ></controls:WebUserControl>
                            <controls:WebUserControl id="Configure1" runat="server" ></controls:WebUserControl>
                </ItemTemplate>
                <FooterTemplate>
                </FooterTemplate>
                </asp:Repeater>
               
                </table>
                <br />
                </td>
            <td class="InnerTD1">
            <br />
                <table class="ConfigServiceTableStyle" style="background-image: url('Images/Backnodesinner.png');border:#c1c1c1 2px solid;table-layout:fixed;" align="center">
                <col width="175px" />
                <col width="100px" />
                <col width="100px" />
                <col width="125px" />
                     <tr>
                        <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid">Settings Group</th>
                        <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid">Remote Service Domain Status</th>
                        <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid">View Nodes</th>
                        <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1  solid">Select</th>
                     </tr>
                <asp:Repeater id="CompositeServicesRepeater" runat="server">
                <HeaderTemplate>        
                </HeaderTemplate>
                <ItemTemplate>
                            <controls:WebUserControl id="RemoteServiceName" runat="server"></controls:WebUserControl>
                            <controls:WebUserControl id="Status2" runat="server" ></controls:WebUserControl>
                            <controls:WebUserControl id="Deployment2" runat="server" ></controls:WebUserControl>
                            <controls:WebUserControl id="Configure2" runat="server" ></controls:WebUserControl>
                            <controls:WebUserControl id="InProcessMessage2" runat="server" ></controls:WebUserControl>
                </ItemTemplate>
                <FooterTemplate>
                </FooterTemplate>
                </asp:Repeater>
                </table>
                <br />
                </td>
        </tr>
   
   </table>       
    <br />
    <br />
    </center>
</asp:Content>

