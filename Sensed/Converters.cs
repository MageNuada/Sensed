using System;
using Avalonia.Layout;
using Avalonia.Data.Converters;

namespace Sensed;

public class BoolAligmentConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        if (value is not bool b)
            return new Avalonia.Data.BindingNotification(new NotSupportedException(), Avalonia.Data.BindingErrorType.Error);
        return b ? HorizontalAlignment.Right : HorizontalAlignment.Left;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        return new Avalonia.Data.BindingNotification(new NotImplementedException(), Avalonia.Data.BindingErrorType.Error);
    }
}