<%@ Page Title="Configuration Service: Add Connection" Buffer="true" Language="C#" EnableSessionState="false" MasterPageFile="~/Site.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" Inherits="ConfigService.ServiceConfiguration.Web.AddConnection" Codebehind="AddConnection.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   
    <h2>
        Add Connection Point
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
                <table width="700" align="center" style="border:0px"> 
                <tr>
                        <td style="font-size:.8em;padding-bottom:10px;padding-left:10px;padding-right:10px;text-align:left;">
                        The Configuration Service already knows the details about the protocol, path and the port to your remote service endpoint. These were automatically saved when creating the Connected Service definition.  So, just supply
                        the DNS base address. For example, on-premise this would be <i>just</i> a server name on your domain that is currently active. For Azure, as an example for StockTrader 5 Business Services, this would be the DNS name created when creating the
                        Hosted Service through the Azure management portal. For example, '[name].cloudapp.net'. The remote domain will return available connections for every Connected Service definition you have established to that domain.  Once you establish this intitial Connection Point,
                        Configuration Service will dynamically manage all URIs for you, you do not need to worry about them again. In a Hyper-V virtualized environment, for example, when a new remote host activates simply by the startup of a new VM, your client 
                        service domain (all clients) will be notified and immediately utilize the new remote host for load balancing/added capacity. Click <a href="ConnectionPoint.aspx">here</a> to see the Connections Points after clicking the Add button. 
                    </td>
                    </tr>
                    </table>
                       
                        <table width="800" class="LoginTableStyle" align="center" style="border-collapse:collapse;border:#39454F 2px solid;"> 
                        
                        <tr>
                                <th class="TradeConfigTHStyle" colspan="2" >
                                    <asp:Label ID="Message" runat="server"></asp:Label><br />
                                </th>
                        </tr>
                        <tr>
                                <td colspan="2" class="TradeConfigTDStyle" style="border:0px">
                                <table align="center" style="border:0px">
                                <tr>
                                        <td style="border:0px;text-align:left;color:#c1c1c1;">
                                        <asp:RadioButtonList ID="RadioButtonListHosts" runat="server" AutoPostBack="True" 
                                                OnSelectedIndexChanged="RadioButtonListHosts_SelectedIndexChanged" TabIndex="1">
                                        </asp:RadioButtonList>
                                        <br /><br />
                                        </td>
                                </tr>
                                </table>
                                </td>
                        </tr>        
                        <tr>
                                <td style="color:#CCCCCC;text-align:right;padding-right:5px;">
                                Enter Base DNS Address to the Remote Host:</td>
                                <td style="color:#CCCCCC;text-align:left;"> 
                                    <asp:TextBox ID="ServiceHostAddress" CssClass="textEntry" 
                                    runat="server" Width="300px" Font-Size="12px" TabIndex="2"></asp:TextBox></td>
                                </tr>
                                <tr>
                                <td></td>
                                <td style="text-align:left;">
                                <br />
                                 <asp:LinkButton  ID="Add" runat="server" width="110px" Height="25px" 
                                        CausesValidation="False" PostBackUrl="AddConnection.aspx" onclick="Add_Click" 
                                        CssClass="ConfigUpdateButton" TabIndex="3">
                                 Add Connection</asp:LinkButton>
                                 <br /><br />
                                 </td>
                                 </tr>
                        </table>
                        <br />
                        <asp:Label ID="ReturnLabel" runat="server" ></asp:Label>
                    <br />
       </center>
       <br />

</asp:Content>

