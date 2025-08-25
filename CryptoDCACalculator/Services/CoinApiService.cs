using CryptoDCACalculator.Helpers.CryptoPriceDataGenerator;
using CryptoDCACalculator.Models;

namespace CryptoDCACalculator.Services;

internal class CoinApiService : ICoinApiService
{
    public Task<IEnumerable<PriceHistoryModel>> GetPricesHistoryAsync(string cryptoSymbol, DateTime dateFrom, DateTime dateTo)
    {
        return Task.FromResult(CryptoPriceDataGenerator.GeneratePriceHistory(cryptoSymbol, dateFrom, dateTo));
    }
}
