using CryptoDCACalculator.Models;

namespace CryptoDCACalculator.Services;

public interface ICalculatorService
{
    Task<List<CalculationResult>> CalculateDCAAsync(InvestmentModel investment);
}
