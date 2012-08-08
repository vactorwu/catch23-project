<%@ Page Title="Configuration Service: Connected Service Definitions" Buffer="true" Language="C#" EnableSessionState="false" MasterPageFile="~/Site.master"  MaintainScrollPositionOnPostback="true" AutoEventWireup="True" Inherits="ConfigService.ServiceConfiguration.Web.UpdateConnectedService" Codebehind="UpdateConnectedService.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   
    <h2>
        Manage Connected Service Definitions
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
   <!-- ****************************************** MAIN OUTER TABLE**************************************************** -->
   <table align="center" class="CSTableStyle">
   <tr>
      <td>
      <!-- ****************************************** INTRO TABLE **************************************************** -->
           <table align="center" class="CSTableStyleIntro" >
           <tr>
                <td colspan="2" style="text-align:left;padding-left:10px;font-size:1.1em;font-weight:normal;background-color:#000000;color:#FFFFFF;border-bottom:0px #d8d8d8 solid">
                    Instructions</td>
               </tr>
           <tr>
                <td colspan="2" style="padding-left:6px;border-bottom:2px #000000 solid">
                    <p style="text-align:left">
                    This page enables you to establish connections to remote services your application will utilize. A primary service implements the Configuration Service; a generic service 
                    does not. For example, a generic service might be one written in Java, PHP or .NET. When establishing connections to <b>primary services</b>, you will simply connect to the configuration service of the remote host, and a list of available services (contracts)
                    will be returned. You can also establish connection to <b>generic services</b> simply by providing the endpoint to the service, and filling out the requested details. A service implementing the Configuration Service can always
                    be treated as a generic service, since the Configuration Service imposes no requirement on the language clients are developed in, or whether they also implement the Configuration Service.
                    </p>
                </td>
            </tr>
            <tr>
                   <td colspan="2" style="text-align:left;padding-left:10px;font-size:1.1em;font-weight:normal;background-color:#000000;color:#FFFFFF;border-bottom:0px #d8d8d8 solid">
                   Connected Service Type
                  </td>
            </tr>
            <tr>
                  <td colspan="2" style="text-align:center; padding-top:20px;vertical-align:middle;color:#c1c1c1;border-bottom: 2px #000000 solid;">
                            <table align="center">
                            <tr>
                            <td style="text-align:left">
                                <asp:RadioButtonList ID="RadioButtonListServiceType" runat="server" 
                                    AutoPostBack="True" 
                                    OnSelectedIndexChanged="RadioButtonListServiceType_SelectedIndexChanged" 
                                    TabIndex="1">
                                </asp:RadioButtonList> 
                            </td>
                            </tr>
                            </table>
                       <br />                 
                  </td>
           </tr>
           </table>
     <!-- ****************************************** END INTRO TABLE**************************************************** -->
     <!-- ****************************************** PanelConfig Start **************************************************** -->            
     
                <asp:Panel ID="PanelConfig" runat="server">  

     <!-- ****************************************** START CONNECT SVCS TABLE**************************************************** -->
                <table align="center" class="CSTableStyleIntro">
                <tr>
                     <td colspan="2" style="text-align:left;padding-left:10px;font-size:1.1em;font-weight:normal;background-color:#000000;color:#FFFFFF;border-bottom:0px #d8d8d8 solid">
                    Remote Configuration Service Details - Get Connected!</td>
               </tr>
                <tr>
                           <td class="ConfigCSTDStyleLeft" style="vertical-align:bottom;">Address to Configuration Service:</td>
                           <td class="ConfigCSTDStyleRight">
                           <p style="font-size:.9em">
                           Once connected, the remote host will return the available services your client application can subscribe to. 
                           You can connect over http, https, or tcp to the remote Configuration Service to retrieve this list. Make sure 
                           to enter the address to the configuration service endpoint. If the host does 
                           not implement the configuration service, choose the Generic Service type above.
                           </p>
                           <asp:TextBox ID="TextBoxConfigLoginAddress" runat="server"  CssClass="textEntry" 
                                   Width="400px" TabIndex="2"></asp:TextBox>
                           
                           </td>
                 </tr>
                 <tr>
                           <td class="ConfigCSTDStyleLeft" style="vertical-align:bottom;">Client Configuration Name:<br /><br /><br /></td>
                           <td class="ConfigCSTDStyleRight">
                            <p style="font-size:.9em">
                               Please make sure to select a valid Configuration Service Client. Service configurations displayed
                               below are always prefixed with "client_configsvc" in the config file 'client' section
                               to appear in this list. You can use <a href="http://msdn2.microsoft.com/en-us/library/aa347733.aspx">svcutil.exe</a> to generate new client definitions
                               to remote configuration services if these services expose their metadata.<br />
                               <asp:DropDownList ID="DropDownListConfigClient" runat="server" 
                                   CssClass="textEntry"  AutoPostBack="True" 
                                   OnSelectedIndexChanged="DropDownListBinding_SelectedIndexChanged" 
                                    Width="400px" TabIndex="3"></asp:DropDownList>
                              
                             </p>
                                Selected Binding Type:
                                <span style="color:Navy"><asp:Label ID="BindingTypeAutoGenerate" runat="server"></asp:Label></span>
                           </td>
               </tr>
               <tr>
                           <td class="ConfigCSTDStyleLeft" style="border-bottom:0px;vertical-align:bottom">User Id:</td>
                           <td class="ConfigCSTDStyleRight" style="border-bottom:0px">
                           <asp:TextBox ID="TextBoxConfigLoginUserId" runat="server" CssClass="textEntry" 
                                   Width="100px" TabIndex="4"></asp:TextBox>  <span style="font-size:.9em;">&nbsp;&nbsp; (see information below left)</span>
                           </td>
              </tr>
              <tr>
                           <td class="ConfigCSTDStyleLeft">Password:</td>
                           <td class="ConfigCSTDStyleRight">
                             <asp:TextBox ID="TextBoxConfigLoginPassword" runat="server" CssClass="textEntry" 
                                   TextMode="Password" Width="100px" TabIndex="5"></asp:TextBox>
                           </td>
              </tr>
              <tr>
                            <td class="ConfigCSTDStyleLeft" style="border:0px;"><p style="font-size:.9em;text-align:justify;">
                                The credentials above are used by the Configuration Service to send/receive 
                                notifications between hosts and subscribed clients. Hosts can be configured to 
                                automatically return these credentials for any client attempting to subscribe, 
                                in which case nothing needs to be entered before connecting, the credentials 
                                will be returned. If the remote host is not configured to automatically provide 
                                these credentials, you will need to enter valid credentials to the remote host 
                                above, with the userid supplied having exactly &#39;Connected Service Rights&#39; as 
                                defined by the remote host in its users database. 
                           </p></td>
                            <td class="ConfigCSTDStyleRight" style="border:0px;"><div style="background-image:url('images/getsvcsbutton.png');background-repeat:no-repeat; "> 
                             <asp:LinkButton ID="AutoGenButton" runat="server" 
                                    PostBackUrl="UpdateConnectedService.aspx" Height="60px" 
                             Width="150px" onclick="AutoGenButton_Click" CssClass="GetSvcsButton" TabIndex="6">Get Remote<br /> Services!</asp:LinkButton> </div>
                            </td>
              </tr>
              <tr>
              <td colspan="2" style="text-align:center;"><span style="font-size:12px;">
                                            <asp:Label ID="LabelGetServices" runat="server"></asp:Label>
                                            </span></td>
              </tr>
              </table>
              </asp:Panel>
     <!-- ****************************************** END CONNECT SVCS TABLE**************************************************** -->
    
    
    
    
     <!-- *********************************************** END SVC PRIMARY TABLE ********************************************* -->
<!-- ******************************************** End Config Panel ********************************** -->
    
 <!-- ***************************************************************** Connected Primary Panel ********************************  -->
               <asp:Panel ID="ConnectedPanel" runat="server">
               <table align="center" class="CSTableStyleIntro">
               <tr>
                     <td colspan="2" style="padding-left:10px;text-align:left;font-size:1.1em;font-weight:normal;background-color:#000000;color:#FFFFFF;border-bottom:0px #d8d8d8 solid">
                     <asp:Label ID="LabelIntroPrimary" runat="server" Text="LabelPrimary" ForeColor="#FFFFFF"></asp:Label></td>
               </tr>
                            <tr>
                                    <td class="ConfigCSTDStyleLeft">Host Name Identifier:</td>
                                    <td class="ConfigCSTDStyleRight">
                                    <asp:Label ID="HostNameID" runat="server" Text="" ForeColor="Navy"></asp:Label>
                                    </td>
                            </tr>
                            <tr>
                            <td class="ConfigCSTDStyleLeft">
                                    Select From Available Remote Endpoints:
                            </td>
                            <td class="ConfigCSTDStyleRight">
                                        <asp:Label ID="LabelIntroService" runat="server" Text=""></asp:Label>
                                        
                                             <asp:DropDownList runat="server" ID="DropDownListServiceName" 
                                            Width="455px" BorderColor="#d4d8db" CssClass="textEntry" 
                                             AutoPostBack="True"  
                                            OnSelectedIndexChanged="DropDownListServiceName_SelectedIndexChanged" 
                                            TabIndex="7">
                                             </asp:DropDownList>
                                             
                                             <br /><br />
                                       

                                  <!--**************************** Panel panelConnected Service 1 ***************************************** -->
                                  <asp:Panel ID="panelConnectedService1" runat="server">
                                            <table class="ConfigDetailsTableStyle">
                                            <tr>
                                            <td colspan="2" style="text-align:center;color:#0E85A0;background-color:#2E383B;font-size:1.0em">
                                            <asp:Label ID="LabelServiceFriendlyName" runat="server"></asp:Label> 
                                            </td>
                                            </tr>
                                            <tr>
                                                    <td  style="text-align:right;padding-right:5px;border:#c1c1c1 1px solid;">
                                                    Service Contract:
                                                    </td>
                                                    <td style="text-align:left;padding-left:5px;border:#c1c1c1 1px solid;">
                                                    <asp:Label ID="LabelContract" runat="server"></asp:Label> 
                                                     </td>
                                             </tr>
                                             <tr>
                                                    <td style="text-align:right;padding-right:5px;border:#c1c1c1 1px solid;">
                                                    Service Binding Type:                        
                                                    </td>
                                                    <td style="text-align:left;padding-left:5px;border:#c1c1c1 1px solid;">
                                                    <asp:Label ID="LabelSvcBindingType" runat="server"></asp:Label> 
                                                     </td>
                                             </tr>
                                             <tr>
                                                    <td style="text-align:right;padding-right:5px;border:#c1c1c1 1px solid;">
                                                    Security Mode:                        
                                                    </td>
                                                    <td style="text-align:left;padding-left:5px;border:#c1c1c1 1px solid;">
                                                    <asp:Label ID="SecurityMode" runat="server"></asp:Label> 
                                                    </td>
                                             </tr>
                                             <tr>
                                                     <td style="text-align:right;padding-right:5px;border:#c1c1c1 1px solid;">
                                                     Service Virtual Path:                        
                                                     </td>
                                                     <td style="text-align:left;padding-left:5px;border:#c1c1c1 1px solid;">
                                                        <asp:Label ID="LabelVPath" runat="server"></asp:Label> 
                                                    </td>
                                            </tr>
                                            <tr>
                                                     <td style="text-align:right;padding-right:5px;border:#c1c1c1 1px solid;">
                                                     Service Port:                        
                                                    </td>
                                                    <td  style="text-align:left;padding-left:5px;border:#c1c1c1 1px solid;">
                                                     <asp:Label ID="LabelPort" runat="server"></asp:Label> 
                                                    </td>
                                            </tr>
                                             <tr>
                                                    <td style="text-align:right;padding-right:5px;border:#c1c1c1 1px solid;">
                                                    Uses Https:                        
                                                    </td>
                                                     <td style="text-align:left;padding-left:5px;border:#c1c1c1 1px solid;">
                                                    <asp:Label ID="LabelUseHttps" runat="server"></asp:Label> 
                                                    </td>
                                             </tr>
                                             <tr>
                                                    <td style="text-align:right;padding-right:5px;border:#c1c1c1 1px solid;">
                                                    Connected Configuration Service ID:                        
                                                    </td>
                                                     <td style="text-align:left;padding-left:5px;border:#c1c1c1 1px solid;">
                                                    <asp:Label ID="ConnectedConfigID" runat="server"></asp:Label> 
                                                    </td>
                                             </tr>
                                             <tr>
                                                    <td style="text-align:right;padding-right:5px;border:#c1c1c1 1px solid;">
                                                    Assigned CS Userid:                        
                                                    </td>
                                                     <td style="text-align:left;padding-left:5px;border:#c1c1c1 1px solid;">
                                                    <asp:Label ID="CSUserID" runat="server"></asp:Label> 
                                                    </td>
                                             </tr>
                                      </table>

                             <!-- ******************* Panel Connected 2 ****************************** -->
                              </asp:Panel>
                              <asp:Panel ID="panelConnectedService2" runat="server">
                                                    <table class="ConfigDetailsTableStyle">
                                                    <tr>
                                                        <td colspan="2" style="text-align:center;color:#0E85A0;background-color:#2E383B;font-size:1.0em">
                                                        <asp:Label ID="LabelServiceFriendlyName2" runat="server"></asp:Label> 
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align:right;padding-right:5px;border:#c1c1c1 1px solid;">
                                                            Service Contract:
                                                        </td>
                                                        <td style="text-align:left;padding-left:5px;border:#c1c1c1 1px solid;">
                                                            <asp:Label ID="LabelContract2" runat="server"></asp:Label> 
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                       <td style="text-align:right;padding-right:5px;border:#c1c1c1 1px solid;">
                                                           Service Binding Type:                        
                                                        </td>
                                                        <td style="text-align:left;padding-left:5px;border:#c1c1c1 1px solid;">
                                                        <asp:Label ID="LabelSvcBindingType2" runat="server"></asp:Label> 
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                    <td style="text-align:right;padding-right:5px;border:#c1c1c1 1px solid;">
                                                    Security Mode:                        
                                                    </td>
                                                    <td style="text-align:left;padding-left:5px;border:#c1c1c1 1px solid;">
                                                    <asp:Label ID="SecurityMode2" runat="server"></asp:Label> 
                                                    </td>
                                             </tr>
                                                    <tr>
                                                       <td style="text-align:right;padding-right:5px;border:#c1c1c1 1px solid;">
                                                           Client Binding Configuration:                        
                                                        </td>
                                                        <td style="text-align:left;padding-left:5px;border:#c1c1c1 1px solid;">
                                                        <asp:Label ID="LabelSvcBindingConfig2" runat="server"></asp:Label> 
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                    <td style="text-align:right;padding-right:5px;border:#c1c1c1 1px solid;">
                                                    Connected Configuration Service ID:                        
                                                    </td>
                                                     <td style="text-align:left;padding-left:5px;border:#c1c1c1 1px solid;">
                                                    <asp:Label ID="ConnectedConfigID2" runat="server"></asp:Label> 
                                                    </td>
                                             </tr>
                                             <tr>
                                                    <td style="text-align:right;padding-right:5px;border:#c1c1c1 1px solid;">
                                                    Assigned CS Userid:                        
                                                    </td>
                                                     <td style="text-align:left;padding-left:5px;border:#c1c1c1 1px solid;">
                                                    <asp:Label ID="CSUserID2" runat="server"></asp:Label> 
                                                    </td>
                                             </tr>
                                                    </table>
                                   </asp:Panel>
                                                    <!-- ***************************** End Connected2 Panel ********************* -->
                                                    </td>
                                                  </tr>

                                                           <tr>
                                                        <td class="ConfigCSTDStyleLeft" style="vertical-align:bottom;">
                                                            Select Client Contract to Use:</td>
                                                        <td class="ConfigCSTDStyleRight">
                                                        <p style="font-size:.9em;text-align:left;">
                                                            You must select from the client contracts available based on those you supplied
                                                            in your initialization logic.</p>
                                                            <asp:DropDownList ID="DropDownListPrimaryContract" runat="server" 
                                                                CssClass="textEntry" Width="455px"  AutoPostBack="True" 
                                                                OnSelectedIndexChanged="DropDownListPrimaryContract_SelectedIndexChanged" 
                                                                TabIndex="8">
                                                            </asp:DropDownList>
                                                            </td>
                                                   </tr>
                                                          <tr>
                                                        <td class="ConfigCSTDStyleLeft" style="vertical-align:bottom;">
                                                            Select Client Configuration to Use:<br /><br /></td>
                                                        <td class="ConfigCSTDStyleRight">  <p style="font-size:.9em">This list has been automatically trimmed to client configurations found in config that  1) Match the service contract selected above; 2) Use a compatible binding type and security mode; and 3) match the Configuration Service naming pattern. 
                                                        Select the appropriate available client configuration to use. For example,
                                                            as generated by <a href="http://msdn2.microsoft.com/en-us/library/aa347733.aspx">svcutil.exe</a>
                                                            via WS Metadata Exchange (mex). If you do not see the configuration
                                                            you want, you need to either add it to your web.config or .exe.config file, or correct issues stemming from the matching pattern.  The matching pattern prevents many <strong>runtime</strong> exceptions before they happen.
                                                            </p>
                                                            <asp:DropDownList ID="DropDownListPrimaryClients" runat="server"  Width="455px" 
                                                                BorderColor="#d4d8db"  CssClass="textEntry"
                                                                AutoPostBack="True" 
                                                                OnSelectedIndexChanged="DropDownListPrimaryBindings_SelectedIndexChanged" 
                                                                TabIndex="9"></asp:DropDownList>
                                                                <br />

                                                               
                                                            Selected Binding Type:
                                                            <span style="color:Navy"><asp:Label ID="LabelBindingTypePrimary" runat="server"></asp:Label></span></td> 
                                                         </tr>
                                                         <tr>
                                                        <td class="ConfigCSTDStyleLeft">
                                                            Online Check Method:</td>
                                                        <td class="ConfigCSTDStyleRight">
                                                        <asp:TextBox ID="TextBoxOnlineMethod" runat="server"  CssClass="textEntry" 
                                                                Width="450px" TabIndex="10"></asp:TextBox>
                                                            &nbsp;</td>
                                                   </tr>
                                                   <tr>
                                                        <td class="ConfigCSTDStyleLeft">
                                                            Online Check Parameters (no entry equates to a null parameter array):</td>
                                                        <td class="ConfigCSTDStyleRight">
                                                            <asp:TextBox ID="TextBoxOnlineParms" runat="server"  CssClass="textEntry" 
                                                                Width="450px" TabIndex="11"></asp:TextBox>
                                                            &nbsp;</td>
                                                   </tr>
                                                   <tr>
                                                        <td class="ConfigCSTDStyleLeft" style="vertical-align:top;">
                                                            Apply Default UserName Client Credentials:</td>
                                                        <td class="ConfigCSTDStyleRight"><asp:CheckBox ID="CheckBoxUserCredentials" 
                                                                runat="server"  AutoPostBack="True" OnCheckedChanged="CheckBoxUserCredentials_CheckedChanged" CssClass="textEntry" /> 
                                                               <p style="font-size:.9em">
                                                                Yes, use a specific user
                                                        ID defined in this configuration database to supply per request to this service.  See documentation, you can also
                                                        supply different credentials dynamically at runtime if desired based on other authentication sources.</p></td>
                
                                                   </tr>
                                                   <tr>
                                                        <td class="ConfigCSTDStyleLeft" style="vertical-align:top;border-bottom:0px;">
                                                            <span style="font-size:.9em">Choose User for Default Client UserName Credentials (if checked above). Only users with 'Service Operation Rights' will appear in this list.</span>
                                                        </td>
                                                        <td class="ConfigCSTDStyleRight" style="border-bottom:0px;">
                                                            <asp:DropDownList ID="DropDownListCsUser" runat="server" Width="300px" 
                                                                CssClass="textEntry" TabIndex="12">
                                                            </asp:DropDownList>
                                                        </td>
                                                 </tr>


                                                <tr>
                                                <td colspan="2">
                                                <table align="center" >
                                                <tr>
                                                    <td style="padding-top:10px;padding-bottom:10px">
                                                    <asp:LinkButton  ID="Add"  Text="Add" runat="server" width="110px" Height="25px" 
                                                            CausesValidation="False" PostBackUrl="UpdateConnectedService.aspx" 
                                                            onclick="Add_Click" CssClass="AddUpdateDeleteButton" TabIndex="13"></asp:LinkButton>
                                                   </td>
                                                   <td style="padding-top:10px;padding-bottom:10px">
                                                    <asp:LinkButton  ID="Update" Text="Update" runat="server" width="110px" 
                                                           Height="25px" CausesValidation="False" 
                                                           PostBackUrl="UpdateConnectedService.aspx" onclick="Update_Click" 
                                                           CssClass="AddUpdateDeleteButton" TabIndex="14"></asp:LinkButton>
                                                   </td>
                                                   <td style="padding-top:10px;padding-bottom:10px">
                                                    <asp:LinkButton  ID="Delete" Text="Delete" runat="server" width="110px" 
                                                           Height="25px" CausesValidation="False" 
                                                           PostBackUrl="UpdateConnectedService.aspx" onclick="Delete_Click" 
                                                           CssClass="AddUpdateDeleteButton" TabIndex="15"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                </table>
                            </td>
                            </tr>
                                <tr>
                                <td colspan="2" style="text-align:center;">
                                <asp:Label ID="UpdateMessage" runat="server" Text="">
                                    </asp:Label> 
                                </td>
                                </tr>
                        </table>
        
<!-- ***********************************************End Connected Primary Panel *************** -->
         </asp:Panel>



          <!-- ******************************************* Generic Panel **************************************** -->
               <asp:Panel ID="GenericPanel" runat="server">
                               
               <table align="center" class="CSTableStyleIntro">
               <tr>
                    <td colspan="2" style="text-align:left;padding-left:10px;font-size:1.1em;font-weight:normal;background-color:#000000;color:#FFFFFF;border-bottom:0px #d8d8d8 solid">
                    Generic Connected Service Details</td>
               </tr>                
               <tr>
                            <td class="ConfigCSTDStyleLeft" style="vertical-align:bottom;">Host Name Identifier:</td>
                            <td class="ConfigCSTDStyleRight" >
                                <p>This will be a <strong>Generic Connected Service.</strong>
                                Assign any logical Host Name Identifier. You can then establish multiple connections
                                via the Connections Page to this logical host and load balance requests across these
                                connections, even if this host is Java-based or a .NET service not implementing the Configuration Service.
                                </p>
                                               
                            <asp:TextBox ID="TextBoxHostNameGeneric" runat="server" Width="300px" 
                                    CssClass="textEntry" MaxLength="200" TabIndex="16"></asp:TextBox>
               
               
                            </td>
                    </tr>
                    <tr>
                            <td class="ConfigCSTDStyleLeft" style="vertical-align:bottom;">Assigned Service Name:</td>
                            <td class="ConfigCSTDStyleRight">
                            <p style="font-size:.9em">Assign any Service Name. Use this name to invoke the service using the load balancing/failover client.</p>
                            <asp:TextBox ID="TextBoxGenericName" runat="server" Width="300px" 
                                    CssClass="textEntry" MaxLength="200" TabIndex="17"></asp:TextBox>
                           </td>
                    </tr>
                    <tr>
                            <td class="ConfigCSTDStyleLeft">
                                Service Contract:</td>
                            <td class="ConfigCSTDStyleRight">
                                <asp:DropDownList ID="DropDownListGenericContract" runat="server" CssClass="textEntry"
                                    Width="300px" AutoPostBack="True" 
                                    OnSelectedIndexChanged="DropDownListGenericContract_SelectedIndexChanged" 
                                    TabIndex="18">
                                </asp:DropDownList>
                                                
                                </td>
                        </tr>
                        <tr>
                            <td class="ConfigCSTDStyleLeft"  style="vertical-align:bottom;">Assign a User Id:</td>
                            <td class="ConfigCSTDStyleRight">
                                <p style="font-size:.9em">Note: Even if the generic service does not require authentication, you must create a default Connected Service User for this service, as this is used
                                internally to track and manage the Connected Service definition to this virtual host. Enter any credentials here, and they will be created automatically.
                                </p>
                                <asp:TextBox ID="TextBoxGenericUser" runat="server" CssClass="textEntry" 
                                    Width="100px" MaxLength="50" TabIndex="19"></asp:TextBox></td>
                    </tr>
                    <tr>
                            <td class="ConfigCSTDStyleLeft">
                                Assign a Password:</td>
                            <td class="ConfigCSTDStyleRight">
                                <asp:TextBox ID="TextBoxGenericPassword" 
                                    runat="server" CssClass="textEntry" TextMode="Password" Width="100px" 
                                    MaxLength="50" TabIndex="20"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="ConfigCSTDStyleLeft">
                            UseHttps (Do client connections require https?):</td>
                        <td class="ConfigCSTDStyleRight"> 
                            <asp:CheckBox ID="CheckBoxGenericUseHttps" runat="server" 
                                CssClass="textEntry" /></td>
                
                    </tr>
                    <tr>
                        <td class="ConfigCSTDStyleLeft"  style="vertical-align:bottom;">
                            Select Client Configuration to Use:
                            <br /><br /></td>
                        <td class="ConfigCSTDStyleRight"> <p style="font-size:.9em">
                            Select the appropriate available client configuration to use. For example,
                            as generated by <a href="http://msdn2.microsoft.com/en-us/library/aa347733.aspx">svcutil.exe</a>
                            via WS Metadata Exchange (mex). The configurations listed below are from the <strong>
                            client's</strong> configuration file, so if you do not see the configuration
                            you want, you need to add it to your web.config or .exe.config file for the client
                            application/service first before it will appear here.&nbsp; The configuration name
                            must be pre-fixed with 'client_' in your config file, and match the selected service
                            contract.</p>
                            <asp:DropDownList ID="DropDownListGenericClients" runat="server"  Width="300px" 
                               CssClass="textEntry" AutoPostBack="True" 
                                OnSelectedIndexChanged="DropDownListBindingGeneric_SelectedIndexChanged" 
                                TabIndex="21">
                            </asp:DropDownList>
                            <br />
                            Selected Binding Type:
                            <span style="color:Navy"><asp:Label ID="LabelGenericBindingType" runat="server" ></asp:Label></span></td> 
                    </tr>
                    <tr>
                            <td class="ConfigCSTDStyleLeft">
                                Online Check Method:</td>
                            <td class="ConfigCSTDStyleRight">
                            <asp:TextBox ID="TextBoxGenericOnlineMethod" runat="server" CssClass="textEntry" 
                                    Width="100px" MaxLength="100" TabIndex="22"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="ConfigCSTDStyleLeft">
                                Online Check Parameters (no entry equates to a null parameter array):</td>
                            <td class="ConfigCSTDStyleRight">
                                <asp:TextBox ID="TextBoxGenericParms" runat="server" CssClass="textEntry"
                                    Width="300px" MaxLength="200" TabIndex="23"></asp:TextBox>
                                </td>
                        </tr>
                      <tr>
                        <td class="ConfigCSTDStyleLeft" style="vertical-align:top" >
                            Default Address:
                           </td>
                        <td class="ConfigCSTDStyleRight">
                
                            <asp:TextBox ID="TextBoxGenericAddress" runat="server" Width="300px" 
                                CssClass="textEntry" MaxLength="250" TabIndex="24"></asp:TextBox>  
                            <p style="font-size:.9em">This address is used as a hint in the Add Connections Page. For example, perhaps 'http://[enterserver]:8080/myservice_ep'.
                            This URI template will then be provided in that page.</p>
                            </td>
                    </tr>
                        <tr>
                            <td class="ConfigCSTDStyleLeft" style="vertical-align:top">
                                Apply Default Client UserName Credentials:</td>
                            <td class="ConfigCSTDStyleRight">
                            <asp:CheckBox ID="CheckBoxGenericCredentials" CssClass="textEntry" runat="server"  
                                    AutoPostBack="True" 
                                    OnCheckedChanged="CheckBoxGenericCredentials_CheckedChanged" TabIndex="25" /> 
                            <p style="font-size:.9em">Yes, use a specific user ID defined in this configuration database to supply per request to this service.  
                            See documentation, you can also supply different credentials dynamically at runtime if desired based on other
                            authentication sources.</p>
                            </td>
                
                        </tr>
                        <tr>
                            <td class="ConfigCSTDStyleLeft" style="vertical-align:top">
                                Choose User for Default Client UserName Credentials (if checked above):
                            </td>
                            <td class="ConfigCSTDStyleRight">
                                <asp:DropDownList ID="DropDownListGenericCredentialUsers" runat="server" 
                                    Width="200px" CssClass="textEntry" TabIndex="26">
                                </asp:DropDownList><br />
                               <p style="font-size:.9em"> Only users with 'Service Operation Rights' will appear in this list.
                               </p>
                               </td>
                        </tr>
                               <tr>
                                <td colspan="2">
                <table align="center" >
                <tr>
                    <td style="padding-top:10px;padding-bottom:10px">
                    <asp:LinkButton  ID="ButtonAddGeneric"  Text="Add" runat="server" width="110px" 
                            Height="25px" CausesValidation="False" 
                            PostBackUrl="UpdateConnectedService.aspx" onclick="AddGeneric_Click" 
                            CssClass="AddUpdateDeleteButton" TabIndex="27"></asp:LinkButton>
                   </td>
                   <td style="padding-top:10px;padding-bottom:10px">
                    <asp:LinkButton  ID="ButtonUpdateGeneric" Text="Update" runat="server" 
                           width="110px" Height="25px" CausesValidation="False" 
                           PostBackUrl="UpdateConnectedService.aspx" onclick="UpdateGeneric_Click" 
                           CssClass="AddUpdateDeleteButton" TabIndex="28"></asp:LinkButton>
                   </td>
                   <td style="padding-top:10px;padding-bottom:10px">
                    <asp:LinkButton  ID="ButtonDeleteGeneric" Text="Delete" runat="server" 
                           width="110px" Height="25px" CausesValidation="False" 
                           PostBackUrl="UpdateConnectedService.aspx" onclick="DeleteGeneric_Click" 
                           CssClass="AddUpdateDeleteButton" TabIndex="29"></asp:LinkButton>
                    </td>
                </tr>
                </table>
                </td>
                </tr>
                <tr>
                     <td colspan="2">
                      <asp:Label ID="GenericUpdateMessage" runat="server" Text=""></asp:Label>
                     </td>
                </tr>
        </table>
       </asp:Panel>
  <!-- ******************************************* END Generic Panel **************************************** -->



<!-- ********************************** END MAIN TABLE ************************************** -->
         </td>
         </tr>
         </table>      

    <br />   
    <asp:Label ID="ReturnLabel" runat="server" Text="returnlabel" 
        CssClass="ReturnLinkStyle" TabIndex="30"></asp:Label>
    <br /><br />
    </center>
</asp:Content>

