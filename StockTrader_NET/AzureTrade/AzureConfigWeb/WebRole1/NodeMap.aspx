<%@ Page Title="Configuration Service: Node Map" Buffer="true" Language="C#" MasterPageFile="~/Site.master" EnableSessionState="false" AutoEventWireup="True" Inherits="ConfigService.ServiceConfiguration.Web.NodeMap" Codebehind="NodeMap.aspx.cs" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Node Map
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
                <span style="font-size:1.2em">Nodes</span>
               <div style="font-size:.8em;padding-left:35px;padding-bottom:10px;text-align:left;padding-right:35px"> This page shows the actual running nodes
               behind any external load balancer.  Use the Connections Page to see the actual view the client service domain sees.  On that page, if externally load-balanced, you will see just one 
               connection as seen by the client to that load balanced endpoint.  The Connections page shows you if the client service cannot reach
               the remote service, for any reason (host is down, network connectivity, mismatched service bindings, etc.).  While a node might be online in this view, the Connections page shows if it is actually reachable from the service client over the network, 
               whether an internal network or the Internet.  Both views provide a more complete picture.  If a node is down (red) below, hover over it to see the exception information.  A grey node indicates a timeout.  Note that measured request counts are persisted every three minutes.
               Becuase of this, and becuase node instances can change, the total requests for a domain is not the sum of requests for currently active nodes.
               </div> 
               <div style="text-align:center;font-size:1.0em;color:#000000"><asp:Label ID="UTC" runat="server" Text="Label"></asp:Label></div>
              <table class="ConnectionPointTableStyle" style="background-image: url('Images/connpointfill.png');text-align:center;table-layout:fixed;" align="center" width="1100">
              <col width="200" />
              <col width="80" />
              <col width="80" />
              <col width="145" />
              <col width="145" />
              <col width="450" />
             <tr>
                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Host Name</th>
                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Status</th>
                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Active Since</th>
                 <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">
                     <asp:Label ID="LabelReq" runat="server" Text="Label"></asp:Label><asp:Label ID="VNODETotalReqs" runat="server" Text="Label"></asp:Label></th>
                  <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid"><asp:Label ID="LabelReqDay" runat="server" Text="Label"></asp:Label><asp:Label ID="VNODETotalReqsPerDay" runat="server" Text="Label"></asp:Label></th>
                  <th class="TradeConfigTHStyle2" style="border:1px #c1c1c1 solid">Primary Business Service Endpoints</th>
             </tr>
            
          <asp:Repeater id="NodeRepeater" runat="server">
          <HeaderTemplate>
          </HeaderTemplate>
          <ItemTemplate>
            <tr>
                <td class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                   <asp:Label runat="server" ID="Address"></asp:Label></td>
                 <td class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                    <asp:Label runat="server" ID="Status"></asp:Label></td>
                 <td class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                    <asp:Label runat="server" ID="ActiveSince"></asp:Label></td>
                     <td class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                    <asp:Label runat="server" ID="LabelTotalRequests"></asp:Label></td>
                     <td class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                    <asp:Label runat="server" ID="LabelRequestsPerDay"></asp:Label></td>
               
                   <td class="TradeConfigTDSettingStyle1a" style="text-align:center;">
                                <asp:Repeater id="EndPointRepeater" runat="server">
                                <HeaderTemplate>    
                                <table align="center">
                                </HeaderTemplate>
                                <ItemTemplate>
                                <tr><td style="text-align:center">
                                <asp:Label runat="server" ID="Address"></asp:Label>
                                </td>
                                </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                </table>
                                </FooterTemplate>
                                </asp:Repeater>
                            </td>

                
                
           </tr>      
         </ItemTemplate>
         <FooterTemplate>
       </FooterTemplate>
       </asp:Repeater>
       </table>
        <br />
                <asp:Label ID="ReturnLabel" runat="server" TabIndex="1" ></asp:Label>
                <br />
                <br />
       </center>
</asp:Content>

