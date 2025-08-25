using CryptoDCACalculator.EF;
using CryptoDCACalculator.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoDCACalculator.Services;

internal class CalculatorService(AppDbContext context, IDataUpdateService dataUpdateService) : ICalculatorService
{
    public async Task<List<CalculationResult>> CalculateDCAAsync(InvestmentModel investment)
    {
        if (investment == null || investment.CryptoCurrency == null || investment.StartDate == default)
        {
            throw new ArgumentNullException(nameof(investment));
        }

        await UpdateDatabaseIfNeeded(investment);

        var today = DateTime.Today.Date;
        var results = new List<CalculationResult>();
        var crypto = await context.CryptoCurrencies
            .Include(c => c.PriceHistory
                .Where(ph => ph.Date >= investment.StartDate && ph.Date <= today))
            .FirstOrDefaultAsync(
                c => c.Id == investment.CryptoCurrency.Id);

        if (crypto == null)
            return results;

        var currentDate = investment.StartDate.Date
            .AddDays(-investment.StartDate.Day)
            .AddDays(investment.InvestmentDay);
        currentDate = investment.StartDate.Date.Day > investment.InvestmentDay
            ? currentDate.AddMonths(1)
            : currentDate;

        var currentPrice = crypto.PriceHistory.FirstOrDefault(
            t=>t.Date.Date == today.Date)?.Price ?? 0;
        decimal totalInvested = 0;
        decimal totalCoins = 0;

        // Calculate for each month from start date to today
        while (currentDate < today)
        {
            var priceAtDate = crypto.PriceHistory.FirstOrDefault(
                                  t=> t.Date.Date == currentDate.Date)?.Price ?? 0;

            var calculationResult = CalculateDCAOnDateAsync(investment, currentDate, totalInvested, totalCoins, currentPrice, priceAtDate);
            totalInvested = calculationResult.TotalInvested;
            totalCoins = calculationResult.TotalCoinAmount;
            results.Add(calculationResult);

            currentDate = currentDate.AddMonths(1);
        }

        // Final calculation for today
        results.Add(CalculateDCAOnDateAsync(investment, today, totalInvested, totalCoins, currentPrice, currentPrice));

        return results.OrderBy(t=>t.Date).ToList();
    }

    private async Task UpdateDatabaseIfNeeded(InvestmentModel investment)
    {
        var today = DateTime.Today.Date;
        if (await dataUpdateService.NeedsPriceHistoryUpdateAsync(investment.CryptoCurrency.Symbol,
                investment.StartDate, today))
            await dataUpdateService.UpdatePriceHistoryAsync(investment.CryptoCurrency.Symbol,
                investment.StartDate, today);
    }

    private CalculationResult CalculateDCAOnDateAsync(
        InvestmentModel investment,
        DateTime investmentDate,
        decimal totalInvested,
        decimal totalCoins,
        decimal currentPrice,
        decimal priceAtDate)
    {
        if (investment == null ||
            investment.CryptoCurrency == null ||
            investment.StartDate == default ||
            priceAtDate <= 0)
            throw new ArgumentException("Invalid investment or price data.");

        var coinsAcquired = investment.MonthlyAmount / priceAtDate;
        totalInvested += investment.MonthlyAmount;
        totalCoins += coinsAcquired;

        var currentValue = totalCoins * currentPrice;
        var roi = totalInvested > 0 ? ((currentValue - totalInvested) / totalInvested) * 100 : 0;

        return new CalculationResult
        {
            Date = investmentDate,
            InvestedAmount = investment.MonthlyAmount,
            CoinAmount = coinsAcquired,
            ValueToday = coinsAcquired * currentPrice,
            ROI = roi,
            TotalInvested = totalInvested,
            TotalCoinAmount = totalCoins,
            TotalValueToday = currentValue,
            CoinSymbol = investment.CryptoCurrency.Symbol,
            PriceAtDate = priceAtDate,
        };
    }
}
