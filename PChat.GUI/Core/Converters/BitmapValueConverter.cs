using Avalonia;
using Avalonia.Markup;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace PChat.GUI.Core.Converters
{
    public class BitmapValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string stringValue || targetType != typeof(IBitmap))
                stringValue = "avares://PChat.GUI/Assets/Images/avatar.png";
            var uri = new Uri(stringValue, UriKind.RelativeOrAbsolute);
            var scheme = uri.IsAbsoluteUri ? uri.Scheme : "file";

            switch (scheme)
            {
                case "file":
                    return new Bitmap(stringValue);

                default:
                    var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
                    return new Bitmap(assets.Open(uri));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}