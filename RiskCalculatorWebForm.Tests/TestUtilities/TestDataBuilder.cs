using System;
using System.Collections.Generic;
using System.Data;

namespace RiskCalculatorWebForm.Tests.TestUtilities
{
    /// <summary>
    /// Builder pattern class for creating test data
    /// </summary>
    public class TestDataBuilder
    {
        public static Dictionary<string, decimal> CreateSamplePortfolioData()
        {
            return new Dictionary<string, decimal>
            {
                { "AAPL", 150000.00m },
                { "GOOGL", 200000.00m },
                { "MSFT", 175000.00m },
                { "AMZN", 180000.00m },
                { "TSLA", 120000.00m }
            };
        }

        public static Dictionary<string, decimal> CreateSmallPortfolioData()
        {
            return new Dictionary<string, decimal>
            {
                { "AAPL", 10000.00m },
                { "MSFT", 15000.00m }
            };
        }

        public static Dictionary<string, decimal> CreateVolatilityData()
        {
            return new Dictionary<string, decimal>
            {
                { "AAPL", 0.025m },
                { "GOOGL", 0.030m },
                { "MSFT", 0.022m },
                { "AMZN", 0.035m },
                { "TSLA", 0.055m }
            };
        }

        public static DataTable CreatePortfolioDataTable()
        {
            var dt = new DataTable();
            dt.Columns.Add("Symbol", typeof(string));
            dt.Columns.Add("Value", typeof(decimal));
            dt.Columns.Add("DailyVaR", typeof(decimal));
            dt.Columns.Add("RiskPercentage", typeof(decimal));
            dt.Columns.Add("RiskLevel", typeof(string));

            dt.Rows.Add("AAPL", 150000.00m, 3750.00m, 2.50m, "LOW");
            dt.Rows.Add("GOOGL", 200000.00m, 6000.00m, 3.00m, "LOW");
            dt.Rows.Add("MSFT", 175000.00m, 3850.00m, 2.20m, "LOW");
            dt.Rows.Add("AMZN", 180000.00m, 6300.00m, 3.50m, "LOW");
            dt.Rows.Add("TSLA", 120000.00m, 6600.00m, 5.50m, "HIGH");

            return dt;
        }

        public static List<string> CreateSampleRiskAlerts()
        {
            return new List<string>
            {
                "HIGH RISK: TSLA VaR exceeds 5% threshold at 2024-01-15 10:30:00",
                "MEDIUM RISK: AMZN volatility increased at 2024-01-15 11:15:00"
            };
        }

        public static List<VaRCalculationResult> CreateSampleVaRResults()
        {
            return new List<VaRCalculationResult>
            {
                new VaRCalculationResult
                {
                    Symbol = "AAPL",
                    Amount = 100000m,
                    VaR = 2500m,
                    CreditRisk = 7.5m,
                    RiskLevel = "LOW",
                    CalculatedAt = DateTime.Now.AddMinutes(-30)
                },
                new VaRCalculationResult
                {
                    Symbol = "TSLA",
                    Amount = 50000m,
                    VaR = 2750m,
                    CreditRisk = 6.2m,
                    RiskLevel = "HIGH",
                    CalculatedAt = DateTime.Now.AddMinutes(-15)
                }
            };
        }

        public static List<MonteCarloSimulationResult> CreateSampleMonteCarloResults()
        {
            return new List<MonteCarloSimulationResult>
            {
                new MonteCarloSimulationResult
                {
                    SimulationCount = 1000,
                    VaR95 = -0.025m,
                    VaR99 = -0.035m,
                    ExpectedReturn = 0.001m,
                    ExecutionTimeMs = 150,
                    Timestamp = DateTime.Now.AddMinutes(-20)
                },
                new MonteCarloSimulationResult
                {
                    SimulationCount = 5000,
                    VaR95 = -0.024m,
                    VaR99 = -0.034m,
                    ExpectedReturn = 0.0012m,
                    ExecutionTimeMs = 750,
                    Timestamp = DateTime.Now.AddMinutes(-10)
                }
            };
        }
    }

    // Test data classes (copied from main project for testing)
    public class VaRCalculationResult
    {
        public string Symbol { get; set; }
        public decimal Amount { get; set; }
        public decimal VaR { get; set; }
        public decimal CreditRisk { get; set; }
        public string RiskLevel { get; set; }
        public DateTime CalculatedAt { get; set; }
    }

    public class MonteCarloSimulationResult
    {
        public int SimulationCount { get; set; }
        public decimal VaR95 { get; set; }
        public decimal VaR99 { get; set; }
        public decimal ExpectedReturn { get; set; }
        public long ExecutionTimeMs { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
