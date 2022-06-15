using Avalonia;
using Avalonia.Markup;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Drawing;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Google.Protobuf;
using PChat.Extensions;
using Color = Avalonia.Media.Color;

namespace PChat.GUI.Core.Converters
{
    public class RandomColorConverter : IValueConverter
    {
        public static RandomColorConverter Instance = new RandomColorConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var random = new Random();
            var rbg = new byte[3];
            random.NextBytes(rbg);
            return Color.FromRgb(rbg[0], rbg[1], rbg[2]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is not string stringValue ? ByteString.Empty : HexString.ToByteString(stringValue))!;
        }
    }
}