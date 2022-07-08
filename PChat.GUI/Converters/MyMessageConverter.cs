using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Google.Protobuf;
using PChat.Extensions;
using PChat.Shared;

namespace PChat.GUI.Converters
{
    public class MyMessageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case string stringValue:
                    return stringValue == Session.Account.Id.ToHexString();
                case ByteString byteString:
                    return byteString.ToHexString() == Session.Account.Id.ToHexString();
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}