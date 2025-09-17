<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RiskCalculatorWebForm.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Risk Calculator - Financial Risk Management System</title>
    <link href="Content/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <header>
                <h1>Financial Risk Management System v2.0</h1>
                <h3>ASP.NET Web Forms Application - Modernized!</h3>
            </header>
            
            <nav class="navigation">
                <table class="nav-table">
                    <tr>
                        <td>
                            <asp:HyperLink ID="lnkCalculate" runat="server" NavigateUrl="~/CalculateVar.aspx" CssClass="nav-link">
                                Calculate VaR
                            </asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="lnkPortfolio" runat="server" NavigateUrl="~/Portfolio.aspx" CssClass="nav-link">
                                View Portfolio
                            </asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="lnkMonteCarlo" runat="server" NavigateUrl="~/MonteCarlo.aspx" CssClass="nav-link">
                                Monte Carlo Simulation
                            </asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="lnkAlerts" runat="server" NavigateUrl="~/RiskAlerts.aspx" CssClass="nav-link">
                                Risk Alerts
                            </asp:HyperLink>
                        </td>
                    </tr>
                </table>
            </nav>
            
            <footer>
                <p><i>Current Time: <asp:Label ID="lblCurrentTime" runat="server"></asp:Label></i></p>
            </footer>
        </div>
    </form>
</body>
</html>
