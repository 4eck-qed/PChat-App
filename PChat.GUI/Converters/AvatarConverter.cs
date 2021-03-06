using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Globalization;
using System.IO;
using Avalonia.Data.Converters;
using Google.Protobuf;

namespace PChat.GUI.Converters
{
    public class AvatarConverter : IValueConverter
    {
        private const string DefaultAvatar = "avares://PChat.GUI/Assets/Images/avatar.png";

        private static Bitmap ToBitmap(string? s)
        {
            if (string.IsNullOrWhiteSpace(s))
                s = DefaultAvatar;
            var uri = new Uri(s, UriKind.RelativeOrAbsolute);
            var scheme = uri.IsAbsoluteUri ? uri.Scheme : "file";

            switch (scheme)
            {
                case "file":
                    try
                    {
                        File.Open(s, FileMode.Open);
                    }
                    catch (Exception)
                    {
                        return ToBitmap(DefaultAvatar);
                    }

                    return new Bitmap(s);

                default:
                    var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
                    return new Bitmap(assets.Open(uri));
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case ByteString byteString:
                    return
                        ToBitmap(byteString
                            .ToStringUtf8()); //TODO handle if image is in byte format instead of avares link
                case string s:
                    return ToBitmap(s);
                default:
                    return ToBitmap(DefaultAvatar);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}