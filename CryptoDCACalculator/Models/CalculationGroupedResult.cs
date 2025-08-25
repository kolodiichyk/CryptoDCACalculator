using System.Collections.ObjectModel;

namespace CryptoDCACalculator.Models;

public class CalculationGroupedResult(DateTime date, IEnumerable<CalculationResult> items)
    : ObservableCollection<CalculationResult>(items)
{
    public DateTime Date { get; } = date;

    public decimal TotalInvested => this.Sum(x => x.InvestedAmount);
    public decimal TotalValueToday => this.Sum(x => x.ValueToday);
    public decimal ROI => this.Sum(x => x.TotalInvested) == 0
        ? 0
        : (this.Sum(x => x.TotalValueToday) - this.Sum(x => x.TotalInvested))
          / this.Sum(x => x.TotalInvested);
}
