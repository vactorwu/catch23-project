<%@ Page Title="Configuration Service: Configuration Keys" Buffer="true"  EnableSessionState="false" MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True" Inherits="ConfigService.ServiceConfiguration.Web.Config" Codebehind="Config.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   
    <h2>
        Configuration Keys
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
                 Inherited settings are simply application global settings inherited from a base Configuration Service settings class.  Custom settings are application-specific settings the developer has defined
                 for the specific application.  These can be any settings you might otherwise have defined in the &lt;appSettings&gt; section of a .NET Web.config or App.Config file.
                 While stored in the central configuration database, these settings are cached locally in-memory, and used just like they came from a config file (although no code is necessary
                 to populate them into local variables).  The in-memory cache means access is as fast as accessing a local static variable. But, Configuration Service
                 keeps all nodes in sync when configuration updates are made on live systems--without having to deploy new application configuration files across nodes, and stop/re-start running nodes.  
                 </div>
<!-- ########################################## Main Content  ############################################# -->
                   <table align="center" style="border-collapse:collapse;border:0px">
                        <tr>
                        <td width="150px" style="text-align:center">
                        <span style="padding-top:20px;">
                        <asp:LinkButton ID="Basic" runat="server" CssClass="AuditButton"
                        CausesValidation="False" PostBackUrl="Audit.aspx" Height="25px" 
                                Width="110px" onclick="Basic_Click" TabIndex="2" >Basic</asp:LinkButton>
                        </span>
                        </td>
                        <td width="150px" style="text-align:center">
                        <asp:LinkButton ID="Detailed" CssClass="ConfigButton" CausesValidation="False"
                        runat="server" PostBackUrl="Audit.aspx"  Height="25px" Width="110px" 
                                onclick="Detailed_Click" TabIndex="3" >Detailed</asp:LinkButton>
                        </td>
                        <td width="150px" style="text-align:center">
                        <asp:LinkButton ID="Advanced" CssClass="AuditButton" CausesValidation="False"
                        runat="server" PostBackUrl="Audit.aspx"  Height="25px" Width="110px" 
                                onclick="Advanced_Click" TabIndex="4" >Advanced</asp:LinkButton>
                        </td>
                        </tr>
                </table>
                <br />
                        <asp:LinkButton ID="Define" CssClass="AuditButton" CausesValidation="False"
                        runat="server" PostBackUrl="Audit.aspx"   Height="30px" 
              Width="110px" TabIndex="5" >Define Keys</asp:LinkButton>
                        <br /><br />
                            Target Settings Group: <asp:Label ID="TargetContract" runat="server" Text="TargetContract"></asp:Label>
                         <br /><div style="font-size:1.3em;color:#0B0352"><asp:Label ID="Scope" runat="server" Text="Scope"></asp:Label></div>
                     
             <table class="ConnectionPointTableStyle" style="background-image: url('Images/connpointfill.png');text-align:center;table-layout:fixed;" align="center" width="1100">
                 <col width="225px" />
                 <col width="380px" />
                 <col width="420px" />
                 <col width="75px" />
            <tr>
                 <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid">Setting Name</th>
                 <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid">Current Value</th>
                 <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid">Description</th>
                <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid">Change Value</th>
            </tr>
           
          <asp:Repeater id="ConfigRepeater" runat="server">
          <HeaderTemplate>
          </HeaderTemplate>
          <ItemTemplate>
            <tr>
                 <td class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                <asp:Label ID="Setting" runat="server" Text=""></asp:Label></td>
                <td class="TradeConfigTDSettingStyle1a" >
                <asp:Label ID="Value" runat="server" Text=""></asp:Label></td>
                <td class="TradeConfigTDSettingStyle1a">
                <asp:Label ID="Description" runat="server" Text="" ></asp:Label></td>
                <td class="TradeConfigTDSettingStyle1a">
                <asp:Label ID="Update" runat="server" Text="" ></asp:Label></td>
           </tr>      
         </ItemTemplate>
         <FooterTemplate>
       </FooterTemplate>
</asp:Repeater>
          
           
         </table>

         <br />
                
               <asp:Label ID="ReturnLabel" runat="server" CssClass="ReturnLinkStyle" 
              TabIndex="6"></asp:Label>
               <br />
                <br />
    </center>
</asp:Content>

