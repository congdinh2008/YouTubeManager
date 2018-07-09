using System;
using System.Windows.Data;
using System.Windows.Controls;

namespace YouTubeManagerWpf.Converters
{
    public class RowNumberConverter : IMultiValueConverter
    {
      #region IMultiValueConverter Members

      public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {

         //get the grid and the item
         Object item = values[0];
         DataGrid grid = values[1] as DataGrid;

         int index = grid.Items.IndexOf(item);

         return index.ToString();
      }

      public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
      {
         throw new NotImplementedException();
      }

      #endregion
   }
}
