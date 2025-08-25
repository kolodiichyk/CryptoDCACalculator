using System.Collections;
using System.Globalization;

namespace CryptoDCACalculator.Helpers.Converters;

public class NumberToAlternatingColorValueConverter : IValueConverter
{
    public Color EvenNumberColor { get; init; } = (Color)Application.Current.Resources["Secondary"];

    public Color OddNumberColor { get; init; } = (Color)Application.Current.Resources["White"];


    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var i = 0;
        if ((parameter as Binding)?.Source is CollectionView collectionView)
        {
            i = ((IList)collectionView.ItemsSource).IndexOf(value);
        }

        return i % 2 == 0
                ? EvenNumberColor
                : OddNumberColor;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException("NumberToAlternatingColorValueConverter.ConvertBack");
    }
}
