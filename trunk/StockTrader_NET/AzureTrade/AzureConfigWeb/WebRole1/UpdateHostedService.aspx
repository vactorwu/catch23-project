<%@ Page Title="Configuration Service: Manage Endpoints" Buffer="true" Language="C#" EnableSessionState="false" MasterPageFile="~/Site.master" AutoEventWireup="true"  MaintainScrollPositionOnPostback="true" Inherits="ConfigService.ServiceConfiguration.Web.UpdateHostedService" Codebehind="UpdateHostedService.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   
    <h2>
        Manage Hosted Service Endpoints
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

                   <table align="center" class="HSTableStyleIntro" >
                   <col width="400" />
                   <col width="700" />
                    <tr>
                        <td colspan="2" style="text-align:left;padding-left:10px;font-size:1.1em;font-weight:normal;background-color:#000000;color:#FFFFFF;border-bottom:0px #d8d8d8 solid">
                        Instructions</td>
                    </tr>
                    <tr>
                     <td colspan="2" style="padding-left:10px;border-bottom:2px #000000 solid;padding-right:10px;">
                     <p style="text-align:left;">
                        This page enables you to 
                        easily create WCF service endpoints for your Hosted Service. These endpoints can be assigned binding 
                        types, binding configurations, virtual paths and ports, as well as many other WCF properties. 
                        The Configuration Service supports all WCF properties, including code callbacks 
                        during initialization so any further customization can be done in your code, 
                        although this is typically not necessary.                         
                    </p>
                    <p style="text-align:left;">
                        This page also enables you to dynamically activate and deactivate endpoints, 
                        without stopping and starting nodes, or redeploying any configuration files. 
                        </p>
                        <p style="text-align:left;">
                        Pay particular attention to the <strong>load balancing</strong> type entry. 
                        On Windows Azure, the Azure Fabric Controller is automatically load balancing 
                        external requests to service endpoints, so this Azure address is supplied. In on-premise deployments, 
                        Configuration Service can automatically provide load balancing and 
                        service request-level failover for subscribing clients that also implement the 
                        Configuration Service--as demonstrated by the StockTrader sample.
                        </p>
                </td>
                </tr>
           
  <!-- ****************************************** Sub Table 1**************************************************** -->
           
                <tr>
                <td colspan="2" style="text-align:left;padding-left:10px;font-size:1.1em;font-weight:normal;background-color:#000000;color:#FFFFFF;border-bottom:0px #d8d8d8 solid">
                Hosted Service Type</td>
               </tr>
               <tr>
               <td colspan="2">
                <table align="center">
                <tr>
                    <td class="ConfigCSTDStyleRight" style="border:0px">
                      <asp:RadioButtonList ID="RadioButtonListServiceType" runat="server" 
                            CssClass="textEntry" Enabled="False" TabIndex="1">
                      </asp:RadioButtonList>
                    </td>
                </tr>
                </table>
                </td>
           </tr>
            <tr>
                <td colspan="2" style="text-align:left;padding-left:10px;font-size:1.1em;font-weight:normal;background-color:#000000;color:#FFFFFF;border-bottom:0px #d8d8d8 solid">
                Endpoint Details</td>
               </tr>
            <tr>
                <td class="ConfigCSTDStyleLeft">
                    Virtual Service Host Name:</td>
                <td class="ConfigCSTDStyleRight"><asp:Label ID="LabelVHostName" runat="server"></asp:Label></td>
           </tr>
           <tr>
               <td class="ConfigCSTDStyleLeft">
                    Implementation Class:</td>
                 <td class="ConfigCSTDStyleRight"><asp:Label ID="LabelClassName" runat="server"></asp:Label></td>
           </tr>
           <tr>
                <td class="ConfigCSTDStyleLeft">
                    Is a WorkFlowServiceHost:</td>
                 <td class="ConfigCSTDStyleRight"><asp:Label ID="WorkFlow" runat="server" ></asp:Label></td>
           </tr>
           <tr>
                <td class="ConfigCSTDStyleLeft">
                    Service Contract:</td>
                 <td class="ConfigCSTDStyleRight">
                     <asp:DropDownList ID="DropDownListContracts" runat="server" 
                         CssClass="textEntry" Width="455px" AutoPostBack="True" 
                         OnSelectedIndexChanged="DropDownListContracts_SelectedIndexChanged" 
                         TabIndex="2">
                     </asp:DropDownList></td>
           </tr>
           <tr>
                <td class="ConfigCSTDStyleLeft">
                Hosted Service Name:</td>
                <td class="ConfigCSTDStyleRight">
                    <asp:TextBox ID="TextBoxHostedServiceName" runat="server" Width="450px" 
                        CssClass="textEntry" TabIndex="3"></asp:TextBox></td>
           </tr>
           <tr>
               
               <td class="ConfigCSTDStyleLeft" style="vertical-align:top">
               Binding Type:
               </td>
               <td class="ConfigCSTDStyleRight">
                <table>
                <tr>
                    <td style="text-align:left;vertical-align:middle; height: 56px;">
                      <asp:RadioButtonList ID="RadioButtonListBindingType" runat="server" 
                            AutoPostBack="True" CssClass="textEntry" 
                            OnSelectedIndexChanged="RadioButtonListBindingType_SelectedIndexChanged" 
                            RepeatDirection="Horizontal" RepeatColumns="4">
                      </asp:RadioButtonList><br />
                    </td>
                </tr>
                </table>
                   <asp:Label ID="LabelBindingWarning" runat="server" ForeColor="#FFFF99"></asp:Label>
                   <p style="font-size:.9em">Note: The available bindings below will be dynamically updated as you change your binding
                   type selection above.</p>
                   </td>
           </tr>
           <tr>
                <td class="ConfigCSTDStyleLeft">
                    <strong><span style="text-decoration: underline">Host</span></strong>
                    Binding Configuration Name:</td>
                <td class="ConfigCSTDStyleRight">
                    <asp:DropDownList ID="DropDownListBindingConfig" runat="server" 
                        CssClass="textEntry" Width="455px" 
                    AutoPostBack="True" 
                        OnSelectedIndexChanged="DropDownListBindingConfig_SelectedIndexChanged" 
                        TabIndex="5"></asp:DropDownList>
                
               
                </td>
           </tr>
           <tr>
               <td class="ConfigCSTDStyleLeft" style="vertical-align:top">
                    Internal <strong><span style="text-decoration: underline">Client</span></strong>
                    Configuration Name:</td>
               <td class="ConfigCSTDStyleRight" >
                    <asp:DropDownList ID="DropDownListInternalClient" runat="server" 
                        CssClass="textEntry" Width="455px" TabIndex="6"></asp:DropDownList>
               
                <p style="font-size:.9em">Note that service hosts will include a client definition to their own endpoints within their configuration file.  This enables nodes to
                check themselves for consistency, and also enables the developer/admin to easily check the status of their endpoints/hosts without an external client. Addresses are
                determined dynamically on node startup.</p>
                </td>
           </tr>
           
           <tr>
                <td class="ConfigCSTDStyleLeft" style="vertical-align:top">Virtual Path:</td>
                <td class="ConfigCSTDStyleRight">
                    <asp:TextBox ID="TextBoxVirtualPath" runat="server" Width="450px" 
                        CssClass="textEntry" TabIndex="7"></asp:TextBox><br />
                    <asp:Label ID="LabelVDirWarning" runat="server" ForeColor="#FFFF99"></asp:Label></td>
           </tr>
            <tr>
                <td class="ConfigCSTDStyleLeft">Use Https:</td>
              <td class="ConfigCSTDStyleRight">
                   <asp:CheckBox ID="CheckBoxHttps" runat="server" 
                   OnCheckedChanged="CheckBoxHttps_CheckedChanged" AutoPostBack="True" 
                       TabIndex="8" />
                   <br />
                   <asp:Label ID="LabelHttpsWarning" runat="server" ForeColor="#FFFF99"></asp:Label></td>
           </tr>
           <tr>
                <td class="ConfigCSTDStyleLeft" style="vertical-align:top">Port:</td>
                <td class="ConfigCSTDStyleRight"><asp:TextBox ID="TextBoxPort" runat="server" 
                        CssClass="textEntry" Width="50px" TabIndex="9"></asp:TextBox><br />
                    <asp:Label ID="LabelPortWarning" runat="server" ForeColor="#FFFF99"></asp:Label></td>
           </tr>
           <tr>
               <td class="ConfigCSTDStyleLeft">Make This a Base Address:</td>
              <td class="ConfigCSTDStyleRight">
                 <asp:CheckBox ID="CheckBoxBaseAddress" runat="server" CssClass="textEntry" 
                      OnCheckedChanged="CheckBoxBaseAddress_CheckedChanged" AutoPostBack="True" 
                      TabIndex="10" />
                  <br />
                   <asp:Label ID="LabelBaseAddressWarning" runat="server" ForeColor="#FFFF99"></asp:Label>
              </td>
           </tr>
            <tr>
               <td class="ConfigCSTDStyleLeft">Add IMetaDataExchange (mex) endpoint for this address:</td>
               <td class="ConfigCSTDStyleRight">
                 <asp:CheckBox ID="CheckBoxMex" runat="server" CssClass="textEntry" TabIndex="11"/>
                </td>
           </tr>
            
           <tr>
                <td class="ConfigCSTDStyleLeft">Endpoint Behavior:</td>
               <td class="ConfigCSTDStyleRight">
                   <asp:CheckBox ID="CheckBoxEPB" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBoxEPB_CheckedChanged" />
                   <asp:DropDownList ID="DropDownListEPB" runat="server" CssClass="textEntry" 
                       Width="455px" TabIndex="12">
                   </asp:DropDownList>
                   <br />
                   <asp:Label ID="LabelEPB" runat="server" ForeColor="#FFFF99"></asp:Label></td>
           </tr>
           <tr>
                <td class="ConfigCSTDStyleLeft">Endpoint Identity:</td>
              <td class="ConfigCSTDStyleRight">
                   <asp:CheckBox ID="CheckBoxIdentity" runat="server" AutoPostBack="True" CssClass="textEntry" OnCheckedChanged="CheckBoxIdentity_CheckedChanged" />
                   <asp:DropDownList ID="DropDownListIdentity" runat="server" CssClass="textEntry" 
                       Width="455px" TabIndex="13">
                   </asp:DropDownList>
                   <br />
                   <asp:Label ID="LabelIdentity" runat="server" ForeColor="#FFFF99"></asp:Label></td>
           </tr>
            <tr>
               <td class="ConfigCSTDStyleLeft">Mark As Node Failover EndPoint:<br />
                  
                   </td>
               <td class="ConfigCSTDStyleRight" style="vertical-align:top">
                 <asp:CheckBox ID="CheckBoxFailover" runat="server" CssClass="textEntry" 
                       OnCheckedChanged="CheckBoxFailover_CheckedChanged" AutoPostBack="True" 
                       TabIndex="14" />
                  <span style="font-size:.9em">Applies only to Node Service endpoints.</span>
                 <br />
                  <asp:Label ID="LabelFailoverWarning" runat="server" ForeColor="#FFFF99"></asp:Label></td>
           </tr>
            
            <tr>
               <td class="ConfigCSTDStyleLeft" style="vertical-align:top">Load Balance Type:</td>
              <td class="ConfigCSTDStyleRight">
               
               <table>
                <tr>
                    <td style ="text-align:left;vertical-align:middle">
                      <asp:RadioButtonList ID="RadioButtonListLoadBalanceType" runat="server" 
                            AutoPostBack="True" CssClass="textEntry" 
                            OnSelectedIndexChanged="RadioButtonListLoadBalanceType_SelectedIndexChanged" 
                            TabIndex="15">
                      </asp:RadioButtonList><br />
                    </td>
                    <td style="text-align:center;">
                      <asp:ImageButton ID="ImageButton1" runat="server" 
                        ImageUrl="~/Images/AzureConfigWeb.png" Height="50%" Width="50%" />
                    </td>
                </tr>
                </table>
                <p style="font-size:.9em">
                This is a key setting for Windows Azure.  You will want to mark your Configuration Service and Primary Service endpoints
                as externally load-balanced on Azure. Simply supply the public DNS name below, for example 'stocktraderbsl.cloudapp.net:443'.  Azure 
                will automatically load balance requests against your running instances for scale-out.</p>
                </td>
           </tr>
           <tr>
                <td class="ConfigCSTDStyleLeft">
                    Externally Load-Balanced Virtual Address:
                </td>
                <td class="ConfigCSTDStyleRight" style="vertical-align:top">
                    <asp:TextBox ID="TextBoxLoadBalanceAddress" runat="server" Width="450px" 
                        CssClass="textEntry" TabIndex="16"></asp:TextBox>
                 <span style="font-size:.9em">Please include :PortNumber</span></td>
           </tr>
           <tr>
           <td class="ConfigCSTDStyleLeft">
                Online Method Name:</td>
           <td class="ConfigCSTDStyleRight">
               <asp:TextBox ID="TextBoxOnlineMethod" runat="server" Width="450px" 
                   CssClass="textEntry" TabIndex="17"></asp:TextBox></td>
       </tr>
        <tr>
           <td class="ConfigCSTDStyleLeft" style="vertical-align:bottom">
               Online Method Parameters:</td>
           <td class="ConfigCSTDStyleRight">
            <asp:CheckBox ID="CheckBoxOnlineParms" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBoxOnlineParms_CheckedChanged" /><br />
            <asp:TextBox ID="TextBoxOnlineParms" runat="server" Width="450px" 
                   CssClass="textEntry" TabIndex="18"></asp:TextBox></td>
       </tr>
           <tr>
                <td class="ConfigCSTDStyleLeft" style="vertical-align:top">
                    Auto Supply Connected Service User ID:</td>
                <td class="ConfigCSTDStyleRight">
                    <asp:CheckBox ID="CheckBoxAutoSupplyUser" runat="server" CssClass="textEntry" 
                        AutoPostBack="True" OnCheckedChanged="CheckBoxAutoSupplyUser_CheckedChanged" 
                        TabIndex="19" />
                    <p style="font-size:.9em">This setting determines whether clients that implement the Configuration Service will need to provide
                    username credentials when first establishing the Connected Service definition to service endpoints the Virtual Service Host implements. 
                    These credentials are used between client and host to exchange basic notifications, typically in an on-premise environment for load-balancing capabilties
                    provided by the Configuration Service.
                    </p> 
                </td>
           </tr>
           <tr>
                <td class="ConfigCSTDStyleLeft">
                    Connected Service Authentication User ID:
                </td>
               <td class="ConfigCSTDStyleRight">
                    <asp:DropDownList ID="DropDownListCsUser" runat="server" Width="300px" 
                        CssClass="textEntry" TabIndex="20">
                    </asp:DropDownList></td>
           </tr>
           <tr>
                <td class="ConfigCSTDStyleLeft" style="vertical-align:bottom">
                    <strong>
                    Activate </strong>Endpoint:<br />
                    </td>
               <td class="ConfigCSTDStyleRight">
                    <p style="font-size:.9em">This selection will activate/deactivate the endpoint
                    across all clustered nodes. For base addresses, all relative addresses
                    for the scheme will also be affected.
                    </p>
                    <asp:CheckBox ID="CheckBoxActivate" CssClass="textEntry" runat="server" 
                        TabIndex="21"/></td>
              </tr>
       
                                  
                                   <tr>
                                            <td colspan="5" style="text-align:center">
                                              <asp:Label ID="UpdateMessage" runat="server" Text=""></asp:Label>
                                                <asp:Label ID="UpdateMessageAzureLB" runat="server"></asp:Label>
                                            </td>
                                   </tr>
                                   <tr>
                                                <td colspan="2">
                                                <table align="center" >
                                                <tr>
                                                    <td style="padding-top:10px;padding-bottom:10px">
                                                    <asp:LinkButton  ID="Add"  Text="Add" runat="server" width="110px" Height="25px" 
                                                            CausesValidation="False" PostBackUrl="UpdateHostedService.aspx" 
                                                            onclick="Add_Click" CssClass="AddUpdateDeleteButton" TabIndex="22"></asp:LinkButton>
                                                   </td>
                                                   <td style="padding-top:10px;padding-bottom:10px">
                                                    <asp:LinkButton  ID="Update" Text="Update" runat="server" width="110px" 
                                                           Height="25px" CausesValidation="False" 
                                                           PostBackUrl="UpdateHostedService.aspx" onclick="Update_Click" 
                                                           CssClass="AddUpdateDeleteButton" TabIndex="23"></asp:LinkButton>
                                                   </td>
                                                   <td style="padding-top:10px;padding-bottom:10px">
                                                    <asp:LinkButton  ID="Delete" Text="Delete" runat="server" width="110px" 
                                                           Height="25px" CausesValidation="False" 
                                                           PostBackUrl="UpdateHostedService.aspx" onclick="Delete_Click" 
                                                           CssClass="AddUpdateDeleteButton" TabIndex="24"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                </table>
                            </td>
                            </tr>
                            <tr>
                            <td colspan="2">
                         <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List"
                            ValidationGroup="HS" ForeColor="Maroon" />
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorName" runat="server" ControlToValidate="TextBoxHostedServiceName"
                            Display="None" ErrorMessage="Please enter a service name" 
                            ValidationGroup="HS" ForeColor="Maroon"></asp:RequiredFieldValidator><asp:RequiredFieldValidator
                                ID="RequiredFieldValidatorPath" runat="server" ControlToValidate="TextBoxVirtualPath"
                                Display="None" ErrorMessage="Please enter a virtual path" 
                            ValidationGroup="HS" ForeColor="Maroon"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorPort" runat="server" ControlToValidate="TextBoxPort"
                            Display="None" ErrorMessage="Please enter a port" ValidationGroup="HS" 
                             ForeColor="Maroon"></asp:RequiredFieldValidator>
                       
                       </td>
              </tr>
              
            </table> <br />
           <div style="text-align:center">
        <asp:Label ID="ReturnLabel" runat="server" Text="returnlabel" 
        CssClass="ReturnLinkStyle" TabIndex="25"></asp:Label> </div>
    <br /><br /> </center>
</asp:Content>

