<%@ Page Title="Configuration Service: Remove Node" Buffer="true" Language="C#" MasterPageFile="~/Site.master" EnableSessionState="false" AutoEventWireup="true" Inherits="ConfigService.ServiceConfiguration.Web.RemoveNode" Codebehind="RemoveNode.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   
    <h2>
       Remove Downed Node
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
                    <td style="text-align:left;padding-left:10px;padding-right:10px;color:#c1c1c1">
            <p >
        This action will remove an offline node from the Configuration Service cluster definition. This does not remove the node instance
        from Windows Azure itself; for that you would use the Windows Azure Management Console.
        Use this page if necessary for a node that has crashed or failed to start. Such a node will not affect operations of the business service,
        but will tend to slow down ConfigWeb just a bit (since Configuration Service must wait a reasonable amount of time for a response).  
        A node should always be assumed online, even if it is not responding.
        Intermittent network connectivity could be the cause.  A node gracefully closed will always remove itself
        from the cluster definition and notify its peers. However, for nodes that have otherwise exited, if that node (machine
        as identified by dns name or IP address) is not intended to be brought back online,
        this operation will clear it from the configuration repository and peer-node communication. And if it is
        brought back online, even after removing it via this page, it will simply re-activate
        itself in the cluster and resume normal operations.  
            </p>
            
            </td>
            </tr>
            <tr>
            <td>
      
    <!-- ************************************Buttons ***************************************************** -->
            <table align="center" >
                <tr>
                    <td style="padding-top:10px;padding-bottom:10px">
                    <asp:LinkButton  ID="Delete"  Text="Remove Node" runat="server" width="110px" 
                            Height="25px" CausesValidation="False" PostBackUrl="RemoveNode.aspx" 
                            onclick="Delete_Click" CssClass="AddUpdateDeleteButton" TabIndex="1"></asp:LinkButton>
                   </td>
                   <td style="padding-top:10px;padding-bottom:10px">
                    <asp:LinkButton  ID="Cancel" Text="Cancel" runat="server" width="110px" 
                           Height="25px" CausesValidation="False" PostBackUrl="RemoveNode.aspx" 
                           onclick="Cancel_Click" CssClass="AddUpdateDeleteButton" TabIndex="2"></asp:LinkButton>
                   </td>
                </tr>
                  
            </table>   

            <br />
        </td>
        </tr>
        <tr>
           <td colspan="5">
          
           <asp:Label ID="Message" runat="server" Text=""></asp:Label>
           
           </td>
        </tr>
        </table>
 
          <br />
         
                    <asp:Label ID="ReturnLabel" runat="server" TabIndex="3" ></asp:Label>
                    <br /> <br />
                   </center>
</asp:Content>

