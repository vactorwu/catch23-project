<%@ Page Title="Configuration Service: Define Keys" Buffer="true" EnableSessionState="false" MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="ConfigService.ServiceConfiguration.Web.ConfigDefine" Codebehind="ConfigDefine.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   
    <h2>
        Manage Configuration keys
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
                   <table align="center" style="border-collapse:collapse;border:0px">
                        <tr>
                        <td width="150px" style="text-align:center">
                        <span style="padding-top:20px;">
                        <asp:LinkButton ID="Basic" runat="server" CssClass="AuditButton"
                        CausesValidation="False" PostBackUrl="ConfigDefine.aspx" Height="25px" 
                                Width="110px" onclick="Basic_Click" TabIndex="2" >Basic</asp:LinkButton>
                        </span>
                        </td>
                        <td width="150px" style="text-align:center">
                        <asp:LinkButton ID="Detailed" CssClass="ConfigButton" CausesValidation="False"
                        runat="server" PostBackUrl="ConfigDefine.aspx"  Height="25px" Width="110px" 
                                onclick="Detailed_Click" TabIndex="3" >Detailed</asp:LinkButton>
                        </td>
                        <td width="150px" style="text-align:center">
                        <asp:LinkButton ID="Advanced" CssClass="AuditButton" CausesValidation="False"
                        runat="server" PostBackUrl="ConfigDefine.aspx"  Height="25px" Width="110px" 
                                onclick="Advanced_Click" TabIndex="4" >Advanced</asp:LinkButton>
                        </td>
                        </tr>
                        <tr>
                        <td colspan="3" style="text-align:center">
                        <br/>
                        <asp:LinkButton ID="Add" CssClass="AuditButton" CausesValidation="False"
                        runat="server" PostBackUrl="ConfigDefine.aspx"  Height="25px" Width="110px" 
                                onclick="AddKey_Click" TabIndex="4" >Add New Key</asp:LinkButton>
                        </td>
                        </tr>
                    </table>
                     <br />
                     Level: <asp:Label ID="Level" runat="server" Text=""></asp:Label>
                     <br /><br />
                     Target Settings Group: <asp:Label ID="Contract" runat="server" Text="ContractLabel"></asp:Label>
                     <br /><div style="font-size:1.3em;color:#0B0352"><asp:Label ID="Scope" runat="server" Text="Scope"></asp:Label></div>
                        <table class="ConnectionPointTableStyle" style="background-image: url('Images/connpointfill.png');text-align:center;table-layout:fixed;" align="center" width="1100">
                        <col width="110px" />
                        <col width="200px" />
                        <col width="80px" />
                        <col width="180px" />
                        <col width="40px" />
                        <col width="40px" />
                        <tr>
                             <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid">Display Name</th>
                             <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid">Settings Class Field Name</th>
                             <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid"> Data Type</th>
                             <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid">Description</th>
                             <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid">Display Order</th>
                             <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid">Edit</th>
                        </tr>
                        <asp:Repeater id="ConfigRepeater" runat="server">
                        <HeaderTemplate>
                        </HeaderTemplate>
                        <ItemTemplate>
                        <tr>
                            <td  class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                            <asp:Label ID="DisplayName" runat="server" Text=""></asp:Label></td>
                             <td  class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                            <asp:Label ID="SettingName" runat="server" Text=""></asp:Label></td>
                            <td  class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                            <asp:Label ID="DataType" runat="server" Text=""></asp:Label></td>
                            <td  class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                            <asp:Label ID="Description" runat="server" Text=""></asp:Label></td>
                            <td  class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                            <asp:Label ID="DisplayOrder" runat="server" Text=""></asp:Label></td>
                            <td  class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                            <asp:Label ID="Edit" runat="server" Text=""></asp:Label></td>
                        </tr>      
                         </ItemTemplate>
                         <FooterTemplate>
                       </FooterTemplate>
                        </asp:Repeater>
       </table>
       <br />
               <asp:Label ID="ReturnLabel" runat="server" CssClass="ReturnLinkStyle" 
              TabIndex="5"></asp:Label>
    <br />
    <br />
    </center>
</asp:Content>

