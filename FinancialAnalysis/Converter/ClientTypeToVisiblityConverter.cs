using FinancialAnalysis.Models;
using System;
using System.Globalization;
using System.Windows;

namespace FinancialAnalysis.UserControls
{
    /// <summary>
    ///     A converter that takes in a boolean and returns a <see cref="Visibility" />
    /// </summary>
    public class ClientTypeToVisiblityConverter : BaseValueConverter<ClientTypeToVisiblityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ClientType clientType = (ClientType)value;

            switch (clientType)
            {
                case ClientType.Private:
                    return Visibility.Collapsed;

                case ClientType.Business:
                    return Visibility.Visible;

                default:
                    return Visibility.Collapsed;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}