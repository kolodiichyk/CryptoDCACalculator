namespace CryptoDCACalculator.ViewModels.Bases;

public interface ILifecycle
{
    void OnDisappearing();

    void OnAppearing();

    void OnNavigatedFrom(NavigatedFromEventArgs args);

    void OnNavigatedTo(NavigatedToEventArgs args);

    void OnNavigatingFrom(NavigatingFromEventArgs args);
}
