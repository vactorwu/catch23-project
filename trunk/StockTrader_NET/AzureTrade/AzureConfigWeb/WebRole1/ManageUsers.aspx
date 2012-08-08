<%@ Page Title="Configuration Service: Manage Users" Buffer="true" Language="C#" EnableSessionState="false" MasterPageFile="~/Site.master"  MaintainScrollPositionOnPostback="true" AutoEventWireup="true" Inherits="ConfigService.ServiceConfiguration.Web.ManageUsers" Codebehind="ManageUsers.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   
    <h2>
       Manage Users
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
                     <td class="TradeHostServiceTDStyle" style="text-align:center;font-size:1.1em;color:#FFFFFF" colspan="2">
                Enter User Details</td>
                    </tr>
                    <tr>

                   
            <td class="TradeHostServiceTDStyle">
                User Name:</td>
             <td class="TradeHostServiceTDStyle2">
            <asp:TextBox ID="TextBoxUserName" runat="server" Width="300px" CssClass="textEntry" 
                     TabIndex="1"></asp:TextBox>
            </td>
       </tr>
        <tr>
            <td class="TradeHostServiceTDStyle">
                Password:</td>
           <td class="TradeHostServiceTDStyle2">
            <asp:TextBox ID="TextBoxPassword" runat="server" Width="300px" TextMode="Password" 
                   CssClass="textEntry" TabIndex="2"></asp:TextBox>
            </td>
       </tr>
        <tr>
            <td class="TradeHostServiceTDStyle">
                Confirm Password:</td>
           <td class="TradeHostServiceTDStyle2">
            <asp:TextBox ID="TextBoxPasswordConfirm" runat="server" Width="300px" TextMode="Password" 
                   CssClass="textEntry" TabIndex="2"></asp:TextBox>
            </td>
       </tr>
       
        <tr>
        <td class="TradeHostServiceTDStyle">
                Local User:</td>
            <td class="TradeHostServiceTDStyle2">
       <table>
                <tr>
                     <td style ="text-align:left;vertical-align:middle">
                     <br />
                     <asp:RadioButtonList ID="RadioButtonListLocalUser" runat="server" 
                             AutoPostBack="True" 
                             OnSelectedIndexChanged="RadioButtonListLocalUser_SelectedIndexChanged" 
                             TabIndex="3">
                     </asp:RadioButtonList></td>
                </tr>
       </table>
       </td>
       </tr>
       
       <tr>
            <td class="TradeHostServiceTDStyle">
                Rights:</td>
           <td class="TradeHostServiceTDStyle2">
            <table>
            <tr>
                <td style ="text-align:left;vertical-align:middle">
                  <br />
                  <asp:RadioButtonList ID="RadioButtonListRights" runat="server" 
                        AutoPostBack="True"  
                        OnSelectedIndexChanged="RadioButtonListRights_SelectedIndexChanged" 
                        TabIndex="4">
                  </asp:RadioButtonList></td>
            </tr>
            </table>
            <br />
            </td>
       </tr>
       <tr>
       <td colspan="2">
        <!-- ************************************Buttons ***************************************************** -->
            <table align="center" >
                <tr>
                    <td style="padding-top:10px;padding-bottom:10px">
                    <asp:LinkButton  ID="Add"  Text="Add" runat="server" width="110px" Height="25px" 
                            CausesValidation="False" PostBackUrl="ManageUsers.aspx" onclick="Add_Click" 
                            CssClass="AddUpdateDeleteButton" TabIndex="5"></asp:LinkButton>
                   </td>
                   <td style="padding-top:10px;padding-bottom:10px">
                    <asp:LinkButton  ID="Update" Text="Update" runat="server" width="110px" Height="25px" CausesValidation="False" PostBackUrl="ManageUsers.aspx" onclick="Update_Click" CssClass="AddUpdateDeleteButton" TabIndex="6">
                    Update</asp:LinkButton>
                   </td>
                   <td style="padding-top:10px;padding-bottom:10px">
                    <asp:LinkButton  ID="Delete" Text="Delete" runat="server" width="110px" 
                           Height="25px" CausesValidation="False" PostBackUrl="ManageUsers.aspx" 
                           onclick="Delete_Click" CssClass="AddUpdateDeleteButton" TabIndex="7"></asp:LinkButton>
                    </td>
                </tr>
                 
            </table>
            </td>
            </tr>
            <tr>
            <td colspan="2" style="text-align:center">
             <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="User" DisplayMode="List" ForeColor="Maroon" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidatorUserName" runat="server"
            ControlToValidate="TextBoxUserName" Display="None"
            ErrorMessage="Please enter a user name" ValidationGroup="User" ForeColor="Maroon"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
            ControlToValidate="TextBoxPassword" Display="None"
            ErrorMessage="Please enter a password" ValidationGroup="User" ForeColor="Maroon"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="CompareValidatorPwd" ForeColor="Maroon" runat="server" ControlToCompare="TextBoxPasswordConfirm"
            ControlToValidate="TextBoxPassword" Display="None" ErrorMessage="Passwords Do Not Match!"
            ValidationGroup="User"></asp:CompareValidator>

            </td>
            </tr>
            <tr>
                    <td colspan="2" style="text-align:center">
                    <asp:Label ID="UpdateMessage" runat="server" ></asp:Label>
                    </td>
             </tr>
            
       </table>
        <br />
         
         
                    <asp:Label ID="ReturnLabel" runat="server" TabIndex="8" ></asp:Label>
                    <br /> <br />
                    </center>
     
    
</asp:Content>

