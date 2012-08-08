<%@ Page Title="Configuration Service: Update Virtual Host" Buffer="true" Language="C#" EnableSessionState="false" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="ConfigService.ServiceConfiguration.Web.VHostUpdate" Codebehind="VHostUpdate.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   
    <h2>
        Update Virtual Hosts
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
                        <td class="ConfigCSTDStyleRight" colspan="2" style="padding-left:10px;padding-right:10px">
                       <p>You may create as many <strong>Primary</strong> Virtual Hosts as you like, these host your custom business services, for which you supply the implementation logic.  The Configuration Service will manage their lifecycle automatically.  Every service
                       implementing Configuration Service will always have one <strong>Configuration</strong> Service host, and one <strong>Node</strong> Service host, which are created automatically. Configuration Service endpoints
                       are external, and should be secured with transport security.  All service operations are authenticated.  The Node Service will
                       typically be configured with private endpoints, such as internal endpoints on Windows Azure.  The Node Service automatically keeps
                       configurations synchronized between running nodes. You are free to modify WCF properties (behaviors, endpoints, bindings) for Configuration Service and Node hosts.
                       </p>
                       
                       </td>
                    </tr>
                     <tr>
                        <td colspan="2" style="text-align:left;padding-left:10px;font-size:1.1em;font-weight:normal;background-color:#000000;color:#FFFFFF;border-bottom:0px #d8d8d8 solid">
                        Enter Virtual Host Details</td>
                    </tr>

                    <tr>
                            <td class="ConfigCSTDStyleLeft">
                            Service Type:
                            </td>
                           <td class="ConfigCSTDStyleRight">
                            <table>
                            <tr>
                                 <td style ="text-align:left;vertical-align:middle">
                                    <br />
                                <asp:RadioButtonList ID="RadioButtonListServiceType" runat="server" 
                                         CssClass="textEntry" Enabled="False" TabIndex="1">
                                </asp:RadioButtonList>
                                </td>
                            </tr>
                            </table>
                            </td>
                </tr>
                <tr>
                            <td class="ConfigCSTDStyleLeft">
                            Service Implementation Class:</td>
                            <td class="ConfigCSTDStyleRight">
                            <asp:DropDownList ID="DropDownListClass" Width="450px" CssClass="textEntry"  
                                    runat="server" TabIndex="2">
                            </asp:DropDownList>
                            <br />
                                <asp:Label ID="ServiceClassNameLabel" runat="server" ForeColor="DarkRed"></asp:Label>
                            </td>
                </tr>
                <tr>
                            <td class="ConfigCSTDStyleLeft" style="vertical-align:bottom">
                            Service Behavior Configuration:</td>
                            <td class="ConfigCSTDStyleRight">
                            <asp:DropDownList ID="DropDownListConfig" runat="server" CssClass="textEntry"  
                                    Width="455px" TabIndex="3"></asp:DropDownList>
                            </td>
                </tr>
                <tr>
                            <td class="ConfigCSTDStyleLeft">
                            Is WorkFlowServiceHost:
                            </td>
                          <td class="ConfigCSTDStyleRight">
                            <asp:CheckBox ID="CheckBoxWorkFlow" runat="server" 
                                  Text="Yes, this is a WorkflowServiceHost" TabIndex="4" />
                            </td>
                </tr>
                <tr>
                          <td class="ConfigCSTDStyleLeft">
                            Service Host Environment:
                           </td>
                           <td class="ConfigCSTDStyleRight">
                            <table>
                                    <tr>
                                         <td style ="text-align:left;vertical-align:middle">
                                         <br />
                                        <asp:RadioButtonList ID="RadioButtonListHostEnvironment" runat="server" 
                                                 CssClass="textEntry" TabIndex="5" >
                                        </asp:RadioButtonList>
                                        </td>
                                    </tr>
                            </table>
                            </td>
                </tr>
                <tr>
                         <td class="ConfigCSTDStyleLeft" style="vertical-align:bottom">
                         Azure Http Role Endpoint Name or IIS Http Host Header Name:</td>
                      <td class="ConfigCSTDStyleRight">
                            
                            <asp:CheckBox ID="CheckBoxHttpHeader" runat="server" AutoPostBack="True" 
                                CssClass="textEntry" OnCheckedChanged="CheckBoxHttpHeader_CheckedChanged" 
                                TabIndex="6" />
                            <asp:ImageButton ID="ImageButton1" runat="server" 
                                ImageUrl="~/Images/AzureConfigWeb.png" Width="10%" Height="10%" />
                          <p style="font-size:.9em">  This is a key setting for Windows Azure.  You will need to supply the name of your Azure endpoint for this virtual service host (input or internal) here.  Windows Azure endpoint names are
                            assigned in Visual Studio (or the service definition file) for your Azure project.</p>
                  
                        
                        <asp:TextBox ID="TextBoxHttpHeader" runat="server" Width="450px" 
                                CssClass="textEntry" TabIndex="7" ></asp:TextBox>
                         </td>
                </tr>
                 <tr>
                       <td class="ConfigCSTDStyleLeft">
                        Azure Tcp Role Endpoint Name or 
                        IIS Tcp Host Header Name:
                        </td>
                        <td class="ConfigCSTDStyleRight">
                         <asp:CheckBox ID="CheckBoxTcpHeader" runat="server" CssClass="textEntry" 
                                AutoPostBack="True" OnCheckedChanged="CheckBoxTcpHeader_CheckedChanged" 
                                TabIndex="8" />
                            <!-- OnCheckedChanged="CheckBoxTcpHeader_CheckedChanged"-->
                            <asp:ImageButton ID="ImageButton2" runat="server" 
                                ImageUrl="~/Images/AzureConfigWeb.png" Width="10%" Height="10%" />
                        <br />
                        <asp:TextBox ID="TextBoxTcpHeader" runat="server" Width="450px" 
                                CssClass="textEntry" TabIndex="9" ></asp:TextBox>
                        </td>
                </tr>
                <tr>
                <td colspan="2">
                
                <table align="center" >
                                                <tr>
                                                    <td style="padding-top:10px;padding-bottom:10px">
                                                    <asp:LinkButton  ID="Add"  Text="Add" runat="server" width="110px" Height="25px" 
                                                            CausesValidation="False" PostBackUrl="VHostUpdate.aspx" 
                                                            onclick="Add_Click" CssClass="AddUpdateDeleteButton" TabIndex="10"></asp:LinkButton>
                                                   </td>
                                                   <td style="padding-top:10px;padding-bottom:10px">
                                                    <asp:LinkButton  ID="Update" Text="Update" runat="server" width="110px" 
                                                           Height="25px" CausesValidation="False" 
                                                           PostBackUrl="VHostUpdate.aspx" onclick="Update_Click" 
                                                           CssClass="AddUpdateDeleteButton" TabIndex="11"></asp:LinkButton>
                                                   </td>
                                                   <td style="padding-top:10px;padding-bottom:10px">
                                                    <asp:LinkButton  ID="Delete" Text="Delete" runat="server" width="110px" 
                                                           Height="25px" CausesValidation="False" 
                                                           PostBackUrl="VHostUpdate.aspx" onclick="Delete_Click" 
                                                           CssClass="AddUpdateDeleteButton" TabIndex="12"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                 
                                   </table>
                </td>
                </tr>
                 <tr>
                                            <td colspan="5" style="text-align:center">
                                              <asp:Label ID="UpdateMessage" runat="server" Text=""></asp:Label></td>
                                   </tr>
                 <tr>
                                   <td colspan="2">
                                   
                                   
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                            ValidationGroup="CS" DisplayMode="List" ForeColor="Maroon" />
                                    <asp:RequiredFieldValidator ID="Validator" runat="server" 
                            ControlToValidate="DropDownListClass" 
                            ErrorMessage="You Must Supply A Service Implementation Class.  If no classes are selectable, you must supply them in your config service startup logic." 
                            ForeColor="DarkRed" ValidationGroup="CS" Visible="True" Display="None"></asp:RequiredFieldValidator>
                                  
                                   <br />
                                   </td>
                                   </tr>
              </table>
              <br />   
    <asp:Label ID="ReturnLabel" runat="server" Text="returnlabel" 
        CssClass="ReturnLinkStyle" TabIndex="13"></asp:Label>
    <br /><br /> </center>
</asp:Content>

