<%@ Page Title="Configuration Service: Hosted Service Endpoints" Buffer="true" Language="C#" EnableSessionState="false" MasterPageFile="~/Site.master" AutoEventWireup="True" Inherits="ConfigService.ServiceConfiguration.Web.HSServices" Codebehind="HSServices.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   
    <h2>
        Hosted Service Endpoints
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
                <span style="font-size:15px">Defined Endpoints for:<br />
                    <asp:Label ID="VHostName" runat="server" Text="Label"></asp:Label>
                <br /><br /></span>
               <asp:LinkButton ID="AddHostedService" runat="server" 
            PostBackUrl="AddConnection.aspx" Height="40px" 
               Width="140px" CssClass="AddConnButton" 
            onclick="AddHostedService_Click" TabIndex="1" >Add Service <br />Endpoint</asp:LinkButton>    
               <br /><br />
               <div style="font-size:.8em;padding-left:35px;padding-bottom:10px;text-align:left;padding-right:35px"> This page shows endpoints defined for a service host. A WCF ServiceHost can host multiple endpoints at once,
               potentially using different network schemes (http, https, net.tcp, etc.), different security configurations, and the like. Configuration Service lets you add, remove, edit, activate endpoints dynamically, without
               having to write new code. The options shown below are basically point and click options chosen in the hosted service defintion page. Choose Edit to view/modify existing endpoints as defined. Keep in mind that
               if changing an endpoint, WCF requires that the service host be closed and then re-created with the new description.  Configuration Service does this automatically across all active nodes.
                         </div>
             <table class="ConnectionPointTableStyle" style="background-image: url('Images/connpointfill.png');text-align:center;table-layout:fixed;" align="center" width="1150">
             <col width="55px" />
             <col width="180" />
             <col width="220px" />
             <col width="275px" />
             <col width="55px" />
             <col width="260px" />
             <col width="65px" />
             <col width="40px" />

             <tr>
                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Hosted Service ID</th>
                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Service Assigned Name For the Endpoint</th>
                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Service Contract</th>
                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Virtual Path and Port</th>
                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Base Address</th>
                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Binding Configuration</th>
                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Activated</th>
                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Edit</th>
             </tr>
            
          <asp:Repeater id="HostedServiceRepeater" runat="server">
          <HeaderTemplate>
          </HeaderTemplate>
          <ItemTemplate>
          <tr>
                <td class="TradeConfigTDSettingStyle1a" style="text-align:left;">
                <asp:Label ID="HSID" runat="server" Text=""></asp:Label></td>
                <td class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                <asp:Label ID="ServiceName" runat="server" Text=""></asp:Label></td>
                <td class="TradeConfigTDSettingStyle1a" style="text-align:left;">
                <asp:Label ID="ServiceContract" runat="server" Text=""></asp:Label></td>
                <td class="TradeConfigTDSettingStyle1a" style="text-align:left;">
                <asp:Label ID="ServiceVirtualPath" runat="server" Text=""></asp:Label></td>
                <td class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                <asp:Label ID="BaseAddress" runat="server" Text=""></asp:Label></td>
                <td class="TradeConfigTDSettingStyle1a" style="text-align:left;">
                <asp:Label ID="BindingInfo" runat="server" Text=""></asp:Label></td>
                <td class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                <asp:Label ID="HSActive" runat="server" Text=""></asp:Label></td>
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
                <br />
                <br />
    
  </center>
</asp:Content>

