<%@ Page Title="Configuration Service: Login" Language="C#" Buffer="true" MasterPageFile="~/Site.master" AutoEventWireup="True" Inherits="ConfigService.ServiceConfiguration.Web.Login" Codebehind="Login.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   
    <h2>
        Configuration Service Login
    </h2>
    <br />
    <table class="LoginTableStyle" align="center" style="border-style:none; width:800px; ">
    <tr>
            <td style="text-align:center;border-top:solid #39454F 2px;border-left:solid #39454F 2px;border-right:solid #39454F 2px;">
                    <table class="LoginTableStyle" align="center" style="width:800px;border:2px;border:2px;table-layout:fixed">
                    <col width="150"/>
                    <col width="500"/>
                    <col width="150"/>
                    <tr>
                    <td>
                    </td>
                    <td>
                    <br />
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/globe.png" 
                            Height="70px" Width="126px" />
                    </td>
                    <td></td>
                    </tr>
                    <tr>
                        <td style="font-size:14px;width:500px;color:#FFFFFF" colspan="3"> 
                        <br />
                        Enter Your Credentials
                        <br /><br />
                       <span style="font-size:.8em;padding-left:15px;color:#c1c1c1">
                           <asp:Label ID="LocalRunning" runat="server" Text="Label"></asp:Label>
                       For the Microsoft live Azure-deployed application, use userid: 'demoadmin'; password: 'demo' for all service domains.
                       <br />
                       For your own Azure StockTrader service domains, change the login URI to use your Azure address (***.cloudapp.net): pwd = 'admin' on initial install.
                       </span>
                       </td>
                   </tr>
                   </table>
            </td>
    </tr>
    <tr>
            <td style="border-left:2px solid #39454F;border-right:2px solid #39454F; background-color: #616161;">
            <br />
           
                <table class="LoginTableStyle" align="center" style="width:800px;border:2px;table-layout:fixed">
                <col width="300px"/>
                <col width="400px"/>
                <col width="100px"/>
                <tr>
                    
                    <td style="color:#CCCCCC; text-align:right;width:250px" >
                    User Name:
                    </td>
                    <td style="text-align:left;padding-left:10px">
                        <asp:TextBox ID="uid" runat="server" CssClass="textEntry"
                    Width="200px" MaxLength="50" CausesValidation="True" TabIndex="1"></asp:TextBox>
                    </td>
                    <td></td>
                 </tr>
                 <tr>
                    
                    <td style="color:#CCCCCC;text-align:right" class="style1">
                     Password:
                    </td>
                    <td style="text-align:left;padding-left:10px">
                         <asp:TextBox ID="pwd" runat="server" CssClass="textEntry" 
                            Width="200px" MaxLength="50" TextMode="Password" TabIndex="2"></asp:TextBox>
                    </td>
                    <td></td>
                   </tr>
                   <tr>
                   <td style="border-bottom: #000000 1px solid">&nbsp;</td>
                   <td style="border-bottom: #000000 1px solid">&nbsp;</td>
                   <td style="border-bottom: #000000 1px solid">&nbsp;</td>
                   </tr>
                    <tr>
                        <td style="font-size:14px;width:500px;text-align:center;color:#FFFFFF;" colspan="3"> 
                        <br />
                        Target Configuration Service Details
                        <br /> 
                        </td>
                   </tr>
                </table>
                <br />
                 <br />
            </td>
    </tr>
    <tr>
            <td style="border-left:2px solid #39454F;border-right:2px solid #39454F;border-bottom:2px solid #39454F">
            <table class="LoginTableStyle" align="center" style="width:800px;border:2px #39454F;table-layout:fixed">
            <col width="300"/>
            <col width="400"/>
            <col width="100"/>
            
            <tr>
                <td style="text-align:right;border-left:2px;color:#CCCCCC;" >
                    Select From Known:
                </td>
                 <td style=" padding-left:7px; padding-right:20px; text-align:left">
                    <asp:CheckBox ID="CheckBoxAddress" runat="server" CssClass="textEntry" 
                         oncheckedchanged="CheckBoxAddress_CheckedChanged" AutoPostBack="true" TabIndex="3" />
                </td>
                <td style="border-right:2px;" ></td>
            </tr>
              <tr>
                <td style="text-align:right;border-left:2px;color:#CCCCCC;" >
                    Service Domain Name:
                </td>
                 <td style=" padding-left:7px; padding-right:20px; text-align:left;">
                  <asp:DropDownList ID="DropDownListNames" runat="server" CssClass="textEntry"
                         Width="400px" Visible="true" AutoPostBack="true" TabIndex="4" 
                         onselectedindexchanged="DropDownListNames_SelectedIndexChanged"> </asp:DropDownList>
                     
                </td>
                <td style="border-right:2px;" ></td>
            </tr>
            <tr>
                 <td style="text-align:right;border-left:2px;color:#CCCCCC;" >
                    Configuration Service Address:
                </td>
                  <td style="border: 0px 0px 0px 0px; padding-left:10px; padding-right:20px; text-align:left;color:#0B0352;font-size:.85em">
                   <asp:Label ID="ServiceAddress" runat="server" 
                         Visible="true" Width="393px"></asp:Label>
                     <asp:TextBox ID="textboxaddress" runat="server" CssClass="textEntry"
                         Visible="true" Width="396px"></asp:TextBox>
                </td>
                 <td></td>
             </tr>
             <tr>
                <td style="text-align:right; border-left:2px;color:#CCCCCC;" >
                    Client Configuration:
                </td>
                <td style="padding-left:10px; padding-right:20px; text-align:left;color:#0B0352;font-size:.85em">
                <asp:Label ID="ClientConfiguration" runat="server" 
                         Visible="true" Width="393px"></asp:Label>
                    <asp:DropDownList ID="clients" runat="server" CssClass="textEntry" 
                    Width="400px" TabIndex="5" 
                        onselectedindexchanged="clients_SelectedIndexChanged">
                     </asp:DropDownList>
                </td>
                <td style="border-right:2px;"></td>
              </tr>
              <tr><td></td><td></td><td></td></tr>
              <tr>
                    <td ></td>
                     <td style="text-align:left" >
                          <table align="left" style="border-collapse:collapse;border:0px">
                        <tr>
                            <td style="text-align:center;padding-top:10px;padding-left:8px;">
                              <asp:Button ID="LoginButton1" CausesValidation="False" PostBackUrl="Login.aspx" 
                                    runat="server" BackColor="Black" ForeColor="White" 
                              Text="Login" TabIndex="6" onclick="LoginButton1_Click" Height="30px" 
                                    Width="80px" />
                            
                            </td>
                        </tr>
                        </table>  
                        <br />
                     
                        
                     
                        <br />
                        <br />
                      <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List"
                                ValidationGroup="Login" ForeColor="Maroon"/>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorUserID" runat="server" ControlToValidate="uid"
                                Display="None" ErrorMessage="Please enter a user name!" 
                              ValidationGroup="Login" ForeColor="Maroon"></asp:RequiredFieldValidator><br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorPassword" 
                              runat="server" ControlToValidate="pwd"
                                Display="None" ErrorMessage="Please enter a password!" 
                              ValidationGroup="Login" ForeColor="Maroon"></asp:RequiredFieldValidator>                                                                         
                    </td>
                    <td></td>          
                 
              </tr>
           </table>  
          <center> <asp:Label ID="InValid" runat="server" ForeColor="Maroon" Width="800px"></asp:Label><br /><br /></center>
          </td>
     </tr>
 </table>
 <br />
 <br />
</asp:Content>

