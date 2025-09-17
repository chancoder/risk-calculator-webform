using Microsoft.VisualStudio.TestTools.UnitTesting;
using RiskCalculatorWebForm;
using RiskCalculatorWebForm.Tests.TestUtilities;
using System;
using System.Collections.Generic;

namespace RiskCalculatorWebForm.Tests.BusinessLogic
{
    [TestClass]
    public class RiskCalculatorTests
    {
        private RiskCalculator _riskCalculator;

        [TestInitialize]
        public void Setup()
        {
            _riskCalculator = new RiskCalculator();
        }

        [TestMethod]
        public void CalculateVaR_ValidInput_ReturnsCorrectValue()
        {
            // Arrange
            string symbol = "AAPL";
            decimal amount = 100000m;
            decimal expectedVaR = amount * 0.025m * 1.645m; // Expected calculation

            // Act
            decimal actualVaR = _riskCalculator.CalculateVaR(symbol, amount);

            // Assert
            Assert.AreEqual(expectedVaR, actualVaR, 0.01m, "VaR calculation should match expected formula");
        }

        [TestMethod]
        public void CalculateVaR_DifferentSymbols_ReturnsDifferentValues()
        {
            // Arrange
            decimal amount = 100000m;
            string symbol1 = "AAPL"; // 2.5% volatility
            string symbol2 = "TSLA"; // 5.5% volatility

            // Act
            decimal var1 = _riskCalculator.CalculateVaR(symbol1, amount);
            decimal var2 = _riskCalculator.CalculateVaR(symbol2, amount);

            // Assert
            Assert.IsTrue(var2 > var1, "TSLA should have higher VaR than AAPL due to higher volatility");
        }

        [TestMethod]
        public void CalculateVaR_ZeroAmount_ReturnsZero()
        {
            // Arrange
            string symbol = "AAPL";
            decimal amount = 0m;

            // Act
            decimal actualVaR = _riskCalculator.CalculateVaR(symbol, amount);

            // Assert
            Assert.AreEqual(0m, actualVaR, "VaR should be zero for zero amount");
        }

        [TestMethod]
        public void GetHistoricalVolatility_KnownSymbol_ReturnsCorrectValue()
        {
            // Arrange
            string symbol = "AAPL";

            // Act
            decimal volatility = _riskCalculator.GetHistoricalVolatility(symbol);

            // Assert
            Assert.AreEqual(0.025m, volatility, "AAPL volatility should be 2.5%");
        }

        [TestMethod]
        public void GetHistoricalVolatility_UnknownSymbol_ReturnsDefaultValue()
        {
            // Arrange
            string symbol = "UNKNOWN";

            // Act
            decimal volatility = _riskCalculator.GetHistoricalVolatility(symbol);

            // Assert
            Assert.AreEqual(0.025m, volatility, "Unknown symbol should return default volatility");
        }

        [TestMethod]
        public void CalculateCreditRisk_ValidSymbol_ReturnsValidScore()
        {
            // Arrange
            string symbol = "AAPL";

            // Act
            decimal creditRisk = _riskCalculator.CalculateCreditRisk(symbol);

            // Assert
            Assert.IsTrue(creditRisk >= 1.0m && creditRisk <= 10.0m, 
                "Credit risk score should be between 1 and 10");
        }

        [TestMethod]
        public void CalculateCreditRisk_SameSymbol_ReturnsConsistentScore()
        {
            // Arrange
            string symbol = "AAPL";

            // Act
            decimal creditRisk1 = _riskCalculator.CalculateCreditRisk(symbol);
            decimal creditRisk2 = _riskCalculator.CalculateCreditRisk(symbol);

            // Assert
            Assert.AreEqual(creditRisk1, creditRisk2, 
                "Credit risk score should be consistent for the same symbol");
        }

        [TestMethod]
        public void GetRiskLevel_LowRisk_ReturnsLow()
        {
            // Arrange
            decimal riskRatio = 0.01m; // 1%

            // Act
            string riskLevel = _riskCalculator.GetRiskLevel(riskRatio);

            // Assert
            Assert.AreEqual("LOW", riskLevel, "1% risk ratio should be classified as LOW");
        }

        [TestMethod]
        public void GetRiskLevel_MediumRisk_ReturnsMedium()
        {
            // Arrange
            decimal riskRatio = 0.03m; // 3%

            // Act
            string riskLevel = _riskCalculator.GetRiskLevel(riskRatio);

            // Assert
            Assert.AreEqual("MEDIUM", riskLevel, "3% risk ratio should be classified as MEDIUM");
        }

        [TestMethod]
        public void GetRiskLevel_HighRisk_ReturnsHigh()
        {
            // Arrange
            decimal riskRatio = 0.06m; // 6%

            // Act
            string riskLevel = _riskCalculator.GetRiskLevel(riskRatio);

            // Assert
            Assert.AreEqual("HIGH", riskLevel, "6% risk ratio should be classified as HIGH");
        }

        [TestMethod]
        public void GetRiskLevel_BoundaryValues_ReturnsCorrectLevels()
        {
            // Test boundary conditions
            Assert.AreEqual("LOW", _riskCalculator.GetRiskLevel(0.019m), "Just under 2% should be LOW");
            Assert.AreEqual("MEDIUM", _riskCalculator.GetRiskLevel(0.02m), "Exactly 2% should be MEDIUM");
            Assert.AreEqual("MEDIUM", _riskCalculator.GetRiskLevel(0.049m), "Just under 5% should be MEDIUM");
            Assert.AreEqual("HIGH", _riskCalculator.GetRiskLevel(0.05m), "Exactly 5% should be HIGH");
        }

        [TestMethod]
        public void GetPortfolioData_ReturnsValidData()
        {
            // Act
            var portfolioData = _riskCalculator.GetPortfolioData();

            // Assert
            Assert.IsNotNull(portfolioData, "Portfolio data should not be null");
            Assert.IsTrue(portfolioData.Count > 0, "Portfolio should contain data");
            Assert.IsTrue(portfolioData.ContainsKey("AAPL"), "Portfolio should contain AAPL");
            Assert.IsTrue(portfolioData.ContainsKey("GOOGL"), "Portfolio should contain GOOGL");
        }

        [TestMethod]
        public void GetPortfolioData_ReturnsExpectedValues()
        {
            // Arrange
            var expectedData = TestDataBuilder.CreateSamplePortfolioData();

            // Act
            var actualData = _riskCalculator.GetPortfolioData();

            // Assert
            foreach (var expectedItem in expectedData)
            {
                Assert.IsTrue(actualData.ContainsKey(expectedItem.Key), 
                    $"Portfolio should contain {expectedItem.Key}");
                Assert.AreEqual(expectedItem.Value, actualData[expectedItem.Key], 
                    $"{expectedItem.Key} value should match expected");
            }
        }

        [TestMethod]
        public void CalculateVaR_MultipleSymbols_AllReturnValidValues()
        {
            // Arrange
            var portfolioData = _riskCalculator.GetPortfolioData();
            decimal testAmount = 100000m;

            // Act & Assert
            foreach (var item in portfolioData)
            {
                decimal var = _riskCalculator.CalculateVaR(item.Key, testAmount);
                Assert.IsTrue(var > 0, $"{item.Key} VaR should be positive");
                Assert.IsTrue(var < testAmount, $"{item.Key} VaR should be less than investment amount");
            }
        }

        [TestMethod]
        public void RiskCalculation_EndToEndTest()
        {
            // Arrange
            string symbol = "AAPL";
            decimal amount = 100000m;

            // Act
            decimal var = _riskCalculator.CalculateVaR(symbol, amount);
            decimal creditRisk = _riskCalculator.CalculateCreditRisk(symbol);
            string riskLevel = _riskCalculator.GetRiskLevel(var / amount);

            // Assert
            Assert.IsTrue(var > 0, "VaR should be positive");
            Assert.IsTrue(creditRisk >= 1.0m && creditRisk <= 10.0m, "Credit risk should be valid");
            Assert.IsTrue(riskLevel == "LOW" || riskLevel == "MEDIUM" || riskLevel == "HIGH", 
                "Risk level should be valid");
        }
    }
}
