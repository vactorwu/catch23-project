<%@ Page Title="Configuration Service: Welcome" Buffer="true" Language="C#" EnableSessionState="false" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="ConfigService.ServiceConfiguration.Web._Default" Codebehind="Default.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   
    <h2>
        Welcome to ConfigWeb!
    </h2>
  
        <table>
        <tr>
        <td width="620">
        <p>
            <a href="Login.aspx">Login</a> to explore the <span style="font-family:Segoe UI; font-style:oblique; font-variant:small-caps;font-weight:bold">StockTrader 5</span> composite application as it is currently deployed
            both on <a href="http://www.microsoft.com/windowsazure/windowsazure/">Windows Azure</a> and a <a href="http://www.microsoft.com/systemcenter/en/us/virtual-machine-manager.aspx">Windows Server 2008 Hyper-V</a> virtualized private cloud.</p>
        <p>
            You are visiting the home page for ConfigWeb, which is a configuration management user interface for the StockTrader 5 composite application.  
            It is designed to provide an application-centric view of a deployed service domain, including many 
            different integrated elements of its runtime environment and the current status for each element. ConfigWeb works in
            conjunction with the <a href="develop.aspx">Configuration Service 5.0</a>, and both are part of the downloadable StockTrader 5 sample 
            application kit on MSDN with all source code provided. The Configuration Service can be easily
            implemented in any custom application or WCF service, with a Visual Studio 2010 template included.  While not required for 
            building applications and services on Windows Azure, the Configuration Service as sample code shows some of the ways to leverage the 
            Windows Azure SDK and API; and to build a scale-out service that runs both on-premise, in the cloud, or in a hybrid scenario using WCF. 
            As such, the sample illustrates the flexibility of WCF for a wide-variety of connecitivity and security scenarios.
            Click <a href="Develop.aspx">here</a> for a brief Configuration Service overview and implementation 
            information. Also make sure to visit <a href="https://azurestocktrader.cloudapp.net/">Azure StockTrader 5</a> as an end-user to see how 
            the secure services you explore here from a configuration perspective tie together into a complete composite application deployed to the cloud.</p>

        <p>
            For more detailed information about Configuration Service,
            refer to the <a href="documentation/ConfigServiceTechnicalGuide.pdf" target="_blank">Configuration Service 5.0 Technical Guide</a>. This is an in-depth sample application, so  <a href="http://msdn.microsoft.com/stocktrader">check back</a> on MSDN for periodic
            updates to this sample based on community feedback, bug fixes, and enhancements.
        </p>
        </td>       
        <td style="padding-left:20px;vertical-align:middle;padding-top:10px"">
            
            <table class="technologies" align="center" width="500" style="background-image:url('images/techfill.png')">
            <tr>
                <th style="text-align:center; vertical-align:middle;font-size:1.2em;font-weight:normal;background-color:#000000; color:white; height: 25px;">
                Topics Illustrated</th>
            </tr>
            <tr >
            <td style="padding-right:8px;line-height:1.5em">   
            <ul>
            <li class="Mainline">
                Integrating Windows Azure and on-premise applications with WCF
                </li>
                 <li class="Mainline">
                Building high performance SQL Azure and SQL Server applications
                </li>
                 <li class="Mainline">
                Using Windows Azure and Windows Server AppFabric Caching
                </li>
                <li class="Mainline">
                Working with both ASP.NET applications and WCF Services in the cloud
                </li>
                 <li class="Mainline">
                Creating, testing and managing secure WCF Services
                </li>
                <li class="Subline">
                Scale-out scenarios both on Windows Azure and on-premise</li>
                <li class="Mainline">
                Handling configuration updates across many live nodes
                </li>
                <li class="Subline">
                Monitoring applications and services deployed on-premise or the cloud</li>
                <li class="Subline">
                Viewing exception logs online via a SQL Azure logging database
                </li>
                <li class="Subline">
                Capturing and displaying live performance metrics for a service domain 
                </li>
                <li class="Subline">
                Can be a learning sample, implemented as-is, or extended
                </li>
                
            </ul>
            </td>
         </tr>
         </table>
         <br />
        </td>
        </tr>
        </table>
       <center><asp:Image ID="TradeImage" runat="server" ImageUrl="~/Images/globe.png" /><br /></center>
   <br />
</asp:Content>

