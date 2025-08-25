using CommunityToolkit.Mvvm.ComponentModel;

namespace CryptoDCACalculator.ViewModels.Bases;

public abstract class BaseViewModel : ObservableObject, ILifecycle, IInitialize
{
    public virtual void Initialize()
    {

    }

    public virtual void OnDisappearing()
    {
    }

    public virtual void OnAppearing()
    {
    }

    public virtual void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
    }

    public virtual void OnNavigatedTo(NavigatedToEventArgs args)
    {
    }

    public virtual void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
    }

    protected void TryInvoke(Action action, Action onFinally = null)
    {
        try
        {
            action.Invoke();
        }
        catch (Exception e)
        {
            ExceptionHandler(e);
        }
        finally
        {
            onFinally?.Invoke();
        }
    }

    protected async Task TryInvokeAsync(Func<Task> action, Action onFinally = null)
    {
        try
        {
            await action.Invoke();
        }
        catch (Exception e)
        {
            ExceptionHandler(e);
        }
        finally
        {
            onFinally?.Invoke();
        }
    }

    private void ExceptionHandler(Exception e)
    {
        Console.WriteLine(e);
        MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.DisplayAlert("Error", e.Message, "OK"));
    }
}
