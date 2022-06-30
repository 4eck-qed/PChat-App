using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;

namespace PChat.GUI.Converters;

public class IsNullOrEmptyConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        switch (value)
        {
            case null:
                return true;
            case string s:
                return string.IsNullOrEmpty(s);
            case IEnumerable<object> collection:
                return !collection.Any();
            default:
                throw new NotSupportedException("Type not supported.");
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}