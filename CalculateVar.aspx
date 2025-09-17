<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CalculateVar.aspx.cs" Inherits="RiskCalculatorWebForm.CalculateVar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>VaR Calculation - Risk Calculator</title>
    <link href="Content/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <header>
                <h2>Value at Risk (VaR) Calculator</h2>
            </header>
            
            <div class="content">
                <asp:Panel ID="pnlForm" runat="server" CssClass="form-panel">
                    <div class="form-group">
                        <label for="txtSymbol">Stock Symbol:</label>
                        <asp:TextBox ID="txtSymbol" runat="server" CssClass="form-control" Text="AAPL"></asp:TextBox>
                    </div>
                    
                    <div class="form-group">
                        <label for="txtAmount">Amount ($):</label>
                        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" Text="100000"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revAmount" runat="server" 
                            ControlToValidate="txtAmount" 
                            ValidationExpression="^\d+(\.\d{1,2})?$"
                            ErrorMessage="Please enter a valid amount"
                            CssClass="error-message" />
                    </div>
                    
                    <div class="form-group">
                        <asp:Button ID="btnCalculate" runat="server" Text="Calculate Risk" 
                            CssClass="btn btn-primary" OnClick="btnCalculate_Click" />
                    </div>
                </asp:Panel>
                
                <asp:Panel ID="pnlResults" runat="server" CssClass="results-panel" Visible="false">
                    <h3>Risk Analysis for <asp:Label ID="lblSymbol" runat="server"></asp:Label></h3>
                    
                    <table class="results-table">
                        <tr>
                            <td>Investment Amount</td>
                            <td><asp:Label ID="lblAmount" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Daily VaR (95%)</td>
                            <td><asp:Label ID="lblVaR" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Credit Risk Score</td>
                            <td><asp:Label ID="lblCreditRisk" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Risk Level</td>
                            <td><asp:Label ID="lblRiskLevel" runat="server" CssClass="risk-level"></asp:Label></td>
                        </tr>
                    </table>
                    
                    <div class="form-group">
                        <asp:Button ID="btnNewCalculation" runat="server" Text="New Calculation" 
                            CssClass="btn btn-secondary" OnClick="btnNewCalculation_Click" />
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
