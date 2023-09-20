using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Postwoman.WpfValueConverters;

public class RequestMethodColorSelector : IValueConverter
{

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string valueString)
        {
            return valueString switch
            {
                "GET" => new SolidColorBrush(Colors.Green),
                "POST" => new SolidColorBrush(Colors.Blue),
                "PUT" => new SolidColorBrush(Colors.DarkCyan),
                "DELETE" => new SolidColorBrush(Colors.Red),
                _ => Binding.DoNothing,
            };
        }
        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

}