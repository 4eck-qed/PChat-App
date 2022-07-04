using System;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using PChat.Shared;

namespace PChat.GUI.Converters;

public class FriendRequestsForMeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return SessionContent.FriendRequests == null ? null : SessionContent.FriendRequests.Where(x => x.TargetId == SessionContent.Account.Id);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}