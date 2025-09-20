<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Portfolio.aspx.cs" Inherits="risk_calculator_webapp.Portfolio" %>
<%@ Register Src="~/Controls/NavigationControl.ascx" TagName="NavigationControl" TagPrefix="rc" %>
<%@ Register Src="~/Controls/PortfolioGridControl.ascx" TagName="PortfolioGridControl" TagPrefix="rc" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Portfolio View - Risk Calculator</title>
    <link href="Content/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <header>
                <h2>Portfolio Analysis Dashboard</h2>
            </header>
            
            <rc:NavigationControl ID="navControl" runat="server" />
            
            <div class="content">
                <rc:PortfolioGridControl ID="portfolioGridControl" runat="server" 
                    OnPortfolioDataLoaded="PortfolioGridControl_PortfolioDataLoaded" />
                
                <div class="portfolio-insights">
                    <h4>Portfolio Insights</h4>
                    <table class="insights-table">
                        <tr>
                            <td>Portfolio Last Updated:</td>
                            <td><asp:Label ID="lblLastUpdated" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Number of Holdings:</td>
                            <td><asp:Label ID="lblHoldingsCount" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Highest Risk Holding:</td>
                            <td><asp:Label ID="lblHighestRisk" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Lowest Risk Holding:</td>
                            <td><asp:Label ID="lblLowestRisk" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                </div>
            </div>
            
            <nav class="bottom-nav">
                <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="~/Default.aspx" CssClass="nav-link">
                    ← Back to Main
                </asp:HyperLink>
            </nav>
        </div>
    </form>
</body>
</html>
