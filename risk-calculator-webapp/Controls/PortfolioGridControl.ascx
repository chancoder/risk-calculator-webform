<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PortfolioGridControl.ascx.cs" Inherits="risk_calculator_webapp.Controls.PortfolioGridControl" %>

<div class="portfolio-grid-control">
    <div class="portfolio-header">
        <h3>Current Portfolio Holdings</h3>
        <asp:Button ID="btnRefresh" runat="server" Text="Refresh Data" 
            CssClass="btn btn-primary" OnClick="btnRefresh_Click" />
    </div>
    
    <asp:GridView ID="gvPortfolio" runat="server" CssClass="portfolio-grid" 
        AutoGenerateColumns="false" GridLines="Both" OnRowDataBound="gvPortfolio_RowDataBound">
        <Columns>
            <asp:BoundField DataField="Symbol" HeaderText="Symbol" />
            <asp:BoundField DataField="Value" HeaderText="Value ($)" DataFormatString="{0:N2}" />
            <asp:BoundField DataField="DailyVaR" HeaderText="Daily VaR" DataFormatString="{0:N2}" />
            <asp:BoundField DataField="RiskPercentage" HeaderText="Risk %" DataFormatString="{0:F2}%" />
            <asp:TemplateField HeaderText="Risk Level">
                <ItemTemplate>
                    <asp:Label ID="lblRiskLevel" runat="server" 
                        Text='<%# Eval("RiskLevel") %>' 
                        CssClass='<%# "risk-level " + Eval("RiskLevel").ToString().ToLower() %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <HeaderStyle CssClass="grid-header" />
        <RowStyle CssClass="grid-row" />
        <AlternatingRowStyle CssClass="grid-alt-row" />
    </asp:GridView>
    
    <div class="portfolio-summary">
        <h4>Portfolio Summary</h4>
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
            <tr>
                <td>Last Updated:</td>
                <td><asp:Label ID="lblLastUpdated" runat="server" CssClass="summary-value"></asp:Label></td>
            </tr>
        </table>
    </div>
    
    <asp:HiddenField ID="hfPortfolioData" runat="server" />
    <asp:HiddenField ID="hfLastRefreshTime" runat="server" />
</div>
