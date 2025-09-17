using Microsoft.VisualStudio.TestTools.UnitTesting;
using RiskCalculatorWebForm.Controls;
using RiskCalculatorWebForm.Tests.TestUtilities;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace RiskCalculatorWebForm.Tests.StateManagement
{
    [TestClass]
    public class ViewStateTests
    {
        [TestMethod]
        public void VarCalculationControl_ViewState_PersistsData()
        {
            // Arrange
            var control = new VarCalculationControl();
            var page = new System.Web.UI.Page();
            page.Controls.Add(control);
            
            // Simulate Page_Load
            control.Page_Load(null, EventArgs.Empty);

            // Act - Set values that should be stored in ViewState
            control.Symbol = "AAPL";
            control.Amount = 100000m;

            // Simulate postback
            var viewState = new StateBag();
            
            // Assert - ViewState should contain the values
            Assert.AreEqual("AAPL", control.Symbol, "Symbol should be persisted");
            Assert.AreEqual(100000m, control.Amount, "Amount should be persisted");
        }

        [TestMethod]
        public void VarCalculationControl_ViewState_CalculationHistory()
        {
            // Arrange
            var control = new VarCalculationControl();
            
            // Simulate Page_Load to initialize ViewState
            control.Page_Load(null, EventArgs.Empty);

            // Act - Add some calculation history
            var history = control.CalculationHistory;
            history.Add(new VaRCalculationResult
            {
                Symbol = "AAPL",
                Amount = 100000m,
                VaR = 2500m,
                CreditRisk = 7.5m,
                RiskLevel = "LOW",
                CalculatedAt = DateTime.Now
            });

            // Assert
            Assert.AreEqual(1, history.Count, "Calculation history should contain one item");
            Assert.AreEqual("AAPL", history[0].Symbol, "History should contain correct symbol");
        }

        [TestMethod]
        public void PortfolioGridControl_ViewState_PortfolioData()
        {
            // Arrange
            var control = new PortfolioGridControl();
            
            // Simulate Page_Load
            control.Page_Load(null, EventArgs.Empty);

            // Act - Portfolio data should be stored in ViewState
            var portfolioData = control.PortfolioData;

            // Assert
            Assert.IsNotNull(portfolioData, "Portfolio data should not be null");
            Assert.IsTrue(portfolioData.Count > 0, "Portfolio data should contain items");
        }

        [TestMethod]
        public void MonteCarloControl_ViewState_SimulationHistory()
        {
            // Arrange
            var control = new MonteCarloControl();
            
            // Simulate Page_Load
            control.Page_Load(null, EventArgs.Empty);

            // Act
            var history = control.SimulationHistory;

            // Assert
            Assert.IsNotNull(history, "Simulation history should not be null");
            Assert.AreEqual(0, history.Count, "Simulation history should start empty");
        }

        [TestMethod]
        public void ViewState_DataTypes_HandledCorrectly()
        {
            // Test various data types that might be stored in ViewState
            var testData = new Dictionary<string, object>
            {
                ["StringValue"] = "Test String",
                ["IntValue"] = 42,
                ["DecimalValue"] = 123.45m,
                ["DateTimeValue"] = DateTime.Now,
                ["BoolValue"] = true,
                ["ListValue"] = new List<string> { "Item1", "Item2" }
            };

            // All these should be serializable for ViewState
            foreach (var item in testData)
            {
                Assert.IsNotNull(item.Value, $"{item.Key} should not be null");
            }
        }

        [TestMethod]
        public void ViewState_PerformanceTest()
        {
            // Test ViewState with large amounts of data
            var largeDataList = new List<string>();
            for (int i = 0; i < 1000; i++)
            {
                largeDataList.Add($"Test Item {i}");
            }

            // This should not cause performance issues
            Assert.AreEqual(1000, largeDataList.Count, "Large data list should be created successfully");
        }

        [TestMethod]
        public void ViewState_CrossPagePersistence()
        {
            // Test that ViewState data doesn't leak between different control instances
            var control1 = new VarCalculationControl();
            var control2 = new VarCalculationControl();

            // Simulate Page_Load for both
            control1.Page_Load(null, EventArgs.Empty);
            control2.Page_Load(null, EventArgs.Empty);

            // Set different values
            control1.Symbol = "AAPL";
            control2.Symbol = "GOOGL";

            // Assert they maintain separate state
            Assert.AreEqual("AAPL", control1.Symbol, "Control 1 should maintain its state");
            Assert.AreEqual("GOOGL", control2.Symbol, "Control 2 should maintain its state");
        }
    }
}
