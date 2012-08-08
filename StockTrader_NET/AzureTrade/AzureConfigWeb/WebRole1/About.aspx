<%@ Page Buffer="true" Title="Configuration Service: About" Language="C#" MasterPageFile="~/Site.master" EnableSessionState="false" AutoEventWireup="true" Inherits="About" Codebehind="About.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <h2>
        About
    </h2>
    <br />
    <table align="center" width="550px" >
    <tr>
    <td style="text-align:center;">
    .NET Configuration Service 5.0.0.0 <br />
         Publication Date 6/09/2011<br /><span><a class="aboutlinks" href="http://social.msdn.microsoft.com/forums/en-US/dotnetstocktradersampleapplication/threads/">
         Support Link</a></span><br /><span><a class="aboutlinks" href="http://msdn.microsoft.com/stocktrader">
         MSDN Download Page</a></span>
    </td>
    </tr>
    <tr> 
         <td style="text-align:center;vertical-align:top;height:400px;">
         <asp:ImageButton ID="ConfigButton" runat="server" ImageUrl="~/Images/AboutBackground.jpg" BorderStyle="Ridge" BorderColor="Black" BorderWidth="3px" PostBackUrl="http://www.microsoft.com/windowsazure/" />
    </td>
    </tr>
    
    </table>
   <br />
</asp:Content>
