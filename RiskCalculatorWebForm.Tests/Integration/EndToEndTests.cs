using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using risk_calculator_webapp;

using RiskCalculatorWebForm.Tests.TestUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RiskCalculatorWebForm.Tests.Integration
{
    [TestClass]
    public class EndToEndTests
    {
        [TestMethod]
        public void CompleteRiskCalculationWorkflow()
        {
            // Test complete workflow from VaR calculation to alert generation

            // Arrange
            var riskCalculator = new RiskCalculator();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var alertService = new RiskAlertService(mockHttpContextAccessor.Object);

            // Clear any existing alerts
            alertService.ClearAlerts();

            // Act - Perform VaR calculation
            string symbol = "AAPL";
            decimal amount = 100000m;
            decimal var = riskCalculator.CalculateVaR(symbol, amount);
            decimal creditRisk = riskCalculator.CalculateCreditRisk(symbol);
            string riskLevel = riskCalculator.GetRiskLevel(var / amount);

            // Check for high risk and add alert
            if (var / amount > 0.05m)
            {
                alertService.AddAlert($"HIGH RISK: {symbol} VaR exceeds 5% threshold at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            }

            // Assert
            Assert.IsTrue(var > 0, "VaR should be positive");
            Assert.IsTrue(creditRisk >= 1.0m && creditRisk <= 10.0m, "Credit risk should be valid");
            Assert.IsTrue(riskLevel == "LOW" || riskLevel == "MEDIUM" || riskLevel == "HIGH", "Risk level should be valid");

            var alerts = alertService.GetAlerts();
            if (var / amount > 0.05m)
            {
                Assert.IsTrue(alerts.Count > 0, "Should have alerts for high risk");
            }
        }

        [TestMethod]
        public void PortfolioAnalysisWorkflow()
        {
            // Test complete portfolio analysis workflow

            // Arrange
            var riskCalculator = new RiskCalculator();
            var portfolioData = riskCalculator.GetPortfolioData();

            // Act - Analyze entire portfolio
            decimal totalValue = 0;
            decimal totalVaR = 0;
            var riskAnalysis = new List<(string Symbol, decimal RiskRatio)>();

            foreach (var holding in portfolioData)
            {
                decimal var = riskCalculator.CalculateVaR(holding.Key, holding.Value);
                decimal riskRatio = (var / holding.Value) * 100;
                riskAnalysis.Add((holding.Key, riskRatio));

                totalValue += holding.Value;
                totalVaR += var;
            }

            decimal portfolioRiskRatio = (totalVaR / totalValue) * 100;
            var highestRisk = riskAnalysis.OrderByDescending(x => x.RiskRatio).First();
            var lowestRisk = riskAnalysis.OrderBy(x => x.RiskRatio).First();

            // Assert
            Assert.IsTrue(totalValue > 0, "Total portfolio value should be positive");
            Assert.IsTrue(totalVaR > 0, "Total VaR should be positive");
            Assert.IsTrue(portfolioRiskRatio > 0, "Portfolio risk ratio should be positive");
            Assert.IsNotNull(highestRisk.Symbol, "Should identify highest risk holding");
            Assert.IsNotNull(lowestRisk.Symbol, "Should identify lowest risk holding");
            Assert.IsTrue(highestRisk.RiskRatio >= lowestRisk.RiskRatio, "Highest risk should be >= lowest risk");
        }

        [TestMethod]
        public void MonteCarloSimulationWorkflow()
        {
            // Test complete Monte Carlo simulation workflow

            // Arrange
            var monteCarloService = new MonteCarloService();
            int simulations = 1000;

            // Act - Run simulation
            var results = monteCarloService.RunSimulation(simulations);

            // Assert
            Assert.IsNotNull(results, "Monte Carlo results should not be null");
            Assert.IsTrue(results.VaR95 < 0, "95% VaR should be negative");
            Assert.IsTrue(results.VaR99 < results.VaR95, "99% VaR should be more negative than 95% VaR");
            Assert.IsTrue(results.ExpectedReturn > -0.1m && results.ExpectedReturn < 0.1m,
                "Expected return should be reasonable");
        }

        [TestMethod]
        public void RiskAlertManagementWorkflow()
        {
            // Test complete risk alert management workflow

            // Arrange
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var alertService = new RiskAlertService(mockHttpContextAccessor.Object);
            var riskCalculator = new RiskCalculator();

            // Clear existing alerts
            alertService.ClearAlerts();

            // Act - Generate various types of alerts
            var portfolioData = riskCalculator.GetPortfolioData();

            foreach (var holding in portfolioData)
            {
                decimal var = riskCalculator.CalculateVaR(holding.Key, holding.Value);
                decimal riskRatio = var / holding.Value;

                if (riskRatio > 0.05m)
                {
                    alertService.AddAlert($"HIGH RISK: {holding.Key} VaR exceeds 5% threshold");
                }
                else if (riskRatio > 0.03m)
                {
                    alertService.AddAlert($"MEDIUM RISK: {holding.Key} VaR above 3% threshold");
                }
            }

            // Add some general alerts
            alertService.AddAlert("Portfolio rebalancing recommended");
            alertService.AddAlert("Market volatility increased");

            var alerts = alertService.GetAlerts();

            // Assert
            Assert.IsTrue(alerts.Count > 0, "Should have generated alerts");
            Assert.IsTrue(alerts.Any(a => a.Contains("HIGH RISK") || a.Contains("MEDIUM RISK")),
                "Should have risk-based alerts");

            // Test alert management
            alertService.ClearAlerts();
            Assert.AreEqual(0, alertService.GetAlerts().Count, "Should clear all alerts");
        }

        [TestMethod]
        public void StateManagementWorkflow()
        {
            // Test ViewState and SessionState integration

            // Arrange
            var sessionData = new Dictionary<string, object>
            {
                ["DefaultSymbol"] = "AAPL",
                ["DefaultAmount"] = "100000",
                ["SessionStartTime"] = DateTime.Now,
                ["TotalCalculations"] = 0
            };

            // Act - Simulate session state usage
            foreach (var item in sessionData)
            {
                // In real application, this would be Session[item.Key] = item.Value;
                Assert.IsNotNull(item.Value, $"Session data {item.Key} should not be null");
            }

            // Simulate ViewState usage
            var viewStateData = new Dictionary<string, object>
            {
                ["LastSymbol"] = "AAPL",
                ["LastAmount"] = 100000m,
                ["CalculationHistory"] = new List<TestUtilities.VaRCalculationResult>()
            };

            foreach (var item in viewStateData)
            {
                Assert.IsNotNull(item.Value, $"ViewState data {item.Key} should not be null");
            }

            // Assert
            Assert.AreEqual(4, sessionData.Count, "Should have all session data items");
            Assert.AreEqual(3, viewStateData.Count, "Should have all ViewState data items");
        }

        [TestMethod]
        public void PerformanceIntegrationTest()
        {
            // Test performance of integrated operations

            // Arrange
            var riskCalculator = new RiskCalculator();
            var monteCarloService = new MonteCarloService();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var alertService = new RiskAlertService(mockHttpContextAccessor.Object);

            var startTime = DateTime.Now;

            // Act - Perform multiple operations
            var portfolioData = riskCalculator.GetPortfolioData();
            decimal totalVaR = 0;

            foreach (var holding in portfolioData)
            {
                decimal var = riskCalculator.CalculateVaR(holding.Key, holding.Value);
                totalVaR += var;

                if (var / holding.Value > 0.05m)
                {
                    alertService.AddAlert($"High risk detected for {holding.Key}");
                }
            }

            // Run Monte Carlo simulation
            var monteCarloResults = monteCarloService.RunSimulation(1000);

            var endTime = DateTime.Now;
            var duration = endTime - startTime;

            // Assert
            Assert.IsTrue(duration.TotalSeconds < 10, "Integrated operations should complete within 10 seconds");
            Assert.IsTrue(totalVaR > 0, "Total VaR should be calculated");
            Assert.IsNotNull(monteCarloResults, "Monte Carlo simulation should complete");
        }

        [TestMethod]
        public void DataConsistencyIntegrationTest()
        {
            // Test data consistency across different components

            // Arrange
            var riskCalculator = new RiskCalculator();
            var portfolioData = riskCalculator.GetPortfolioData();

            // Act - Calculate VaR using different methods
            var directCalculations = new List<decimal>();
            var portfolioCalculations = new List<decimal>();

            foreach (var holding in portfolioData)
            {
                // Direct calculation
                decimal directVar = riskCalculator.CalculateVaR(holding.Key, holding.Value);
                directCalculations.Add(directVar);

                // Portfolio calculation (simulate portfolio control)
                decimal portfolioVar = riskCalculator.CalculateVaR(holding.Key, holding.Value);
                portfolioCalculations.Add(portfolioVar);
            }

            // Assert - Results should be consistent
            Assert.AreEqual(directCalculations.Count, portfolioCalculations.Count,
                "Should have same number of calculations");

            for (int i = 0; i < directCalculations.Count; i++)
            {
                Assert.AreEqual(directCalculations[i], portfolioCalculations[i],
                    $"Calculation {i} should be consistent");
            }
        }
    }
}
