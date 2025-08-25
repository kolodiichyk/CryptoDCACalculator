using CryptoDCACalculator.ViewModels;
using CryptoDCACalculator.Views.Bases;

namespace CryptoDCACalculator.Views;

public partial class LoginPage : BaseContentPage<LoginViewModel>
{
    public LoginPage(LoginViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}
