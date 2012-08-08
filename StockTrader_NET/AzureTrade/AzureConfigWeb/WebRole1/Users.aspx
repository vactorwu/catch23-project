<%@ Page Title="Configuration Service: Users" Buffer="true" Language="C#" MasterPageFile="~/Site.master" EnableSessionState="false" AutoEventWireup="true" Inherits="ConfigService.ServiceConfiguration.Web.Users" Codebehind="Users.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   
    <h2>
        Users
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
 <asp:LinkButton ID="AddUser" runat="server" PostBackUrl="Users.aspx" Height="30px" 
               Width="120px" onclick="AddUser_Click" CssClass="AddConnButton" 
         TabIndex="1" >Add User</asp:LinkButton>    
               <br /><br />
                
         
             <table class="ConnectionPointTableStyle" style="background-image: url('Images/connpointfill.png');text-align:center;table-layout:fixed;" align="center" width="1100">
             <tr>
                 <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid;">User Key</th>
                 <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid;">User Name</th>
                 <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid;">Is Local User</th>
                 <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid;">Rights</th>
                 <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid;">Edit</th>
             </tr>
            
          <asp:Repeater id="UserRepeater" runat="server">
          <HeaderTemplate>
          </HeaderTemplate>
          <ItemTemplate>
            <tr>
                 <td class="TradeConfigTDSettingStyle1a" style="text-align:left;">
                    <asp:Label ID="UserKey" runat="server" Text=""></asp:Label></td>
                 <td class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                    <asp:Label ID="UserName" runat="server" Text=""></asp:Label></td>
                 <td class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                    <asp:Label ID="Local" runat="server" Text=""></asp:Label></td>
                <td class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                    <asp:Label ID="Rights" runat="server" Text=""></asp:Label></td>
                <td class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                    <asp:Label ID="Edit" runat="server" Text=""></asp:Label></td>
           </tr>      
         </ItemTemplate>
         <FooterTemplate>
       </FooterTemplate>
       </asp:Repeater>
       </table>
       <br />
       
                <asp:Label ID="ReturnLabel" runat="server" 
         CssClass="ReturnLinkStyle" TabIndex="2"></asp:Label>
       <br /><br />        
       </center>
</asp:Content>

