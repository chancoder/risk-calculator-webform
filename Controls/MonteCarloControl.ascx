<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MonteCarloControl.ascx.cs" Inherits="RiskCalculatorWebForm.Controls.MonteCarloControl" %>

<div class="monte-carlo-control">
    <div class="simulation-controls">
        <h4>Monte Carlo Risk Simulation</h4>
        
        <div class="form-group">
            <label for="txtSimulations">Number of Simulations:</label>
            <asp:TextBox ID="txtSimulations" runat="server" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvSimulations" runat="server" 
                ControlToValidate="txtSimulations" 
                ErrorMessage="Number of simulations is required"
                CssClass="error-message" />
            <asp:RegularExpressionValidator ID="revSimulations" runat="server" 
                ControlToValidate="txtSimulations" 
                ValidationExpression="^\d+$"
                ErrorMessage="Please enter a valid number"
                CssClass="error-message" />
            <asp:RangeValidator ID="rvSimulations" runat="server"
                ControlToValidate="txtSimulations"
                Type="Integer"
                MinimumValue="100"
                MaximumValue="10000"
                ErrorMessage="Simulations must be between 100 and 10,000"
                CssClass="error-message" />
        </div>
        
        <div class="form-group">
            <asp:Button ID="btnRunSimulation" runat="server" Text="Run Simulation" 
                CssClass="btn btn-primary" OnClick="btnRunSimulation_Click" />
            <asp:Button ID="btnClearHistory" runat="server" Text="Clear History" 
                CssClass="btn btn-secondary" OnClick="btnClearHistory_Click" />
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
            <tr>
                <td>Simulation Time</td>
                <td><asp:Label ID="lblSimulationTime" runat="server"></asp:Label></td>
            </tr>
        </table>
        
        <div class="chart-placeholder">
            <p><i>Chart visualization would be displayed here in a production application</i></p>
        </div>
    </asp:Panel>
    
    <div class="simulation-history">
        <h5>Recent Simulations</h5>
        <asp:GridView ID="gvHistory" runat="server" CssClass="history-grid" 
            AutoGenerateColumns="false" GridLines="Both">
            <Columns>
                <asp:BoundField DataField="Timestamp" HeaderText="Time" />
                <asp:BoundField DataField="SimulationCount" HeaderText="Simulations" />
                <asp:BoundField DataField="VaR95" HeaderText="95% VaR" DataFormatString="{0:F4}%" />
                <asp:BoundField DataField="VaR99" HeaderText="99% VaR" DataFormatString="{0:F4}%" />
                <asp:BoundField DataField="ExpectedReturn" HeaderText="Expected Return" DataFormatString="{0:F4}%" />
                <asp:BoundField DataField="ExecutionTime" HeaderText="Time (ms)" />
            </Columns>
            <HeaderStyle CssClass="grid-header" />
            <RowStyle CssClass="grid-row" />
            <AlternatingRowStyle CssClass="grid-alt-row" />
        </asp:GridView>
    </div>
    
    <div class="error-panel">
        <asp:Label ID="lblError" runat="server" CssClass="error-message" Visible="false"></asp:Label>
    </div>
    
    <asp:HiddenField ID="hfSimulationHistory" runat="server" />
    <asp:HiddenField ID="hfLastSimulationCount" runat="server" />
</div>
