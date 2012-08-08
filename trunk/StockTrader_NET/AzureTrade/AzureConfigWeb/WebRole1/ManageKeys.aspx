<%@ Page Title="Configuration Service: Manage Keys" Buffer="true" Language="C#" MasterPageFile="~/Site.master"  EnableSessionState="false" MaintainScrollPositionOnPostback="true" AutoEventWireup="True" Inherits="ConfigService.ServiceConfiguration.Web.ManageKeys" Codebehind="ManageKeys.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   
    <h2>
        Define Key
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
            <col width="200" />
            <col width="700" />
            <tr>

            <td class="TradeHostServiceTDStyle">
                Configuration Key Display Name:</td>
            <td class="TradeHostServiceTDStyle2">
            <asp:TextBox ID="TextBoxDisplayName" runat="server" Width="600px" 
                    CssClass="textEntry" TabIndex="1"></asp:TextBox>
            </td>
       </tr>
       <tr>
            <td class="TradeHostServiceTDStyle">
                Settings Class Field Name:</td>
           <td class="TradeHostServiceTDStyle2">
           <span style="color:#FFFF99">Make sure you have defined the exact name of this field in your Settings.cs class. You cannot add a new key unless this has been
           defined first in the Settings class.</span> 
            <asp:TextBox ID="TextBoxSettingName" runat="server" Width="600px"  
                   CssClass="textEntry" TabIndex="2"></asp:TextBox>
            </td>
       </tr>
         <tr>
            <td class="TradeHostServiceTDStyle">
                Settings Group Name:</td>
           
           <td class="TradeHostServiceTDStyle2">
           <asp:CheckBox ID="CheckBoxGroupName" runat="server" AutoPostBack="True" Checked="True"  CssClass="textEntry" OnCheckedChanged="CheckBoxGroupName_CheckedChanged" />
               Select from existing Groups.  De-selecting allows you to create new Setting Groups.<br />
               <asp:DropDownList ID="DropDownListGroupName" runat="server"  
                   CssClass="textEntry" Width="600px" TabIndex="3">
               </asp:DropDownList>
            <asp:TextBox ID="TextBoxGroupName" runat="server" Width="600px"  
                   CssClass="textEntry" Visible="False" TabIndex="4"></asp:TextBox>
            </td>
       </tr>
   <tr>
            <td class="TradeHostServiceTDStyle">
                Data Type:</td>
           <td class="TradeHostServiceTDStyle2">
           
           
               <asp:TextBox ID="TextBoxDataType" runat="server" Width="87px"  
                   CssClass="textEntry" TabIndex="5"></asp:TextBox>
               <br />
               Note: '<strong>radiobutton</strong>' will use a ';' deliminated list from Key Valid Values if you
               want the key update page to display a selection of valid values vs. a text box for updates; '<strong>nodisplay</strong>' can be used for long serialized
               data if you do not want it to be displayed in ConfigWeb. Entering datatype <strong>password</strong> will also mark the key as non-viewable.</td>
       </tr>
        <tr>
            <td class="TradeHostServiceTDStyle">
                Key Description:</td>
           <td class="TradeHostServiceTDStyle2" >
            <asp:TextBox ID="TextBoxDescription" runat="server" Width="600px" Height="60px" 
                   TextMode="MultiLine"  CssClass="textEntry" TabIndex="6"></asp:TextBox>
            </td>
       </tr>
       <tr>
            <td class="TradeHostServiceTDStyle">
                Key Valid Values:</td>
           <td class="TradeHostServiceTDStyle2" >
            <asp:TextBox ID="TextBoxValidValues" runat="server" Width="600px"  
                   CssClass="textEntry" TabIndex="7" TextMode="MultiLine"></asp:TextBox>
            </td>
       </tr>
   
     
     <tr>
            <td class="TradeHostServiceTDStyle">
                Key Level:</td>
           <td class="TradeHostServiceTDStyle2" >
                  <asp:RadioButtonList ID="RadioButtonListLevel" runat="server" 
                      AutoPostBack="False" TabIndex="8" >
                  </asp:RadioButtonList> </td>
           
       </tr>
     


       <tr>
            <td class="TradeHostServiceTDStyle">
                Read Only:</td>
           <td class="TradeHostServiceTDStyle2" >
                  <asp:RadioButtonList ID="RadioButtonReadOnly" runat="server" 
                      AutoPostBack="False" TabIndex="9" >
                  </asp:RadioButtonList></td>
       </tr>
        <tr>
            <td class="TradeHostServiceTDStyle">
               Initial  Key Value:</td>
           <td class="TradeHostServiceTDStyle2">
            <asp:TextBox ID="TextBoxValue" runat="server" Width="600px"  CssClass="textEntry" 
                   Height="60px" TextMode="MultiLine" TabIndex="10"></asp:TextBox>
            </td>
       </tr>
        <tr>
            <td class="TradeHostServiceTDStyle">
               Display Order:</td>
           <td class="TradeHostServiceTDStyle2">
            <asp:TextBox ID="TextBoxDisplayOrder" runat="server" Width="30px"  
                   CssClass="textEntry" TabIndex="11"></asp:TextBox>
            </td>
       </tr>
       <tr>
       <td colspan="2">
        <!-- ************************************Buttons ***************************************************** -->
            <table align="center" >
                <tr>
                    <td style="padding-top:10px;padding-bottom:10px">
                    <asp:LinkButton  ID="Add"  Text="Add" runat="server" width="110px" Height="25px" 
                            CausesValidation="False" PostBackUrl="ManageKeys.aspx" onclick="Add_Click" 
                            CssClass="AddUpdateDeleteButton" TabIndex="12"></asp:LinkButton>
                   </td>
                   <td style="padding-top:10px;padding-bottom:10px">
                    <asp:LinkButton  ID="Update" Text="Update" runat="server" width="110px" 
                           Height="25px" CausesValidation="False" PostBackUrl="ManageKeys.aspx" 
                           onclick="Update_Click" CssClass="AddUpdateDeleteButton" TabIndex="13"></asp:LinkButton>
                   </td>
                   <td style="padding-top:10px;padding-bottom:10px">
                    <asp:LinkButton  ID="Delete" Text="Delete" runat="server" width="110px" 
                           Height="25px" CausesValidation="False" PostBackUrl="ManageKeys.aspx" 
                           onclick="Delete_Click" CssClass="AddUpdateDeleteButton" TabIndex="14"></asp:LinkButton>
                    </td>
                </tr>
            </table>
            </td>
            </tr>
             <tr>
                    <td colspan="2" style="text-align:center">
                    <asp:Label ID="UpdateMessage" runat="server" ></asp:Label>
                    </td>
             </tr>
             <tr>
             <td colspan="2">
         <span style="font-size:13px;font-weight:normal">
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="CS" DisplayMode="List" ForeColor="Maroon" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidatorDisplayName" runat="server"
            ControlToValidate="TextBoxDisplayName" Display="None" ErrorMessage="Please enter a display name"
            ValidationGroup="CS" ForeColor="Maroon"></asp:RequiredFieldValidator>
        <asp:RequiredFieldValidator ID="RequiredFieldValidatorSettingName" runat="server" ControlToValidate="TextBoxSettingName"
            Display="None" ErrorMessage="Please enter a setting name" ValidationGroup="CS" ForeColor="Maroon"></asp:RequiredFieldValidator><asp:RequiredFieldValidator ID="RequiredFieldValidatorDataType" runat="server" ControlToValidate="TextBoxDataType"
            Display="None" ErrorMessage="Please enter a data type" ValidationGroup="CS" ForeColor="Maroon"></asp:RequiredFieldValidator><asp:RangeValidator
                ID="RangeValidatorDisplayOrder" runat="server" ControlToValidate="TextBoxDisplayOrder"
                Display="None" ErrorMessage="Display Order must be an integer" MaximumValue="2000000"
                MinimumValue="-200000" ValidationGroup="CS" Type="Integer" ForeColor="Maroon"></asp:RangeValidator>
                 <asp:RequiredFieldValidator ID="RequiredFieldValidatorValid" runat="server" ControlToValidate="TextBoxValidValues"
            Display="None" ErrorMessage="Please enter a valid values description for this key"
            ValidationGroup="CS" ForeColor="Maroon"></asp:RequiredFieldValidator>
        <asp:RequiredFieldValidator ID="RequiredFieldValidatorValue" runat="server" ControlToValidate="TextBoxValue" Display="None" ErrorMessage="Please enter an initial starting value for this key"
            ValidationGroup="CS" ForeColor="Maroon"></asp:RequiredFieldValidator>
        <asp:RequiredFieldValidator ID="RequiredFieldValidatorDisplayOrder" runat="server"
            ControlToValidate="TextBoxDisplayOrder" Display="None"
            ErrorMessage="Please enter a display order for the key" ValidationGroup="CS" ForeColor="Maroon"></asp:RequiredFieldValidator><br />
            </span>
             </td>
             </tr>
       </table>
       <br />
         
          <br />
                    <asp:Label ID="ReturnLabel" runat="server" TabIndex="15" ></asp:Label>
                    <br /> <br />
                    </center>
</asp:Content>

