using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CryptoDCACalculator.ViewModels.Bases;
using CryptoDCACalculator.Views;

namespace CryptoDCACalculator.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _userName;

    [ObservableProperty]
    private string _password;

    [RelayCommand]
    private async Task LoginAsync()
    {
        await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
    }
}
