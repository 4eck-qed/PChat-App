using System;
using System.Globalization;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.VisualTree;
using Google.Protobuf;
using PChat.Extensions;
using PChat.Shared;

namespace PChat.GUI.Converters
{
    public class CropWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case Grid grid:
                    var children = grid.Children.ToList();
                    var width = children.Sum(child => child.Width);
                    return width;
            }

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}