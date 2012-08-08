<%@ Page Title="Configuration Service: Welcome" Buffer="true" Language="C#" MasterPageFile="~/Site.master" EnableSessionState="false" AutoEventWireup="true" Inherits="ConfigService.ServiceConfiguration.Web.Develop" Codebehind="Develop.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   
    <h2>
        Implementing Configuration Service 5
    </h2>
        <p style="font-size:.95em;">
            The Configuration Service can be implemented in your own ASP.NET applications 
            and Windows Communication Foundation Services-- whether you are intending to 
            deploy on-premise or on Windows Azure. A new Visual Studio 2010 template and 
            corresponding wizard allows you to do so without writing any code. The template 
            provides a structured solution that, in addition to providing a complete 
            Configuration Service 5.0 implementation, also provides a sample service with a 
            data access layer (DAL) based on the high-performance StockTrader 5.0 design 
            pattern, and a Web site for excersing the functionality of the service 
            operations and SQL data access. The sample service within the template is not 
            the actual StockTrader 5.0 application: while using the same design pattern for 
            logical partitioning and data access, it is a basic service that is meant to 
            serve as a starting place for developers to replace business logic and data 
            access logic with their own logic for their own high-performance custom business 
            applications. The complete StockTrader 5.0 sample application code is also 
            provided separately from the template, and is a good starting place for 
            exploring a complete end-to-end application (with scenario-specific logic). For 
            more detailed information about Configuration Service, refer to:</p>
            <ul style="font-size:.95em">
            <li> <a href="documentation/ConfigServiceVSTemplate.pdf" target="_blank">Using the Visual Studio Template/Implementing Configuration Service 5.0 and the StockTrader 5.0 Design Pattern</a> </li>
            <li>  <a href="documentation/stocktraderconfiguration.pdf" target="_blank">Tutorial: Changing the StockTrader 5.0 Configuration and Using ConfigWeb</a></li>
            <li> <a href="documentation/ConfigServiceTechnicalGuide.pdf" target="_blank">Configuration Service 5.0 Technical Guide</a>. </li>
            <li> <a href="documentation/GuideToSolutions.pdf" target="_blank">Guide to Visual Studio 2010 Solutions</a>. </li>

            </ul>
            <p style="font-size:.95em;">
            The Configuration Service 5.0 template supports all .NET host environments, including:
            </p>

            <ul style="font-size:.95em;">
            <li>Azure Web Role for ASP.NET/IIS cloud hosting.</li>
            <li>Azure Worker Role for .NET cloud hosting.</li>
            <li>Azure VM Role for .NET cloud hosting*.</li>
            <li>On-premise ASP.NET IIS Web Site (IIS 7.x or IIS 6.x).</li>
            <li>On-premise .NET Windows NT Service.</li>
            <li>On-premise .NET Windows Console Application.</li>
            <li>On-premise .NET Windows Forms Application (inheriting from a complete .NET forms base class/application).</li>
            </ul>
            <span style="font-size:.8em">*When deploying on a VM-Role, the application host type is simply one of the on-premise host types, and deployed as such.</span>
        <p style="font-size:.95em;">
            Across each of these host types, the template code for all parts of the solution (all contained projects) is the same, no matter
            whether deploying on-premise or in the Azure cloud.  The only difference is the host project itself, which simply bootstraps the
            application within the correct startup environment above.  The host class contains no business logic or data access logic: these are
            logically partioned into separate C# projects, common across all hosting environments listed above.
        </p>
        <table align="center">
        <tr>
        <td style="width:960px;">
        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/template1.png" />
        </td>
        </tr>
        <tr>
        <td style="width:960px;">
        <span style="font-size:.8em;font-weight:bold;">First, simply point Visual Studio at the provided template.
        </span><br /><br />
        </td>
        </tr>
        <tr>
        <td style="width:960px;">
        <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/template2.png" />
        </td>
        </tr>
        <tr>
       <td style="width:960px;">
        <span style="font-size:.8em;font-weight:bold;">Next, the wizard asks you for basic information about the type of application/service you want, and
        where to create your configuration and logging databases. Almost all information has default entries, so you just need a couple of
        entries for database location and database password, and you are done!
        </span>
        </td>
        </tr>
        </table>
        <p style="font-size:.95em;">
            The Configuration Service itself is provided as re-usable shared libraries, written entirely in C#, with full sources provided. 
            Utilizing base classes and reflection, it is a standalone service that is not tied to StockTrader 5 specifically. The Configuration Service is simple
            to implement with the new template which creates an n-tier WCF application (or basic ASP.NET application) automatically, with proper partitioning and project structure
            for business applications.  
        </p>
        <p style="font-size:.95em;">
            Once implemented, you will be able to 'login' to your own WCF Services/ASP.NET applications with ConfigWeb (as shown here for StockTrader); 
            and then use all the pages you see here, without any additional work, other than your custom code for business implementation logic and data access logic, of course.
            If you scale your service across many nodes, Configuration Service takes care of virtualizing WCF ServiceHosts, and on configuration updates syncing all 
            running nodes in a 'live' fashion without redeployment or copying updated configuration files to all nodes.
        </p>
            
        <p style="font-size:.95em;">
            On-premise, by implementing the Configuration Service within both client applications and remote service(s) they connect to, 
            the Configuration Service can provide automatic load-balancing and failover as shown with StockTrader. 
        </p>
        <p style="font-size:.95em;">
            Finally, the Configuration Service does not force any particular design pattern on your code, or the use of a runtime API other than a simple bootstrap startup call. 
            Just build your apps and services however you want. The code is not invoked during the normal lifetime of your service, except on startup/shutdown of nodes, and when using ConfigWeb to browse or
            alter a configuration. Hence, it has no impact on performance.  It does, however, necessitate a commitment to using a SQL Server or SQL Azure database to 
            store and manage certain configuration information vs. embedding within configuration files (but that's the point). It's up to the developer
            how much or little of this information they would want to move into the configuration database. All database-stored configuration settings are treated just as if they came from 
            a configuration file, and loaded into local in-memory variables only on host start-up and settings changes, so performance is the same.</p>
            <br />
            <br />
            </asp:Content>

