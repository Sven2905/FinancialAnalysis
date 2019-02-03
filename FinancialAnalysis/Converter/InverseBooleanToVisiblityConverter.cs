using System;
using System.Globalization;
using System.Windows;

namespace FinancialAnalysis.UserControls
{
    /// <summary>
    ///     A converter that takes in a boolean and returns a <see cref="Visibility" />
    /// </summary>
    public class InverseBooleanToVisiblityConverter : BaseValueConverter<InverseBooleanToVisiblityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
            {
                return (bool)value ? Visibility.Visible : Visibility.Collapsed;
            }

            return (bool)value ? Visibility.Collapsed : Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}