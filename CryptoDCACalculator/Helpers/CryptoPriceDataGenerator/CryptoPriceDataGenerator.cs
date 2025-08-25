using CryptoDCACalculator.EF.Entities;
using CryptoDCACalculator.Models;

namespace CryptoDCACalculator.Helpers.CryptoPriceDataGenerator;

public static class CryptoPriceDataGenerator
{
    // Historical base prices and key milestones for realistic price generation
    private static readonly Dictionary<string, CryptoHistoricalData> CryptoData = new()
    {
        ["BTC"] = new CryptoHistoricalData
        {
            StartPrice = 7200m, // Jan 2020
            KeyPrices = new Dictionary<DateTime, decimal>
            {
                [new DateTime(2020, 1, 1)] = 7200m,
                [new DateTime(2020, 12, 31)] = 28900m,
                [new DateTime(2021, 4, 14)] = 64500m, // ATH 2021
                [new DateTime(2021, 12, 31)] = 46200m,
                [new DateTime(2022, 6, 18)] = 17800m, // 2022 low
                [new DateTime(2022, 12, 31)] = 16500m,
                [new DateTime(2023, 12, 31)] = 42300m,
                [new DateTime(2024, 3, 14)] = 73700m, // ATH 2024
                [new DateTime(2024, 12, 31)] = 95000m,
                [new DateTime(2025, 8, 22)] = 61500m
            },
            Volatility = 0.045 // 4.5% daily volatility
        },
        ["ETH"] = new CryptoHistoricalData
        {
            StartPrice = 130m,
            KeyPrices = new Dictionary<DateTime, decimal>
            {
                [new DateTime(2020, 1, 1)] = 130m,
                [new DateTime(2020, 12, 31)] = 730m,
                [new DateTime(2021, 5, 12)] = 4350m, // ATH 2021
                [new DateTime(2021, 12, 31)] = 3680m,
                [new DateTime(2022, 6, 18)] = 1050m, // 2022 low
                [new DateTime(2022, 12, 31)] = 1200m,
                [new DateTime(2023, 12, 31)] = 2300m,
                [new DateTime(2024, 12, 31)] = 3800m,
                [new DateTime(2025, 8, 22)] = 4750m
            },
            Volatility = 0.055
        },
        ["SOL"] = new CryptoHistoricalData
        {
            StartPrice = 1.5m, // Solana was very new in 2020
            KeyPrices = new Dictionary<DateTime, decimal>
            {
                [new DateTime(2020, 1, 1)] = 1.5m,
                [new DateTime(2020, 12, 31)] = 3.2m,
                [new DateTime(2021, 11, 6)] = 260m, // ATH 2021
                [new DateTime(2021, 12, 31)] = 170m,
                [new DateTime(2022, 12, 31)] = 9.8m, // FTX collapse impact
                [new DateTime(2023, 12, 31)] = 98m,
                [new DateTime(2024, 12, 31)] = 180m,
                [new DateTime(2025, 8, 22)] = 145m
            },
            Volatility = 0.08
        },
        ["XRP"] = new CryptoHistoricalData
        {
            StartPrice = 0.193m,
            KeyPrices = new Dictionary<DateTime, decimal>
            {
                [new DateTime(2020, 1, 1)] = 0.193m,
                [new DateTime(2020, 12, 31)] = 0.22m,
                [new DateTime(2021, 4, 14)] = 1.84m, // ATH 2021
                [new DateTime(2021, 12, 31)] = 0.83m,
                [new DateTime(2022, 12, 31)] = 0.35m,
                [new DateTime(2023, 7, 13)] = 0.93m, // SEC lawsuit developments
                [new DateTime(2023, 12, 31)] = 0.62m,
                [new DateTime(2024, 12, 31)] = 2.15m,
                [new DateTime(2025, 8, 22)] = 0.58m
            },
            Volatility = 0.065
        },
        ["BNB"] = new CryptoHistoricalData
        {
            StartPrice = 13.5m,
            KeyPrices = new Dictionary<DateTime, decimal>
            {
                [new DateTime(2020, 1, 1)] = 13.5m,
                [new DateTime(2020, 12, 31)] = 38m,
                [new DateTime(2021, 5, 10)] = 690m, // ATH 2021
                [new DateTime(2021, 12, 31)] = 530m,
                [new DateTime(2022, 12, 31)] = 245m,
                [new DateTime(2023, 12, 31)] = 315m,
                [new DateTime(2024, 12, 31)] = 695m,
                [new DateTime(2025, 8, 22)] = 550m
            },
            Volatility = 0.055
        },
        ["DOGE"] = new CryptoHistoricalData
        {
            StartPrice = 0.002m,
            KeyPrices = new Dictionary<DateTime, decimal>
            {
                [new DateTime(2020, 1, 1)] = 0.002m,
                [new DateTime(2020, 12, 31)] = 0.005m,
                [new DateTime(2021, 5, 8)] = 0.74m, // Musk/SNL peak
                [new DateTime(2021, 12, 31)] = 0.17m,
                [new DateTime(2022, 12, 31)] = 0.068m,
                [new DateTime(2023, 12, 31)] = 0.096m,
                [new DateTime(2024, 12, 31)] = 0.31m,
                [new DateTime(2025, 8, 22)] = 0.11m
            },
            Volatility = 0.12
        },
        ["TON"] = new CryptoHistoricalData
        {
            StartPrice = 0.1m, // TON had limited trading in early 2020
            KeyPrices = new Dictionary<DateTime, decimal>
            {
                [new DateTime(2020, 1, 1)] = 0.1m,
                [new DateTime(2020, 12, 31)] = 0.4m,
                [new DateTime(2021, 12, 31)] = 3.2m,
                [new DateTime(2022, 12, 31)] = 1.8m,
                [new DateTime(2023, 12, 31)] = 2.3m,
                [new DateTime(2024, 7, 15)] = 8.2m, // Peak 2024
                [new DateTime(2024, 12, 31)] = 5.1m,
                [new DateTime(2025, 8, 22)] = 5.8m
            },
            Volatility = 0.075
        },
        ["TRX"] = new CryptoHistoricalData
        {
            StartPrice = 0.0135m,
            KeyPrices = new Dictionary<DateTime, decimal>
            {
                [new DateTime(2020, 1, 1)] = 0.0135m,
                [new DateTime(2020, 12, 31)] = 0.025m,
                [new DateTime(2021, 5, 7)] = 0.165m, // ATH 2021
                [new DateTime(2021, 12, 31)] = 0.062m,
                [new DateTime(2022, 12, 31)] = 0.051m,
                [new DateTime(2023, 12, 31)] = 0.103m,
                [new DateTime(2024, 12, 31)] = 0.24m,
                [new DateTime(2025, 8, 22)] = 0.16m
            },
            Volatility = 0.07
        }
    };

    public static IEnumerable<PriceHistoryModel> GeneratePriceHistory(string cryptoSymbol, DateTime startDate, DateTime endDate)
    {
        if (cryptoSymbol == null)
            throw new ArgumentNullException(nameof(cryptoSymbol));

        if (startDate > endDate)
            throw new ArgumentException("Start date must be before or equal to end date");

        var random = new Random(12345); // Fixed seed for reproducible results

        if (!CryptoData.TryGetValue(cryptoSymbol, out var data))
        {
            throw new ArgumentException($"No historical data available for symbol: {cryptoSymbol}");
        }

        var priceHistories = new List<PriceHistoryModel>();
        var currentId = 1; // You might want to make this parameter-based if you need specific IDs

        // Generate prices for each day in the specified range
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            var price = GenerateRealisticPrice(cryptoSymbol, date, data, random);

            priceHistories.Add(new PriceHistoryModel
            {
               CryptoCurrencySymbol = cryptoSymbol,
                Date = date,
                Price = price
            });
        }

        return priceHistories;
    }

    private static decimal GenerateRealisticPrice(string symbol, DateTime date, CryptoHistoricalData data, Random random)
    {
        // Find the closest key prices before and after this date
        DateTime? beforeDate = null;
        DateTime? afterDate = null;
        decimal beforePrice = data.StartPrice;
        decimal afterPrice = data.StartPrice;

        foreach (var kvp in data.KeyPrices)
        {
            if (kvp.Key <= date && (beforeDate == null || kvp.Key > beforeDate))
            {
                beforeDate = kvp.Key;
                beforePrice = kvp.Value;
            }
            if (kvp.Key > date && (afterDate == null || kvp.Key < afterDate))
            {
                afterDate = kvp.Key;
                afterPrice = kvp.Value;
            }
        }

        // If no after date, use the last known price
        if (afterDate == null)
        {
            afterDate = beforeDate;
            afterPrice = beforePrice;
        }

        // Interpolate between key prices
        decimal basePrice;
        if (beforeDate == null)
        {
            basePrice = data.StartPrice;
        }
        else if (beforeDate == afterDate)
        {
            basePrice = beforePrice;
        }
        else
        {
            var totalDays = (afterDate.Value - beforeDate.Value).TotalDays;
            var daysPassed = (date - beforeDate.Value).TotalDays;
            var ratio = totalDays > 0 ? daysPassed / totalDays : 0;

            // Use logarithmic interpolation for more realistic crypto price movements
            var logBefore = Math.Log((double)beforePrice);
            var logAfter = Math.Log((double)afterPrice);
            var logInterpolated = logBefore + ratio * (logAfter - logBefore);
            basePrice = (decimal)Math.Exp(logInterpolated);
        }

        // Add daily volatility using normal distribution
        var volatilityFactor = 1.0 + (random.NextGaussian() * data.Volatility);
        var finalPrice = basePrice * (decimal)Math.Max(0.01, volatilityFactor); // Ensure price never goes below 1 cent

        return Math.Round(finalPrice, 8);
    }
}
