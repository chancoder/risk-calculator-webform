<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Portfolio.aspx.cs" Inherits="RiskCalculatorWebForm.Portfolio" %>

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
                <h2>Current Portfolio Holdings</h2>
            </header>
            
            <div class="content">
                <asp:GridView ID="gvPortfolio" runat="server" CssClass="portfolio-grid" 
                    AutoGenerateColumns="false" GridLines="Both">
                    <Columns>
                        <asp:BoundField DataField="Symbol" HeaderText="Symbol" />
                        <asp:BoundField DataField="Value" HeaderText="Value ($)" DataFormatString="{0:N2}" />
                        <asp:BoundField DataField="DailyVaR" HeaderText="Daily VaR" DataFormatString="{0:N2}" />
                        <asp:BoundField DataField="RiskPercentage" HeaderText="Risk %" DataFormatString="{0:F2}%" />
                    </Columns>
                    <HeaderStyle CssClass="grid-header" />
                    <RowStyle CssClass="grid-row" />
                    <AlternatingRowStyle CssClass="grid-alt-row" />
                </asp:GridView>
                
                <div class="portfolio-summary">
                    <h3>Portfolio Summary</h3>
                    <table class="summary-table">
                        <tr>
                            <td>Total Portfolio Value:</td>
                            <td><asp:Label ID="lblTotalValue" runat="server" CssClass="summary-value"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Total Daily VaR:</td>
                            <td><asp:Label ID="lblTotalVaR" runat="server" CssClass="summary-value"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Portfolio Risk Ratio:</td>
                            <td><asp:Label ID="lblRiskRatio" runat="server" CssClass="summary-value"></asp:Label></td>
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
