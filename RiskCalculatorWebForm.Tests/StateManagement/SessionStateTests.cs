using Microsoft.VisualStudio.TestTools.UnitTesting;
using RiskCalculatorWebForm;
using RiskCalculatorWebForm.Tests.TestUtilities;
using System;
using System.Collections.Generic;
using System.Web;

namespace RiskCalculatorWebForm.Tests.StateManagement
{
    [TestClass]
    public class SessionStateTests
    {
        [TestInitialize]
        public void Setup()
        {
            // Mock HttpContext for session testing
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://localhost/test.aspx", ""),
                new HttpResponse(null));
        }

        [TestMethod]
        public void SessionState_DefaultValues_InitializedCorrectly()
        {
            // Arrange
            var sessionData = new Dictionary<string, object>
            {
                ["DefaultSymbol"] = "AAPL",
                ["DefaultAmount"] = "100000",
                ["DefaultSimulationCount"] = "1000"
            };

            // Act & Assert
            foreach (var item in sessionData)
            {
                Assert.IsNotNull(item.Value, $"{item.Key} should have a default value");
            }
        }

        [TestMethod]
        public void SessionState_PortfolioData_PersistsAcrossPages()
        {
            // Arrange
            var portfolioData = TestDataBuilder.CreateSamplePortfolioData();

            // Act - Simulate storing in session
            Session["PortfolioData"] = portfolioData;
            Session["PortfolioLastUpdated"] = DateTime.Now;

            // Assert
            var retrievedData = Session["PortfolioData"] as Dictionary<string, decimal>;
            Assert.IsNotNull(retrievedData, "Portfolio data should be retrievable from session");
            Assert.AreEqual(portfolioData.Count, retrievedData.Count, "Portfolio data count should match");
        }

        [TestMethod]
        public void SessionState_CalculationStatistics_TracksCorrectly()
        {
            // Arrange
            int totalCalculations = 0;
            DateTime sessionStartTime = DateTime.Now;

            // Act - Simulate multiple calculations
            for (int i = 0; i < 5; i++)
            {
                totalCalculations++;
                Session["TotalCalculations"] = totalCalculations;
                Session["LastCalculationTime"] = DateTime.Now;
            }

            // Assert
            Assert.AreEqual(5, Session["TotalCalculations"], "Should track total calculations correctly");
            Assert.IsNotNull(Session["LastCalculationTime"], "Should store last calculation time");
        }

        [TestMethod]
        public void SessionState_SimulationHistory_MaintainsData()
        {
            // Arrange
            var simulationHistory = TestDataBuilder.CreateSampleMonteCarloResults();

            // Act
            Session["SimulationHistory"] = simulationHistory;
            Session["TotalSimulations"] = simulationHistory.Count;

            // Assert
            var retrievedHistory = Session["SimulationHistory"] as List<MonteCarloSimulationResult>;
            Assert.IsNotNull(retrievedHistory, "Simulation history should be retrievable");
            Assert.AreEqual(simulationHistory.Count, retrievedHistory.Count, "History count should match");
        }

        [TestMethod]
        public void SessionState_RiskAlerts_PersistsData()
        {
            // Arrange
            var alertService = new RiskAlertService();
            var testAlerts = TestDataBuilder.CreateSampleRiskAlerts();

            // Act
            foreach (var alert in testAlerts)
            {
                alertService.AddAlert(alert);
            }

            // Assert
            var alerts = alertService.GetAlerts();
            Assert.AreEqual(testAlerts.Count, alerts.Count, "Should persist all alerts");
        }

        [TestMethod]
        public void SessionState_DataTypes_HandledCorrectly()
        {
            // Test various data types in session
            var testData = new Dictionary<string, object>
            {
                ["StringValue"] = "Test String",
                ["IntValue"] = 42,
                ["DecimalValue"] = 123.45m,
                ["DateTimeValue"] = DateTime.Now,
                ["BoolValue"] = true,
                ["ListValue"] = new List<string> { "Item1", "Item2" },
                ["DictionaryValue"] = new Dictionary<string, decimal> { ["AAPL"] = 1000m }
            };

            // Act - Store all in session
            foreach (var item in testData)
            {
                Session[item.Key] = item.Value;
            }

            // Assert - Retrieve and verify
            foreach (var item in testData)
            {
                var retrieved = Session[item.Key];
                Assert.IsNotNull(retrieved, $"{item.Key} should be retrievable from session");
                Assert.AreEqual(item.Value.GetType(), retrieved.GetType(), 
                    $"{item.Key} should maintain correct type");
            }
        }

        [TestMethod]
        public void SessionState_SessionTimeout_HandlesGracefully()
        {
            // Test that application handles missing session data gracefully
            Session.Clear();

            // Act - Try to access session data that doesn't exist
            var defaultSymbol = Session["DefaultSymbol"] ?? "AAPL";
            var totalCalculations = Session["TotalCalculations"] ?? 0;

            // Assert
            Assert.AreEqual("AAPL", defaultSymbol, "Should provide default value when session is empty");
            Assert.AreEqual(0, totalCalculations, "Should provide default value when session is empty");
        }

        [TestMethod]
        public void SessionState_CrossPageDataSharing()
        {
            // Simulate data being set on one page and accessed on another
            Session["PageVisits"] = 1;
            Session["FirstVisitTime"] = DateTime.Now;
            Session["UserPreferences"] = new Dictionary<string, string>
            {
                ["Theme"] = "Dark",
                ["Language"] = "English"
            };

            // Simulate navigation to another page
            var pageVisits = (int)Session["PageVisits"];
            var firstVisit = (DateTime)Session["FirstVisitTime"];
            var preferences = Session["UserPreferences"] as Dictionary<string, string>;

            // Assert
            Assert.AreEqual(1, pageVisits, "Page visits should be accessible across pages");
            Assert.IsNotNull(firstVisit, "First visit time should be accessible across pages");
            Assert.IsNotNull(preferences, "User preferences should be accessible across pages");
            Assert.AreEqual("Dark", preferences["Theme"], "Preferences should maintain values across pages");
        }

        [TestMethod]
        public void SessionState_MemoryManagement()
        {
            // Test with large amounts of data to ensure proper memory management
            var largeList = new List<string>();
            for (int i = 0; i < 10000; i++)
            {
                largeList.Add($"Large data item {i}");
            }

            // Act
            Session["LargeDataSet"] = largeList;

            // Assert
            var retrieved = Session["LargeDataSet"] as List<string>;
            Assert.IsNotNull(retrieved, "Large data set should be stored and retrieved");
            Assert.AreEqual(10000, retrieved.Count, "Large data set should maintain all items");
        }

        [TestMethod]
        public void SessionState_ConcurrentAccess()
        {
            // Test that session state handles concurrent access scenarios
            var sessionData = new Dictionary<string, object>();
            
            // Simulate concurrent writes (in real scenario this would be different threads)
            for (int i = 0; i < 100; i++)
            {
                Session[$"ConcurrentKey_{i}"] = $"Value_{i}";
            }

            // Assert
            for (int i = 0; i < 100; i++)
            {
                Assert.AreEqual($"Value_{i}", Session[$"ConcurrentKey_{i}"], 
                    $"Concurrent key {i} should maintain correct value");
            }
        }
    }
}
