<%@ Page Title="Configuration Service: Purge Connections" Buffer="true" Language="C#" EnableSessionState="false" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="ConfigService.ServiceConfiguration.Web.PurgeConnectionPoints" Codebehind="PurgeConnectionPoints.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   
    <h2>
        Purge Connections
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
                    <table align="center" class="ManageKeysTableStyle" >
                    <tr>
                    <td colspan="5" style="text-align:left;padding-left:10px;padding-right:10px;color:#c1c1c1">
            <p >
            This action will delete all current connections to Host <asp:Label ID="RemoteHost" runat="server"></asp:Label>.
            For on-premise deployments with Configuration Service dynamic clustering, if no remote hosts are online when you purge, the remote service domain 
            will continue to send notifications to this connected client domain 
            by design. Hence new connections will continue to be received as new remote
            hosts are brought online. The purge operation does not end/remove your actual Connected Service definition 
            to the remote service domain; it simply removes current connections to this domain.
            
            </p>
            <p style="text-align:left">
            If you want to permanently remove the dynamic relationship with
            the remote service domain, simply use the Connected
            Services menu item and delete the Connected Service Definition(s) you have with this remote service domain. 
            This will also purge all existing connections in a one step process.
            </p>
            <p>Finally, to remove everything at once (meaning all connections and all connected service defintions to the remote service domain), you can delete the connected service
            user to this domain (typcial named '*csUser'). Use the Manage Users menu to accomplish this.</p>
            </td>
            </tr>
            <tr>
            <td colspan="5" style="margin-left:auto; margin-right:auto;">
      
    <!-- ************************************Buttons ***************************************************** -->
            <table style="margin-left:auto; margin-right:auto;">
                <tr>
                    <td style="padding-top:10px;padding-bottom:10px">
                    <asp:LinkButton  ID="Purge"  Text="Purge" runat="server" width="110px" 
                            Height="25px" CausesValidation="False" PostBackUrl="PurgeConnectionPoints.aspx" 
                            onclick="Purge_Click" CssClass="AddUpdateDeleteButton" TabIndex="1"></asp:LinkButton>
                   </td>
                   <td style="padding-top:10px;padding-bottom:10px">
                    <asp:LinkButton  ID="Cancel" Text="Cancel" runat="server" width="110px" 
                           Height="25px" CausesValidation="False" PostBackUrl="ConnectionPoint.aspx" 
                           onclick="Cancel_Click" CssClass="AddUpdateDeleteButton" TabIndex="2"></asp:LinkButton>
                   </td>
                </tr>
                  
            </table>  
            <br />
        </td>
        </tr>
        <tr>
           <td colspan="5" style="text-align:center">
           <asp:Label ID="Message" runat="server" Text="">
           </asp:Label>
           </td>
        </tr>
        </table>
 
          <br />
         
                    <asp:Label ID="ReturnLabel" runat="server" TabIndex="3" ></asp:Label>
                    <br /> <br />
                   </center>
</asp:Content>

