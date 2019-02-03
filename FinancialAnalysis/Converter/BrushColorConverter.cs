using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace FinancialAnalysis.UserControls
{
    public class BrushColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool) value)
            {
                return new SolidColorBrush(Color.FromRgb(0, 200, 81));
            }

            return new SolidColorBrush(Color.FromRgb(204, 0, 0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}