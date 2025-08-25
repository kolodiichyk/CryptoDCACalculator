using CommunityToolkit.Mvvm.ComponentModel;
using CryptoDCACalculator.ViewModels.Bases;

namespace CryptoDCACalculator.Views.Bases;

public abstract class BaseContentPage<TViewModel> : ContentPage where TViewModel : ObservableObject
{
    protected BaseContentPage(TViewModel viewModel)
    {
        base.BindingContext = viewModel;
        if (viewModel is IInitialize viewModelWithInitialize)
        {
            viewModelWithInitialize.Initialize();
        }
    }

    protected new TViewModel BindingContext => (TViewModel)base.BindingContext;

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ILifecycle viewModel)
        {
            viewModel.OnAppearing();
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (BindingContext is ILifecycle viewModel)
        {
            viewModel.OnDisappearing();
        }
    }

    protected override void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
        base.OnNavigatingFrom(args);
        if (BindingContext is ILifecycle viewModel)
        {
            viewModel.OnNavigatingFrom(args);
        }
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        if (BindingContext is ILifecycle viewModel)
        {
            viewModel.OnNavigatedFrom(args);
        }
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (BindingContext is ILifecycle viewModel)
        {
            viewModel.OnNavigatedTo(args);
        }
    }
}
