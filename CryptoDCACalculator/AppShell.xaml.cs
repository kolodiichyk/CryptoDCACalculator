using System.Globalization;

namespace CryptoDCACalculator;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
    }
}
