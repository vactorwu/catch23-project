<%@ Page Title="Configuration Service: Setting Update" Buffer="true" EnableSessionState="false" MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True" Inherits="ConfigService.ServiceConfiguration.Web.ConfigUpdate" Codebehind="ConfigUpdate.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   
    <h2>
        Update Configuration Key
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
                 <div style="font-size:.8em;padding-left:35px;padding-bottom:10px;text-align:left;padding-right:35px"> 
                 You can change a setting's value here, and all running nodes will automatically receieve the new setting and be updated. The setting update will be pushed through the network to the correct service domain automatically.
                 Once updated, new nodes for that domain will always start up with the updated value since it has been persisted into the configuration repository, which nodes read on startup. Running nodes are updated live via the node service and reflection.
                 Base-level validation is provided on integral types, but developers can easily supply their own server-side validation logic for custom settings, and perform custom actions across all nodes when specific settings change, if they desire. For StockTrader 5,
                 a key custom setting for the Web application is AccessMode, which determines which Business Service domain to utilize, and over which network/encoding/security standard. The OrderMode setting
                 for the StockTrader Business Service domain serves a similar purpose, specifying which remote Order Processor Service domain to use, and via what type of communication channel.
                 </div>
<!-- ########################################## Main Content  ############################################# -->
            <table class="ConnectionPointTableStyle" style="background-image: url('Images/connpointfill.png');text-align:center;table-layout:fixed;" align="center" width="1100">
                        <col width="200px" />
                        <col width="300px" />
                        <col width="200px" />
                        <col width="300px" />
                        <col width="100px" />
                        <tr>

                      <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid">Setting Name</th>
                      <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid">Current Value</th>
                      <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid">Setting Description</th>
                      <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid">Valid Values</th>
                      <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid">Last Updated</th>
          </tr>
          <tr>
                  <td class="TradeConfigTDSettingStyle1a" style="text-align:center;"><asp:Label ID="KeyName" runat="server"></asp:Label></td>
                  <td class="TradeConfigTDSettingStyle1a" style="text-align:center;"><asp:Label ID="Value" runat="server"></asp:Label></td>
                  <td class="TradeConfigTDSettingStyle1a" style="text-align:center;text-align:left"><asp:Label ID="Description" runat="server"></asp:Label></td>
                  <td class="TradeConfigTDSettingStyle1a" style="text-align:center;"><asp:Label ID="ValidValues" runat="server"></asp:Label></td>
                  <td class="TradeConfigTDSettingStyle1a" style="text-align:center;"><asp:Label ID="LastUpdated" runat="server" ></asp:Label></td>
            </tr>
            </table>
           
            <table width="1100" class="ConfigUpdateTableStyle" align="center" style="table-layout:fixed;border:#39454F 2px solid;"> 
            <col width="1100" />
            <tr>
                <td style="color:#CCCCCC;text-align:center;" >
                <br />
                  Enter/Select New Value:<br /><br />
                <asp:TextBox ID="NewValue" CssClass="textEntry" runat="server" Width="500px" 
                        Font-Size="13px" Height="60px" TextMode="MultiLine" TabIndex="1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                
                 <td style="color:#CCCCCC;text-align:center;">
                    <table align="center" style="border:0px;" >
                    <tr>                   
                            <td style="color:#CCCCCC;text-align:center;">
                             <div style="text-align:left">
                            <asp:RadioButtonList ID="RadioButtonListValidValues" runat="server" TabIndex="1">
                            </asp:RadioButtonList></div>
                           </td>
                    </tr>
                    </table>
                 </td>
             </tr>
             <tr>
             <td style="text-align:center;">
                    <br />
                                 <asp:LinkButton  ID="Update" runat="server" width="120px" Height="30px"
                                 CausesValidation="False" PostBackUrl="ConfigUpdate.aspx" 
                        CssClass="ConfigUpdateButton" TabIndex="2">
                                 Update</asp:LinkButton>
                                 <br /><br />
                                <asp:Label ID="UpdateMessage" runat="server"></asp:Label> <br /><br />
            </td>
            </tr>
            </table>
                   <br />
                        <asp:Label ID="ReturnLabel" runat="server" TabIndex="3" ></asp:Label>
                    <br />
        </center>
        <br />
        <br />
</asp:Content>

