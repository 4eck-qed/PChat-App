using Avalonia;
using Avalonia.Markup;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Google.Protobuf;
using PChat.Extensions;

namespace PChat.GUI.Converters
{
    public class HexStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is not ByteString byteString ? "Not A ByteString" : byteString.ToHexString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is not string stringValue ? ByteString.Empty : HexString.ToByteString(stringValue))!;
        }
    }
}