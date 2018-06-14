using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace YouTubeManagerWpf.Converters
{
    public class ArrayToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var arr = value as string[];

            if (arr != null && arr.Any())
                return string.Join(", ", arr);

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
