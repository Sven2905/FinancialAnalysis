using System;
using System.Globalization;
using System.Windows;

namespace FinancialAnalysis.UserControls
{
    /// <summary>
    ///     A converter that takes in a boolean and returns a <see cref="Visibility" />
    /// </summary>
    public class ZeroToVisiblityConverter : BaseValueConverter<ZeroToVisiblityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
            {
                return (int)value == 0 ? Visibility.Collapsed : Visibility.Visible;
            }

            return (int)value == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}