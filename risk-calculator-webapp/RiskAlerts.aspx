<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RiskAlerts.aspx.cs" Inherits="risk_calculator_webapp.RiskAlerts" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Risk Alerts - Risk Calculator</title>
    <link href="Content/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <header>
                <h2>Risk Alert System</h2>
            </header>
            
            <div class="content">
                <div class="alerts-controls">
                    <asp:Button ID="btnClearAlerts" runat="server" Text="Clear All Alerts" 
                        CssClass="btn btn-danger" OnClick="btnClearAlerts_Click" 
                        OnClientClick="return confirm('Are you sure you want to clear all alerts?');" />
                </div>
                
                <asp:Panel ID="pnlNoAlerts" runat="server" CssClass="no-alerts-panel" Visible="false">
                    <div class="alert alert-info">
                        <strong>No Risk Alerts</strong>
                        <p>No risk alerts at this time. The system will automatically generate alerts when risk thresholds are exceeded.</p>
                    </div>
                </asp:Panel>
                
                <asp:Panel ID="pnlAlertsList" runat="server" CssClass="alerts-list-panel" Visible="false">
                    <h3>Active Risk Alerts</h3>
                    <asp:Repeater ID="rptAlerts" runat="server">
                        <ItemTemplate>
                            <div class="alert alert-warning alert-item">
                                <span class="alert-time"><%# Eval("Time") %></span>
                                <span class="alert-message"><%# Eval("Message") %></span>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </asp:Panel>
                
                <div class="alert-stats">
                    <p><strong>Total Alerts:</strong> <asp:Label ID="lblAlertCount" runat="server" Text="0"></asp:Label></p>
                    <p><strong>Last Updated:</strong> <asp:Label ID="lblLastUpdated" runat="server"></asp:Label></p>
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
