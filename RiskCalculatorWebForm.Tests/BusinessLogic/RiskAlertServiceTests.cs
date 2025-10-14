using Microsoft.VisualStudio.TestTools.UnitTesting;
using risk_calculator_webapp;
using RiskCalculatorWebForm.Tests.TestUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Moq;


namespace RiskCalculatorWebForm.Tests.BusinessLogic
{
    [TestClass]
    public class RiskAlertServiceTests
    {
        private RiskAlertService _alertService;
        private HttpContext _mockHttpContext;

        [TestInitialize]
        public void Setup()
        {
            _mockHttpContext = MockHttpContext.CreateMockHttpContext();

            // Create mock IHttpContextAccessor for dependency injection
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(_mockHttpContext);

            _alertService = new RiskAlertService(mockHttpContextAccessor.Object);

            // Mock HTTP context is already created and available for testing
// HttpContext.Current is not available in .NET 8 - using mock context instead
        }

        [TestMethod]
        public void AddAlert_ValidAlert_AddsToAlerts()
        {
            // Arrange
            string alertMessage = "Test alert message";

            // Act
            _alertService.AddAlert(alertMessage);
            var alerts = _alertService.GetAlerts();

            // Assert
            Assert.IsTrue(alerts.Count > 0, "Alerts should contain at least one item");
            Assert.IsTrue(alerts.Contains(alertMessage), "Alerts should contain the added message");
        }

        [TestMethod]
        public void AddAlert_MultipleAlerts_AllAdded()
        {
            // Arrange
            var alertMessages = new List<string>
            {
                "Alert 1",
                "Alert 2",
                "Alert 3"
            };

            // Act
            foreach (var message in alertMessages)
            {
                _alertService.AddAlert(message);
            }
            var alerts = _alertService.GetAlerts();

            // Assert
            Assert.AreEqual(alertMessages.Count, alerts.Count, "Should have all alerts");
            foreach (var message in alertMessages)
            {
                Assert.IsTrue(alerts.Contains(message), $"Should contain alert: {message}");
            }
        }

        [TestMethod]
        public void GetAlerts_EmptySession_ReturnsEmptyList()
        {
            // Act
            var alerts = _alertService.GetAlerts();

            // Assert
            Assert.IsNotNull(alerts, "Alerts should not be null");
            Assert.AreEqual(0, alerts.Count, "Should return empty list when no alerts");
        }

        [TestMethod]
        public void ClearAlerts_WithExistingAlerts_ClearsAllAlerts()
        {
            // Arrange
            _alertService.AddAlert("Test alert 1");
            _alertService.AddAlert("Test alert 2");

            // Act
            _alertService.ClearAlerts();
            var alerts = _alertService.GetAlerts();

            // Assert
            Assert.AreEqual(0, alerts.Count, "Should clear all alerts");
        }

        [TestMethod]
        public void GetAlerts_AfterAddingAndClearing_ReturnsEmptyList()
        {
            // Arrange
            _alertService.AddAlert("Test alert");
            _alertService.ClearAlerts();

            // Act
            var alerts = _alertService.GetAlerts();

            // Assert
            Assert.AreEqual(0, alerts.Count, "Should return empty list after clearing");
        }

        [TestMethod]
        public void AddAlert_SpecialCharacters_HandlesCorrectly()
        {
            // Arrange
            string specialAlert = "Alert with special chars: !@#$%^&*()_+-=[]{}|;':\",./<>?";

            // Act
            _alertService.AddAlert(specialAlert);
            var alerts = _alertService.GetAlerts();

            // Assert
            Assert.IsTrue(alerts.Contains(specialAlert), "Should handle special characters");
        }

        [TestMethod]
        public void AddAlert_EmptyMessage_HandlesCorrectly()
        {
            // Arrange
            string emptyMessage = "";

            // Act
            _alertService.AddAlert(emptyMessage);
            var alerts = _alertService.GetAlerts();

            // Assert
            Assert.IsTrue(alerts.Contains(emptyMessage), "Should handle empty messages");
        }

        [TestMethod]
        public void AddAlert_NullMessage_HandlesCorrectly()
        {
            // Arrange
            string nullMessage = null;

            // Act
            _alertService.AddAlert(nullMessage);
            var alerts = _alertService.GetAlerts();

            // Assert
            Assert.IsTrue(alerts.Contains(nullMessage), "Should handle null messages");
        }

        [TestMethod]
        public void AddAlert_LongMessage_HandlesCorrectly()
        {
            // Arrange
            string longMessage = new string('A', 1000); // 1000 character message

            // Act
            _alertService.AddAlert(longMessage);
            var alerts = _alertService.GetAlerts();

            // Assert
            Assert.IsTrue(alerts.Contains(longMessage), "Should handle long messages");
        }

        [TestMethod]
        public void GetAlerts_MultipleOperations_MaintainsOrder()
        {
            // Arrange
            var expectedOrder = new List<string>
            {
                "First alert",
                "Second alert",
                "Third alert"
            };

            // Act
            foreach (var alert in expectedOrder)
            {
                _alertService.AddAlert(alert);
            }
            var actualAlerts = _alertService.GetAlerts();

            // Assert
            Assert.AreEqual(expectedOrder.Count, actualAlerts.Count, "Should maintain count");

            // Check that alerts are in the order they were added (FIFO)
            for (int i = 0; i < expectedOrder.Count; i++)
            {
                Assert.AreEqual(expectedOrder[i], actualAlerts[i],
                    $"Alert at position {i} should match expected order");
            }
        }

        [TestMethod]
        public void AlertService_IntegrationTest()
        {
            // Test complete workflow
            // 1. Start with empty alerts
            Assert.AreEqual(0, _alertService.GetAlerts().Count, "Should start empty");

            // 2. Add some alerts
            _alertService.AddAlert("High risk detected");
            _alertService.AddAlert("Portfolio rebalanced");
            _alertService.AddAlert("Market volatility increased");

            // 3. Verify alerts were added
            var alerts = _alertService.GetAlerts();
            Assert.AreEqual(3, alerts.Count, "Should have 3 alerts");
            Assert.IsTrue(alerts.Contains("High risk detected"), "Should contain first alert");
            Assert.IsTrue(alerts.Contains("Portfolio rebalanced"), "Should contain second alert");
            Assert.IsTrue(alerts.Contains("Market volatility increased"), "Should contain third alert");

            // 4. Clear alerts
            _alertService.ClearAlerts();
            Assert.AreEqual(0, _alertService.GetAlerts().Count, "Should be empty after clearing");

            // 5. Add new alerts
            _alertService.AddAlert("New alert after clearing");
            Assert.AreEqual(1, _alertService.GetAlerts().Count, "Should have one new alert");
        }
    }
}
