<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonteCarlo.aspx.cs" Inherits="RiskCalculatorWebForm.MonteCarlo" %>

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
                <h2>Monte Carlo Risk Simulation</h2>
            </header>
            
            <div class="content">
                <div class="simulation-controls">
                    <div class="form-group">
                        <label for="txtSimulations">Number of Simulations:</label>
                        <asp:TextBox ID="txtSimulations" runat="server" CssClass="form-control" Text="1000"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revSimulations" runat="server" 
                            ControlToValidate="txtSimulations" 
                            ValidationExpression="^\d+$"
                            ErrorMessage="Please enter a valid number"
                            CssClass="error-message" />
                    </div>
                    
                    <div class="form-group">
                        <asp:Button ID="btnRunSimulation" runat="server" Text="Run Simulation" 
                            CssClass="btn btn-primary" OnClick="btnRunSimulation_Click" />
                    </div>
                </div>
                
                <asp:Panel ID="pnlResults" runat="server" CssClass="results-panel" Visible="false">
                    <div class="simulation-info">
                        <p>Running <asp:Label ID="lblSimulationCount" runat="server"></asp:Label> simulations for portfolio risk scenarios...</p>
                    </div>
                    
                    <table class="results-table">
                        <tr>
                            <td>Simulations Run</td>
                            <td><asp:Label ID="lblSimulationsRun" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>95% VaR (Daily)</td>
                            <td><asp:Label ID="lblVaR95" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>99% VaR (Daily)</td>
                            <td><asp:Label ID="lblVaR99" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Expected Return</td>
                            <td><asp:Label ID="lblExpectedReturn" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                    
                    <div class="chart-placeholder">
                        <p><i>Chart visualization would be displayed here in a production application</i></p>
                    </div>
                </asp:Panel>
                
                <div class="error-panel">
                    <asp:Label ID="lblError" runat="server" CssClass="error-message" Visible="false"></asp:Label>
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
