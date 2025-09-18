<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonteCarlo.aspx.cs" Inherits="RiskCalculatorWebForm.MonteCarlo" %>
<%@ Register Src="~/Controls/NavigationControl.ascx" TagName="NavigationControl" TagPrefix="rc" %>
<%@ Register Src="~/Controls/MonteCarloControl.ascx" TagName="MonteCarloControl" TagPrefix="rc" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Monte Carlo Simulation - Risk Calculator</title>
    <link href="Content/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <header>
                <h2>Monte Carlo Risk Simulation Dashboard</h2>
            </header>
            
            <rc:NavigationControl ID="navControl" runat="server" />
            
            <div class="content">
                <rc:MonteCarloControl ID="monteCarloControl" runat="server" 
                    OnSimulationCompleted="MonteCarloControl_SimulationCompleted" />
                
                <div class="simulation-statistics">
                    <h4>Simulation Statistics</h4>
                    <table class="stats-table">
                        <tr>
                            <td>Total Simulations This Session:</td>
                            <td><asp:Label ID="lblTotalSimulations" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Last Simulation Time:</td>
                            <td><asp:Label ID="lblLastSimulationTime" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Average Simulation Time:</td>
                            <td><asp:Label ID="lblAverageSimulationTime" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Fastest Simulation:</td>
                            <td><asp:Label ID="lblFastestSimulation" runat="server"></asp:Label></td>
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
