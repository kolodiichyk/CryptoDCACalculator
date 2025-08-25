using CryptoDCACalculator.EF;
using CryptoDCACalculator.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoDCACalculator.Services;

internal class DataUpdateService(IDbContextFactory<AppDbContext> contextFactory, ICoinApiService coinApiService) : IDataUpdateService
{
    public async Task<bool> NeedsPriceHistoryUpdateAsync(string cryptoSymbol, DateTime startDate, DateTime endDate)
    {
        if (endDate < startDate)
            throw new ArgumentException("End date must be greater than or equal to start date.");

        await using var appDbContext = await contextFactory.CreateDbContextAsync();
        var daysBetweenDates = (endDate.Date - startDate.Date).TotalDays;
        var daysBetweenDatesInDatabase =
            await appDbContext.PriceHistory.CountAsync(
                t => t.CryptoCurrency.Symbol == cryptoSymbol &&
                     t.Date >= startDate.Date &&
                     t.Date <= endDate.Date) - 1;

        return daysBetweenDates != daysBetweenDatesInDatabase;
    }

    public async Task UpdatePriceHistoryAsync(string cryptoSymbol, DateTime startDate, DateTime endDate)
    {
        var pricesHistory = await coinApiService.GetPricesHistoryAsync(cryptoSymbol, startDate.Date, endDate.Date);

        if (pricesHistory == null || !pricesHistory.Any())
            throw new InvalidOperationException("No price history data retrieved from the API.");

        var datesWithPrice = pricesHistory
            .Select(t => t.Date)
            .ToList();

        await using var appDbContext = await contextFactory.CreateDbContextAsync();
        var existingDatesWithPrice = appDbContext.PriceHistory
            .Where(t => t.CryptoCurrency.Symbol == cryptoSymbol && t.Date >= startDate && t.Date <= endDate)
            .Select(t=>t.Date).ToList();

        var newDates = datesWithPrice
            .Except(existingDatesWithPrice)
            .ToList();

        var newPricesHistory = pricesHistory
            .Where(t => newDates.Contains(t.Date))
            .ToList();

        var cryptoCurrency = await appDbContext.CryptoCurrencies.SingleAsync(t=>t.Symbol == cryptoSymbol);
        await appDbContext.PriceHistory.AddRangeAsync(
            newPricesHistory.Select(t=> new PriceHistory
            {
                CryptoCurrencyId = cryptoCurrency.Id,
                Date = t.Date,
                Price = t.Price
            }));
        await appDbContext.SaveChangesAsync();
    }
}
