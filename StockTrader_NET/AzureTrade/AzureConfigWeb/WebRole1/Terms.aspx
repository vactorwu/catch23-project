<%@ Page Title="Configuration Service: Terms" Buffer="true" Language="C#" MasterPageFile="~/Site.master" EnableSessionState="false" AutoEventWireup="true" Inherits="ConfigService.ServiceConfiguration.Web.Terms" Codebehind="Terms.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   
    <h2>
       Glossary
    </h2>
    <br />
    <br />
   <table width="900" align="center" style="border-collapse:collapse;border:#39454F 2px solid;"> 
        <tr>
            <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid;">Term</th>
            <th class="TradeConfigTHStyle2" style="border-right:1px #c1c1c1 solid;">Description</th>
                 
        </tr>
     <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">
     Azure-hosted
     </td>
      <td class="TradeConfigTDSettingStyle1a" style="text-align:left;">An application or service running in the Windows Azure public cloud.  Microsoft operates multiple, large scale data centers throughout the world
      that enable organizations to deploy and run applications without buying and maintaining their own equipment within on-premise data centers.  Applications running on Windows Azure can be publicly accessible over the
      Internet, or private applications.  They can also securely integrate with on-premise resources for <b>hybrid cloud</b> deployments.  The platform is provided as a service, and this means customers do not have to do
      any machine or network management (as opposed to "infrastructure as a service", where the computer(s) are remote, but the customer still has to maintain all the remote computers they are paying for).  
      Windows Azure can host applications based on .NET, Java, Ruby and PHP.  Windows Azure-hosted applications can be seamlessly scaled across many instances (vm nodes) to provide high reliability and scalability.  Applications
      can be easily developed using .NET and Visual Studio (or the other development environments listed above); and simply "published" to the Azure cloud.
      </td>
      </tr>
      <tr>
      <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">
     Azure AppFabric
     </td>

     <td class="TradeConfigTDSettingStyle1a" style="text-align:left;">
     New middle tier, cloud-optimized services that are provided on Windows Azure.  They enable developers to build a new generation of cloud-connected applications that are highly secure, integrated and scalable. The services today (some in preview CTP) include
     Azure AppFabric Caching Services; Azure AppFabric Access Control Service; Azure AppFabric Service Bus; Azure AppFabric Integration; and a new Azure AppFabric application composition model.  All work in conjunction with the existing  .NET Framework 4.0 development technologies and
     Visual Studio.
     </td>
     </tr>
     <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">basicHttpBinding</td>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:left;">
     This is the Windows Communication Foundation (WCF) binding that supports basic SOAP 1.1/1.2 services. It can be used with transport security, operating over https/SSL, for creating secure connections between service clients and remote services.
     It is the most interoperable binding, providing excellent interoperability between .NET, Java, PHP and many other runtime platforms on a wide variety of operating systems.</td>
     </tr>
      <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">Binding</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">
     A WCF term that defines the transport protocol, encoding standards, and security used for communication with services. 
     WCF supports many different types of bindings, and does not require developers to build any extra code for a service to support multiple bindings at the same time.</td>
     </tr>
     <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">Binding Configuration</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">
     A WCF term that defines the custom configuration settings for a specific binding used by an application or service.  Typically, binding configuration information for clients is 
     automatically generated via the <a href="http://msdn2.microsoft.com/en-us/library/aa347733.aspx">svcutil.exe</a> proxy-generation application that is part of the Windows SDK. 
     This is how the binding configuration information for StockTrader was generated--then it was simply embedded in the config file for the service client.</td>
     </tr>
     <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">BSL</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">The Business Services Layer that defines the business operations and all business logic for an application or service.
     This is a middle-tier layer, and can be easily exposed via a WCF service contract as a Web Service, ala the StockTrader Business Services Layer and Order Processing Service. The BSL uses a data access layer (DAL) for database queries/information exchange in a properly logically-partitioned, n-tier enterprise application. While the BSL is often
     configured to run remotely with respect to the User Interface Layer (UI); typically the DAL runs in-process with the BSL; hence there is a <strong>logical</strong> partitioning between the BSL and the DAL, but not (typically)
     a <strong>physical</strong> paritioning. Note that in most systems, the database itself will always reside on a separate, dedicated physical server(s) from the BSL layer.</td>
     </tr>
     <tr>
    <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">Channel</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">A connection established between a service client and a service host.  
    The channel's properties (transport protocol, security settings, timeouts, etc.) are determined based on the binding configuration information used when creating the channel.</td>
     </tr>
     <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">Configuration Database</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">The configuration database (aka configuration repository) is the database and database s
     chema holding the configuration settings for a specific service host.  
     Note that configuration settings are specific to the host program, not the service, since a service might be hosted in multiple programs, each requiring different configuration settings. 
     The configuration database is also the set of tables that stores the node management system information, and allows for service node clustering and dynamic information to be exchanged between live service nodes.  
     Service hosts initialize themselves from their configuration database on startup.  The connection string to the configuration database is contained in the Web.config (IIS-hosted services/apps) or the *.exe.config (self-hosted services) file.</td>
     </tr>
      <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">Configuration Setting</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">
     A configuration setting is a unique row defined in the configuration database that is used by a 
     specific service/application for configuration purposes.  Configuration settings (configuration keys) 
     are viewed/updated via the ConfigWeb application.  Anything a developer might otherwise store in an appSettings section 
     of a web.config or .exe.config file can be stored in the configuration database. Each configuration setting 
     points to a specific service/application member variable defined in it's Settings.cs file.  On startup or 
     on update to any configuration setting, .NET <strong>reflection</strong> is used to set this member variable's
      value with the value defined in the configuration database.  Settings are treated like constants--and they are custom 
      settings used by the developer to determine application behavior--such as binding/endpoint information; 
      transaction timeouts/behavior, database connection information, or any type of information that might be 
      used to configure a service/application.  Developers can define any number of 
     custom-configuration settings they desire, and store these in the configuration database.  
     By storing them in the configuration database vs. statically compiling them into the application, 
     live updates can be made to change a service's behavior without re-compiling and re-deploying 
     application-specific code. Also, since these settings are centrally stored, 
     clustered service nodes will initialize themselves with the same values--keeping 
     in sync across load-balanced nodes. As updates are made to any configuration setting, 
     the Configuration Management system automatically notifies/updates clustered node instances 
     so they remain in sync without requiring restarts.</td>
     </tr>
     <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">Connected Service</td>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:left;">A service that is utilized by a client application or client service.  The
     Configuration Service define both Connected Services and <strong>Generic Connected Services</strong>.  Generic services
     are simply those that do not implement the Configuration Service.  These can be fully integrated, but do
     not support the centralized management user interface or node notification services. For example, it is possible
     with StockTrader to define any number of connections to IBM WebSphere Trade 6.1 (which does not implement the Configuration Service) 
     across different WebSphere servers; and still load-balance
     against these nodes from the StockTrader Web application.  However, WebSphere Trade 6.1 does not support centralized configuration management, and each node must
     be separately configured with it's application-specific settings (such as Order Mode and Access Mode via IBM's Trade 6.1 JSP configuration page.</td>
     </tr>
     <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">Connection Point</td>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:left;">
     A Connection Point is a connection (a Channel) between a service 
     client and a service host.  It is unique based on the endpoint address (URI) to the service.  
     The Connections tab in the configuration management system is used to view/delete/add connections 
     between service clients and service hosts. Note that many connections can be configured, 
     whether or not they are active depends on the configuration setup of the StockTrader application 
     (for example, the Web App/AccessMode and Business Services/OrderMode settings). 
     Connections use a specific WCF 'binding' that determines the transport protocol, 
     encoding standards and security used for communication between services. WCF automatically 
     manages connections, including pooling connections between service clients and connected services.  
     The number of actual OS-connections allowed per outbound address (URI) for Http connections is determined based on 
     the ServicePointManager.DefaultConnectionLimit set either programmatically 
     (ala .NET StockTrader via the DEFAULT_CONNECTION_LIMIT configuration database setting), 
     or via a .NET configuration file (machine.config, web.config or exe.config via the system.net--> connectionManagement--> maxconnection setting, set per outbound address).</td>
     </tr>
     <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">DAL</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">
     The Data Access Layer for the application.  
     This layer isolates all database-specific communication in a single class.  An application can support multiple DALs and connect to multiple database types without requiring any changes/additional logic 
     in the business service layer (BSL) or user-interface layer (UI). In StockTrader, model classes are used to represent database information, and these are passed between tiers for database updates and database record display. </td>
     </tr>
     <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">Data Contract</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">This is the class that defined the schema for the data that will be exchanged between services.  For example, in StockTrader, the Customer information is defined in the AccountDataModel class, which is in turn 'attributed' as a WCF Data Contract class.  These classes will be automatically serialized by WCF based on SOAP/XML or binary serialization based on the encoding standard defined with the binding configuration used when creating the Channel between services. </td>
     </tr>
     <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">DEFAULT_CONNECTION_LIMIT</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">The StockTrader configuration database-based configuration setting that determines the maximum outbound connections .NET will create for HTTP-based connections, per unique outbound address.  In .NET, the default is throttled to 2 connections per address.  This is fine for desktop applications, but should be adjusted for server-based applications that are service clients, as it will create a bottleneck and greatly limit system performance.  This is one of the most commonly-missed tuning settings for Web Services applications.  Note that this setting is picked up on .NET application startup, and is the one configuration database setting that does require a process re-start to become effective if changed.  .NET StockTrader is configured by default with enough connections allowed for high-throughput situations.</td>
     </tr>
     <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">Durable Message Queue</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">A message queue that is persisted to disk, such that messages are preserved on server restarts/system crashed. With MSMQ, this is also known as a transacted queue. The Windows Server (Enterprise Edition)operating system will also support clusting of MSMQ nodes (up to 8 redundant nodes) to ensure system reliability. For mission-critical applications, RAID arrays are typically used in conjunction with Windows Clustering Services to ensure system availability and assured message delivery.</td>
     </tr>
     <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">Endpoint</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">A network address to a service or web site.  On Windows Azure, Endpoints are defined in Visual Studio for the role.  You can define internal and input endpoints.  Input endpoints are Internet-facing, and automatically load balanced by the Windows Azure Fabric Controller. Internal endpoints are only visible between nodes, and not exposed to the Internet.  Configuration service uses internal endpoints for the node service, for example.</td>
     </tr>
     <tr>
    <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">Hosted Service</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">
    A service, defined with a specific service contract, that is a Host Service process at a specific endpoint.  For example, the StockTrader Business Services self-host program
    has 6 hosted services:  http and tcp endpoints for both the Configuration Service and Node Service (4 total); and http and tcp endpoints for the Business Services themselves (ITradeServices).
    </td>
    </tr>
     <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">IIS-Hosted Service</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">WCF Services can be hosted in Internet Information Server, where they run under IIS security and management infrastructure, or they can be hosted in a custom program-called self-hosting.  With IIS 7.0 (Vista and Windows Server 2008), IIS-hosted services can support any WCF binding.  IIS 6.0-hosted services can only support HTTP-based bindings as a transport protocol.</td>
     </tr>
     <tr>
    <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">Node</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">
    An instance of a Service Host running a specific server.  See also <strong>Service Node</strong>.
    </td>
    </tr>
    <tr>
    <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">netMsmqBinding</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">The message-queue bindings that allows WCF services to use Microsoft MSMQ message queueing as the transport.  It is used to construct loosely-coupled services.  The .NET StockTrader Order Processor Service uses this binding (as well as other optional bindings) for message-based exchange of orders from the Business Services middle tier service.</td>
    </tr>
     <tr>
    <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">netTcpBinding</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">The WCF binding that allows WCF services to directly use the TCP/IP transport protocol (not HTTP) for communication. When combined with a binding configuration that specifies 'BinaryMessageEncoding', this binding will allow WCF to use binary encoding (not XML) to communicate from a .NET client to a .NET service, hence increasing performance (by avoiding XML serialization). Note that WCF allows multiple transports and encodings (binding configurations) to be simultaneously supported by the same service host; hence no interoperability is lost by additionally supporting this .NET binary encoding technique.  
    The .NET StockTrader Web application supports communication over TCP/binary encoding to the StockTrader remote business services via the Tcp_Binary setting for AccessMode.</td>
    </tr>
    <tr>
    <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">Poison Message</td>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:left;">
     A message that in some way is corrupt, and hence cannot be sucessfully processed from the message queue. WCF will automatically detect poison messages, and fire a Poison Message Event that can be handled gracefully.  In .NET StockTrader, poison messages are re-tried based on a WCF binding setting 3-times; and then automatically sent to a separate Poison Message queue for later processing/audits. You can see this in action by buying the stock ACIDBUY (or selling the stock ACIDSELL).  Monitor the Error Console in the Order Processing Service to see the live messages with the flow of retries and ultimate poison message handling.</td>
     </tr>
     <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">Proxy</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">A class that allows a client to communicate with a remote service.  Proxies can be generated automatically via the <a href="http://msdn2.microsoft.com/en-us/library/aa347733.aspx">svcutil.exe</a> (Windows/.NET 3.5 SDK) program by simply pointing to the desired service.  While the generated proxy class can be used within a client application; it is more advantageous with .NET 3.5/WCF to generate a proxy to get the data contract and method/operation signatures for a service, and then use this generated information to form a custom client that uses the WCF Channel Factory class to create the actual connections. Channels can then be easily cached, as with StockTrader, to greatly increase performance of a client application. Typically only one Channel instance needs to be cached per service client. Cached channels are thread-safe based on the WCF/Windows networking infrastructure.</td>
     </tr>
     <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">Self-Host/Self-Hosted Service</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">WCF Services can be hosted in IIS; or they can be hosted in a custom program-called self-hosting. Self hosts can be Windows NT Services, console applications, or Windows applications.  .NET StockTrader self-hosts both the Order Processor Service and the Business Service (middle-tier services) in Windows Applications created in .NET. Business Services are also hosted in IIS, and the Web Application can be configured to use either the IIS-hosted or self-hosted Business Services via the AccessMode configuration setting.</td>
     </tr>
     <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">Service</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">A service is an autonomous 'black-box' set of functionality, defined as a set of operations (methods that can be called remotely) to support the functionality the service provides.  With WCF, services respond over a set of defined transport protocols and encoding standards, including SOAP/Web services and WS-* industry standards to allow for maximum performance and a wide
     variety of interoperability scenarios with non-Microsoft platforms.</td>
     </tr>
     <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">Service Behaviors and Endpoint Behaviors</td>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:left;">This is a WCF construct that specifies additional configuration information for a service, and is applied either
      programmatically or via .NET configuration files (web.config or exe.config files). Behaviors are used in 
      StockTrader in a couple of areas; first, a Service Behavior (TradeServiceBehavior) is set via the config files to configure the maximum number of concurrent instances for the Trader Business Services and Order Processing Service.  This is a performance-oriented tuning setting, as concurrent instances are throttled by default.  Second, an
      Endpoint Behavior is set programmatically in the Order Processing Service to set a maximum transaction batch size that determines how many
      messages are read off the queue at a time and processed as part of a single atomic distributed transaction against the database.  This is also a performance-oriented setting, since
      transaction batching can lead to increased throughput (although must be balanced against database concurrency considerations). 
     </td>
     </tr>
     <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">Service Contract</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">The set of operations (methods) a sevice supports.  These are defined in an interface; they are implemented in an implementation class deriving from this interface.</td>
     </tr>
     <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">Service Host</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">The name of the unique application/process hosting a set of services. The HostNameIdentifier is the same as the service host name.  In .NET StockTrader, services are both self-hosted, and hosted in IIS; however, each separate type of service host program should have a unique name, even if sharing a service implementation class.</td>
     </tr>
     <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">Service Implementation</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">A class that derives from the <strong>Service Contract</strong> interface; and that implements the actual operations defined in that contract.</td>
     </tr>
     <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">Service Name</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">The name of the service; the Configuration Management service uses the implementation namespace + class name as the service name.  Services might be hosted in multiple service hosts; hence a service name might not be unique; but the host name + the service name is.</td>
     </tr>
     <tr>
    <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">Service Node</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">An instance of a service host running on a unique server and listening on this server's tcp/ip address(es).  Service nodes are clustered automatically via the Configuration Management services, into a virtualized service cluster.</td>
     </tr>
     <tr>
    <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">SQL Azure</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">
    SQL Azure is a cloud-based implementation of Microsoft SQL Server, running in a worldwide datagrid and made available to customers as a complete, enterprise relational database service in the cloud.
    Developers can use all their existing SQL Server knowledge and tools, but use a high-performance, highly available RDBMs database without buying, setting up or maintaining any database software themselves.  Azure StockTrader 5.0
    uses a SQL Azure database as its backing database when deployed to the cloud; and typically an on-premise SQL Server when deployed on-premise: all with one code base.  On-premise applications can also connect to and use SQL Azure databases in the cloud
    just as if the database was running locally.  SQL Azure provides high availability by maintaining failover replicas automatically, and Microsoft does all management and maintanence of the database software itself.  So unlike "infrastructure as a service" databases
    deployed to remote servers, setup and management of the database servers and sofwtare itself is automatic, for greatly reduced management costs and overhead.</td>
     </tr>
     <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">UI</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">The user-interface layer of an application.  This layer is the layer a user interacts with, and may be a HTML-Web-based application, a desktop/smart-client application, or a mobile device application.  .NET supports any number of client/user interface types.  For example, the .NET StockTrader supports both a Web-based UI constructed using ASP.NET; and a smart-client UI constructed using .NET and Windows Presentation Foundation (WPF). All client-types connect to the same remote services.  Since WCF is based on industry standards, any client can connect to any Web Service, no matter the platform is developed on (for example Java/JE/J2EE or .NET) or the platform it is hosted on.</td>
     </tr>

     <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">WCF/Windows Communication Foundation</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">Windows Communication Foundation is the part of .NET 3.5 that defines the remoting infrastructure, intra-process communication standards and programming model for building service-oriented applications using .NET. It is fundamentally based on open industry Web Service standards, and implements the WS-* advanced Web Services industry-standard specifications.</td>
     </tr>
     <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">WF/Windows Workflow Foundation</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">Windows Workflow Foundation is the part of .NET 3.5 that defines advanced human and system workflow capabilities for the .NET platform.  The StockTrader application (and WebSphere Trade 6.1) do not implement workflow to date; although the StockTrader scenario might be extended in the future to demonstrate these capabilities.</td>
     </tr>
     <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">WPF/Windows Presenation Foundation</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">Windows Presenation Foundation is the part of .NET 3.5 that defines advanced smart-client UI capabilities.  The .NET StockTrader includes a WPF-based client to the middle-tier .NET StockTrader (or WebSphere Trade 6.1) services.</td>
     </tr>
     <tr>
     <td class="TradeConfigTDSettingStyle1a" style="text-align:right;">ws2007HttpBinding</td><td class="TradeConfigTDSettingStyle1a" style="text-align:left;">This is the WCF binding that supports WS-* industry standards for advanced service operation such as WS-Security, WS-Atomic Transactions, WS-Reliable Messaging and others.</td>
     </tr>
   
</table>
<br /><br />

    
</asp:Content>

