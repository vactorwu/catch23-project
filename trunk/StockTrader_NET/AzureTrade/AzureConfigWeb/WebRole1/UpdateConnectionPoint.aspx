<%@ Page Title="Configuration Service: Delete Connection Point" Buffer="true" Language="C#" EnableSessionState="false" MasterPageFile="~/Site.master"  MaintainScrollPositionOnPostback="true" AutoEventWireup="true" Inherits="ConfigService.ServiceConfiguration.Web.UpdateConnectionPoint" Codebehind="UpdateConnectionPoint.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   
    <h2>
        Delete Connection Point
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
        Connection from:  <asp:Label ID="ClientService" runat="server" ></asp:Label>&nbsp;<br />
        <asp:Label ID="Message" runat="server" ForeColor="Maroon"></asp:Label><br />
        <br />

            <table class="ConnectionPointTableStyle" style="background-image: url('Images/connpointfill.png');text-align:center;table-layout:fixed;" align="center" width="1100">
             <tr>
                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Remote Service Host</th>
                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Remote Address</th>
                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Action</th>
             </tr>
              <tr>
                    <td class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                    <asp:Label ID="ServiceHost" runat="server"></asp:Label></td>
                    
                    <td class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                        <asp:Label ID="AddressLabel" runat="server"></asp:Label></td>
                    <td class="TradeConfigTDSettingStyle1a" style="text-align:center;font-size:1.0em">Delete
                   </td>
                </tr>   
           </table> 
           <br />
           <asp:LinkButton  ID="Delete" Text="Delete" runat="server" width="110px" 
                           Height="25px" CausesValidation="False" PostBackUrl="UpdateConnectionPoint.aspx" 
                           onclick="Delete_Click" CssClass="AddUpdateDeleteButton" TabIndex="2"></asp:LinkButton>
            <br />
                <br />
                    <asp:Label ID="ReturnLabel" runat="server" TabIndex="3" ></asp:Label>
                    <br /> <br />
                   </center>
</asp:Content>

