<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="risk_calculator_webapp.Default" %>
<%@ Register Src="~/Controls/NavigationControl.ascx" TagName="NavigationControl" TagPrefix="rc" %>

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
                <h1>Financial Risk Management System v2.1</h1>
                <h3>ASP.NET Web Forms Application - Enhanced with ASCX Controls!</h3>
            </header>
            
            <rc:NavigationControl ID="navControl" runat="server" />
            
            <div class="welcome-message">
                <h4>Welcome to the Risk Calculator</h4>
                <p>This application provides comprehensive financial risk analysis tools including VaR calculations, portfolio analysis, Monte Carlo simulations, and risk alert management.</p>
                
                <div class="feature-highlights">
                    <h5>Key Features:</h5>
                    <ul>
                        <li><strong>Value at Risk (VaR) Calculations</strong> - Calculate risk metrics for individual securities</li>
                        <li><strong>Portfolio Analysis</strong> - View comprehensive portfolio risk metrics</li>
                        <li><strong>Monte Carlo Simulations</strong> - Run statistical risk simulations</li>
                        <li><strong>Risk Alerts</strong> - Monitor and manage risk alerts</li>
                        <li><strong>State Management</strong> - ViewState and SessionState for data persistence</li>
                    </ul>
                </div>
            </div>
            
            <footer>
                <p><i>Current Time: <asp:Label ID="lblCurrentTime" runat="server"></asp:Label></i></p>
                <p><i>Session ID: <asp:Label ID="lblSessionId" runat="server"></asp:Label></i></p>
            </footer>
        </div>
    </form>
</body>
</html>
