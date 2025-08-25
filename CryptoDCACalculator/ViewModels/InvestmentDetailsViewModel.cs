using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CryptoDCACalculator.Models;
using CryptoDCACalculator.ViewModels.Bases;
using CryptoDCACalculator.Helpers;
using CryptoDCACalculator.Views;

namespace CryptoDCACalculator.ViewModels;

public partial class InvestmentDetailsViewModel : BaseViewModel, IQueryAttributable
{
    [ObservableProperty]
    private InvestmentModel _investment;

    [RelayCommand]
    private async Task OnEditInvestmentAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(InvestmentPage)}",
            new Dictionary<string, object>{ { nameof(Investment),  _investment } });
    }

    [RelayCommand]
    private async Task OnShowInvestmentChartAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(InvestmentDetailsChartPage)}",
            new Dictionary<string, object>{ { nameof(Investment),  _investment } });
    }

    public void ApplyQueryAttributes(IDictionary<string, object> parameters)
    {
        if (parameters == null)
            return;

        if (parameters.TryGetValue(nameof(Investment), out InvestmentModel investment) && investment != null)
            Investment = investment;
    }
}
