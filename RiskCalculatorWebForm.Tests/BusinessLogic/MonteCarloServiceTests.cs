using Microsoft.VisualStudio.TestTools.UnitTesting;
using risk_calculator_webapp;
using RiskCalculatorWebForm.Tests.TestUtilities;
using System;
using System.Diagnostics;
using System.Linq;

namespace RiskCalculatorWebForm.Tests.BusinessLogic
{
    [TestClass]
    public class MonteCarloServiceTests
    {
        private MonteCarloService _monteCarloService;

        [TestInitialize]
        public void Setup()
        {
            _monteCarloService = new MonteCarloService();
        }

        [TestMethod]
        public void RunSimulation_ValidInput_ReturnsValidResult()
        {
            // Arrange
            int simulations = 100;

            // Act
            var result = _monteCarloService.RunSimulation(simulations);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsTrue(result.VaR95 < 0, "95% VaR should be negative (loss)");
            Assert.IsTrue(result.VaR99 < result.VaR95, "99% VaR should be more negative than 95% VaR");
        }

        [TestMethod]
        public void RunSimulation_MultipleRuns_ReturnsConsistentResults()
        {
            // Arrange
            int simulations = 1000;

            // Act
            var result1 = _monteCarloService.RunSimulation(simulations);
            var result2 = _monteCarloService.RunSimulation(simulations);

            // Assert
            Assert.IsNotNull(result1, "First result should not be null");
            Assert.IsNotNull(result2, "Second result should not be null");
            
            // Results should be in reasonable ranges (not identical due to randomness)
            Assert.IsTrue(Math.Abs(result1.VaR95 - result2.VaR95) < 0.1m, 
                "VaR results should be within reasonable range");
        }

        [TestMethod]
        public void RunSimulation_DifferentSimulationCounts_ScalesAppropriately()
        {
            // Arrange
            int smallSimulations = 100;
            int largeSimulations = 1000;

            // Act
            var smallResult = _monteCarloService.RunSimulation(smallSimulations);
            var largeResult = _monteCarloService.RunSimulation(largeSimulations);

            // Assert
            Assert.IsNotNull(smallResult, "Small simulation result should not be null");
            Assert.IsNotNull(largeResult, "Large simulation result should not be null");
            
            // Both should be valid negative values
            Assert.IsTrue(smallResult.VaR95 < 0, "Small simulation VaR should be negative");
            Assert.IsTrue(largeResult.VaR95 < 0, "Large simulation VaR should be negative");
        }

        [TestMethod]
        public void RunSimulation_PerformanceTest()
        {
            // Arrange
            int simulations = 1000;
            var stopwatch = Stopwatch.StartNew();

            // Act
            var result = _monteCarloService.RunSimulation(simulations);
            stopwatch.Stop();

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 5000, 
                "Simulation should complete within 5 seconds");
        }

        [TestMethod]
        public void RunSimulation_ExpectedReturn_IsReasonable()
        {
            // Arrange
            int simulations = 10000; // Larger sample for better statistics

            // Act
            var result = _monteCarloService.RunSimulation(simulations);

            // Assert
            Assert.IsTrue(result.ExpectedReturn > -0.1m && result.ExpectedReturn < 0.1m, 
                "Expected return should be reasonable (between -10% and +10%)");
        }

        [TestMethod]
        public void RunSimulation_VaRPercentiles_AreOrderedCorrectly()
        {
            // Arrange
            int simulations = 1000;

            // Act
            var result = _monteCarloService.RunSimulation(simulations);

            // Assert
            Assert.IsTrue(result.VaR95 > result.VaR99, 
                "95% VaR should be less negative than 99% VaR");
        }

        [TestMethod]
        public void RunSimulation_EdgeCases()
        {
            // Test with minimum reasonable number of simulations
            var result = _monteCarloService.RunSimulation(100);
            Assert.IsNotNull(result, "Minimum simulation count should work");
        }

        [TestMethod]
        public void RunSimulation_StatisticalProperties()
        {
            // Arrange
            int simulations = 5000;
            int numberOfRuns = 10;
            var results = new MonteCarloResult[numberOfRuns];

            // Act
            for (int i = 0; i < numberOfRuns; i++)
            {
                results[i] = _monteCarloService.RunSimulation(simulations);
            }

            // Assert
            var avgVaR95 = results.Average(r => r.VaR95);
            var avgVaR99 = results.Average(r => r.VaR99);
            var avgExpectedReturn = results.Average(r => r.ExpectedReturn);

            Assert.IsTrue(avgVaR95 < 0, "Average 95% VaR should be negative");
            Assert.IsTrue(avgVaR99 < avgVaR95, "Average 99% VaR should be more negative than 95% VaR");
            Assert.IsTrue(avgExpectedReturn > -0.05m && avgExpectedReturn < 0.05m, 
                "Average expected return should be reasonable");
        }

        [TestMethod]
        public void RunSimulation_ConsistencyTest()
        {
            // Arrange
            int simulations = 1000;

            // Act
            var results = new MonteCarloResult[5];
            for (int i = 0; i < 5; i++)
            {
                results[i] = _monteCarloService.RunSimulation(simulations);
            }

            // Assert - all results should be valid and in reasonable ranges
            foreach (var result in results)
            {
                Assert.IsNotNull(result, "Each result should not be null");
                Assert.IsTrue(result.VaR95 < 0, "VaR95 should be negative");
                Assert.IsTrue(result.VaR99 < result.VaR95, "VaR99 should be more negative than VaR95");
                Assert.IsTrue(result.ExpectedReturn > -0.1m && result.ExpectedReturn < 0.1m, 
                    "Expected return should be in reasonable range");
            }
        }
    }
}
