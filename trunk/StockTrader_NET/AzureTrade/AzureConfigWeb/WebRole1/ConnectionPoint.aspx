<%@ Register TagPrefix="controls" TagName="WebUserControl" Src = "Controls/WebUserControl.ascx"  %>
<%@ Page Title="Configuration Service: Connection Points" Buffer="true" EnableSessionState="false" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="ConfigService.ServiceConfiguration.Web.ConnectionPoint" Codebehind="ConnectionPoint.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   
    <h2>
        Service Connection Points
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
                   <table align="center" width="300px">
                   <tr>
                        <td style="text-align:center;">
                         <asp:LinkButton id="ViewTypeHosts" runat="server" CssClass="LoginButton"
                        CausesValidation="False" PostBackUrl="ConnectionPoint.aspx" Height="20px" 
                                Width="100px" TabIndex="1" >View Services</asp:LinkButton>
                        </td>
                        <td style="text-align:center">
                        <asp:LinkButton id="ViewTypeClients" CssClass="LoginButton" CausesValidation="False"
                        runat="server" PostBackUrl="ConnectionPoint.aspx"  Height="20px" Width="100px" 
                                TabIndex="2" >View Clients</asp:LinkButton>
                        </td>
                    </tr>
                    </table>
                  <br />
                       <asp:Label ID="Message" runat="server" Text="Label" ></asp:Label>
                 <br /> 
               
                       <asp:LinkButton ID="AddConnection" runat="server" PostBackUrl="AddConnection.aspx" Height="30px" 
                       width="120px" CssClass="AddConnButton" 
                       TabIndex="3" >Add Connection</asp:LinkButton>    
                     <br />
                     <table width="1100" align="center">
                     <tr>
                     <td>
                     <p style="font-size:.8em;text-align:left">
                     This page shows the actual WCF connections between the client service domain and the remote service domain for any business service endpoints, as maintained by
                     the Configuration Service. If the remote service domain is running behind any type of NAT/external load balancer, as is the case with Windows Azure
                     service domains, then you will see just one connection for this service domain (with a corresponding icon), just as the client service domain sees--even though in reality there may be many nodes servicing this
                     endpoint behind the NAT load balancer.  On private cloud
                     deployments, unless you are also using a NAT to translate addresses, you will see every node as a distinct connection point, as clients communicate directly to each load-balanced node,
                     with load balancing and request-level failover provided by the Configuration Service itself with no NAT required. While the Node Map and Service Map pages display all nodes even if behind a NAT, 
                     this page is important in that it shows the view the client service domain is actually operating
                     against. As such, you can see the detail on the WCF client being utilized, including the endpoint address, the name of the client definition as
                     defined in configuration, the binding type, binding configuration name, and security mode the client is utilizing.  </p>
                     <p style="font-size:.8em;text-align:left">If a connection point below
                     shows red, hover over it to see the actual exception the client is receiving. If a connection below shows as a grey-colored server, a timeout was exceeded (also indicating an issue).   
                     </p>
                     </td>
                     </tr>
                     </table>
                    
                     
                     <asp:Repeater id="ConnectionRepeater" runat="server">
                     <HeaderTemplate>
                     
                     </HeaderTemplate>
                     
                    <ItemTemplate>
                    <controls:WebUserControl id="Address" runat="server" ></controls:WebUserControl>
                    <controls:WebUserControl id="Configuration" runat="server"></controls:WebUserControl>
                    <controls:WebUserControl id="Online" runat="server"></controls:WebUserControl>
                    <controls:WebUserControl id="Delete" runat="server"></controls:WebUserControl>
                    </ItemTemplate>

                        <FooterTemplate>
                        </table>
                        </FooterTemplate>
                    </asp:Repeater>
                    <br />
                    <asp:Label ID="Message2" runat="server"></asp:Label>
                    <br />
                   
                    <asp:Label ID="MSMQ" runat="server" ></asp:Label>
                    <br />
                    <asp:Label ID="ReturnLabel" runat="server" TabIndex="4" ></asp:Label>
                    <br />
       </center>
       <br />
</asp:Content>

