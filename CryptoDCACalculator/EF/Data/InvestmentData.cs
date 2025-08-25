using System;
using System.Collections.Generic;
using CryptoDCACalculator.EF.Entities;

namespace CryptoDCACalculator.EF.Data;

internal static class InvestmentData
{
    public static IEnumerable<Investment> Seed()
    {
        return
        [
            new Investment { Id = 1, CryptoCurrencyId = 1, MonthlyAmount = 100, InvestmentDay = 1, StartDate = new DateTime(2023, 1, 15), IsActive = true },
            new Investment { Id = 2, CryptoCurrencyId = 2, MonthlyAmount = 150, InvestmentDay = 15, StartDate = new DateTime(2024, 2, 10), IsActive = true },
            new Investment { Id = 3, CryptoCurrencyId = 3, MonthlyAmount = 130, InvestmentDay = 10, StartDate = new DateTime(2025, 3, 13), IsActive = true },
            new Investment { Id = 4, CryptoCurrencyId = 4, MonthlyAmount = 100, InvestmentDay = 13, StartDate = new DateTime(2022, 3, 13), IsActive = true }
        ];
    }
}
