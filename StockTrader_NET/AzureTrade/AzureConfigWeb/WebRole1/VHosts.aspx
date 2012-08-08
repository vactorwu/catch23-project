<%@ Page Title="Configuration Service: Virtual Service Hosts" Buffer="true" Language="C#" EnableSessionState="false" MasterPageFile="~/Site.master" AutoEventWireup="True" Inherits="ConfigService.ServiceConfiguration.Web.VHosts" Codebehind="VHosts.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   
    <h2>
        Virtual Service Hosts
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
<!-- ########################################## Main Content  ############################################# -->
             <asp:LinkButton ID="AddVirtualHost" runat="server" 
            PostBackUrl="VHosts.aspx" Height="30px" 
             Width="120px"  CssClass="AddConnButton" onclick="AddVirtualHost_Click" 
            TabIndex="1" >Add Virtual Host</asp:LinkButton>    
                         <br /><br />
                         <div style="font-size:.8em;padding-left:35px;padding-bottom:10px;text-align:left;padding-right:35px"> This page shows your Virtual Service Hosts.  Each equates to a WCF ServiceHost, which
                         is created automatically by the Configuration Service based on the configuration information you supplied (as stored in the config repository) in ConfigWeb. All ServiceHosts drive off their WCF service implementation
                         class.  Configuration Service automatically creates your ServiceHosts across nodes as they start, and manages their complete lifecycle.  Endpoints, service behaviors and the like are specified in ConfigWeb, and used to
                         construct the service host at node startup. You can have as many Primary Service Hosts as you want, each will have its own implementation class.  Choose Select to view/add/delete endpoints for the service host,
                         or edit to modify the base service host information.
                         </div>
             <table class="ConnectionPointTableStyle" style="background-image: url('Images/connpointfill.png');text-align:center;table-layout:fixed;" align="center" width="1100">
             <col width="50px" />
             <col width="60" />
             <col width="310px" />
             <col width="60px" />
             <col width="60px" />
             <col width="70px" />
             <col width="70px" />

             <tr>
                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Service Host ID</th>
                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Service Host Type</th>
                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Service Host Name / Implementation Class</th>

                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Is a Workflow Host</th>
                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Number of Service Endpoints</th>
                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Modify Services for this Virtual Host</th>
                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Edit This Virtual Host</th>
             </tr>
          <asp:Repeater id="VHostRepeater" runat="server">
          <HeaderTemplate>
          </HeaderTemplate>
          <ItemTemplate>
            <tr>
                <td class="TradeConfigTDSettingStyle1a" style="text-align:left;">
                <asp:Label ID="VID" runat="server" Text=""></asp:Label></td>
               <td class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                <asp:Label ID="ServiceHostType" runat="server" Text=""></asp:Label></td>
               <td class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                <asp:Label ID="ServiceHostName" runat="server" Text=""></asp:Label></td>
               <td class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                <asp:Label ID="LabelWorkflow" runat="server" Text=""></asp:Label></td>
               <td class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                <asp:Label ID="NumEndpoints" runat="server" Text=""></asp:Label></td>
               <td class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                <asp:Label ID="ViewServices" runat="server" Text=""></asp:Label></td>
               <td class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                <asp:Label ID="Edit" runat="server" Text=""></asp:Label></td>
           </tr>      
         </ItemTemplate>
         <FooterTemplate>
       </FooterTemplate>
       </asp:Repeater>
       </table>
       
    <br />
                    <asp:Label ID="ReturnLabel" runat="server" TabIndex="2" ></asp:Label>
                    <br /> <br />
    </center>
</asp:Content>

