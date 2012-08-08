<%@ Page Title="Configuration Service: Connected Services" Buffer="true" Language="C#" EnableSessionState="false" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="ConfigService.ServiceConfiguration.Web.CSServices" Codebehind="CSServices.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   
    <h2>
       Connected Services
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
                 <div style="font-size:.8em;padding-left:35px;padding-bottom:10px;text-align:left;padding-right:35px"> This page shows definitions estblished in client applications that describe service endpoints this client connects to.
                 Services that implement the Configuration Service can connect to other services that also implement the Configuration Service, or any service via WCF SOAP, WS-* or REST capabilities. These might include services running on other
                 platforms such as Java or PHP. Likewise, from a service hosting standpoint, any client you allow can connect to your service, no matter whether it implements the Configuration Service or not, and no matter what platform it runs on. 
                 </div>
<!-- ########################################## Main Content  ############################################# -->
               <asp:LinkButton ID="AddConnectedService" runat="server" 
         PostBackUrl="AddConnection.aspx" Height="30px" 
               Width="120px" 
         CssClass="AddConnButton" TabIndex="1" >Add Service</asp:LinkButton>    
               <br /><br />
                
         
             <table class="ConnectionPointTableStyle" style="background-image: url('Images/connpointfill.png');text-align:center;table-layout:fixed;" align="center" width="1100">
             <col width="60" />
             <col width="280" />
             <col width="80" />
             <col width="240" />
             <col width="380" />
             <col width=60 />
             <tr>
                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Virtual Host ID</th>
                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Service Name</th>
                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Service Type</th>
                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Service Contract</th>
                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Client Configuration</th>
                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Edit/ Delete</th>
             </tr>
          

          <asp:Repeater id="ConnectedServiceRepeater" runat="server">
          <HeaderTemplate>
          </HeaderTemplate>
          <ItemTemplate>
            <tr>
                <td class="TradeConfigTDSettingStyle1a" style="text-align:left;">
                <asp:Label ID="VHOSTID" runat="server" Text=""></asp:Label></td>
                <td class="TradeConfigTDSettingStyle1a" >
                <asp:Label ID="CSNAME" runat="server" Text=""></asp:Label></td>
                <td class="TradeConfigTDSettingStyle1a">
                <asp:Label ID="CSServiceType" runat="server" Text="" ></asp:Label></td>
                <td class="TradeConfigTDSettingStyle1a" >
                <asp:Label ID="CSServiceContract" runat="server" Text=""></asp:Label></td>
                <td class="TradeConfigTDSettingStyle1a" >
                <asp:Label ID="CSBindingInfo" runat="server" Text=""></asp:Label></td>
                <td class="TradeConfigTDSettingStyle1a" >
                <asp:Label ID="CSEdit" runat="server" Text=""></asp:Label></td>
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

