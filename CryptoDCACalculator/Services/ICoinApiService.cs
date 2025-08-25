using CryptoDCACalculator.Models;

namespace CryptoDCACalculator.Services;

internal interface ICoinApiService
{
    Task<IEnumerable<PriceHistoryModel>> GetPricesHistoryAsync(string cryptoSymbol, DateTime dateFrom, DateTime dateTo);
}
