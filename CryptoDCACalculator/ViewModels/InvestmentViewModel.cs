using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CryptoDCACalculator.EF;
using CryptoDCACalculator.Models;
using CryptoDCACalculator.ViewModels.Bases;
using CryptoDCACalculator.Helpers;
using CryptoDCACalculator.Helpers.Messages;
using Microsoft.EntityFrameworkCore;

namespace CryptoDCACalculator.ViewModels;

public partial class InvestmentViewModel(AppDbContext context, IMessenger messenger) : BaseViewModel, IQueryAttributable
{
    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private int? _id;

    [ObservableProperty]
    private DateTime? _startDate;

    [ObservableProperty]
    private int _investmentDay = 1;

    [ObservableProperty]
    private decimal _monthlyAmount;

    [ObservableProperty]
    private int _cryptoCurrencyId;

    [ObservableProperty]
    private bool _isActive;

    [ObservableProperty]
    private ObservableCollection<CryptoCurrencyModel> _cryptoCurrencies;

    [ObservableProperty]
    private CryptoCurrencyModel? _selectedCryptoCurrency;

    public void ApplyQueryAttributes(IDictionary<string, object> parameters)
    {
        if (parameters.TryGetValue("Investment", out InvestmentModel investment) && investment != null)
        {
            Id = investment.Id;
            StartDate = investment.StartDate;
            InvestmentDay = investment.InvestmentDay;
            MonthlyAmount = investment.MonthlyAmount;
            CryptoCurrencyId = investment.CryptoCurrency.Id;
            IsActive = investment.IsActive;
        }
    }

    public override async void OnAppearing()
    {
        base.OnAppearing();

        Title = Id == null ? "New Investment" : "Edit Investment";

        var cryptoCurrencies = (await context.CryptoCurrencies.ToListAsync()).ToModel();
        CryptoCurrencies = new ObservableCollection<CryptoCurrencyModel>(cryptoCurrencies);
        SelectedCryptoCurrency = CryptoCurrencies.FirstOrDefault(
            t=>t.Id == CryptoCurrencyId);
    }

    [RelayCommand]
    private async Task OnSaveAsync()
    {
        await TryInvokeAsync(async () =>
        {
            if (StartDate == null ||
                MonthlyAmount <= 0 ||
                SelectedCryptoCurrency == null ||
                InvestmentDay <= 0 || InvestmentDay > 31)
            {
                await Shell.Current.DisplayAlert("Alert", "Some of the parameters are empty or incorrect", "OK");
                return;
            }

            var isNew = Id == null;
            if (isNew)
            {
                var newInvestment = new EF.Entities.Investment
                {
                    StartDate = StartDate.Value,
                    InvestmentDay = InvestmentDay,
                    MonthlyAmount = MonthlyAmount,
                    CryptoCurrencyId = SelectedCryptoCurrency.Id,
                    IsActive = true
                };
                context.Investments.Add(newInvestment);
                await context.SaveChangesAsync();
                Id = newInvestment.Id;
            }
            else
            {
                var existingInvestment = await context.Investments.FindAsync(Id.Value);
                if (existingInvestment != null)
                {
                    existingInvestment.StartDate = StartDate.Value;
                    existingInvestment.InvestmentDay = InvestmentDay;
                    existingInvestment.MonthlyAmount = MonthlyAmount;
                    existingInvestment.CryptoCurrencyId = SelectedCryptoCurrency.Id;
                }

                await context.SaveChangesAsync();
            }

            messenger.Send(new InvestmentUpdatedEvent(new InvestmentChanges(Id.Value, isNew)));
            await Shell.Current.GoToAsync(isNew ?".." : "../../");
        });
    }

    [RelayCommand]
    private async Task OnDeleteAsync()
    {
        await TryInvokeAsync(async () =>
        {
            if (await Shell.Current.DisplayAlert("Delete", "Are you sure?", "Yes", "No"))
            {
                var investment = await context.Investments.FindAsync(Id.Value);
                context.Investments.Remove(investment);
                await context.SaveChangesAsync();

                messenger.Send(new InvestmentUpdatedEvent(new InvestmentChanges(Id.Value, false)));
                await Shell.Current.GoToAsync("../../");
            }
        });
    }
}
