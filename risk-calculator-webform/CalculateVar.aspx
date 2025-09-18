<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CalculateVar.aspx.cs" Inherits="RiskCalculatorWebForm.CalculateVar" %>
<%@ Register Src="~/Controls/NavigationControl.ascx" TagName="NavigationControl" TagPrefix="rc" %>
<%@ Register Src="~/Controls/VarCalculationControl.ascx" TagName="VarCalculationControl" TagPrefix="rc" %>

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
            
            <rc:NavigationControl ID="navControl" runat="server" />
            
            <div class="content">
                <rc:VarCalculationControl ID="varCalcControl" runat="server" 
                    OnVaRCalculated="VarCalcControl_VaRCalculated" 
                    OnCalculationReset="VarCalcControl_CalculationReset" />
                
                <div class="calculation-stats">
                    <h4>Calculation Statistics</h4>
                    <table class="stats-table">
                        <tr>
                            <td>Total Calculations This Session:</td>
                            <td><asp:Label ID="lblTotalCalculations" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Last Calculation Time:</td>
                            <td><asp:Label ID="lblLastCalculationTime" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Session Duration:</td>
                            <td><asp:Label ID="lblSessionDuration" runat="server"></asp:Label></td>
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
