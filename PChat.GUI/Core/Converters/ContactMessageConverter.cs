using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Google.Protobuf;
using PChat.Extensions;

namespace PChat.GUI.Core.Converters
{
    public class ContactMessageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case string stringValue:
                    return stringValue != SessionContent.Account.Id.ToHexString();
                case ByteString byteString:
                    return byteString.ToHexString() != SessionContent.Account.Id.ToHexString();
            }

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}