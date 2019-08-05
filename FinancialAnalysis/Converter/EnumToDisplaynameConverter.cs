﻿using FinancialAnalysis.Models.Helper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace FinancialAnalysis.UserControls
{
    public class EnumToDisplaynameConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Enum)value).GetAttributeOfType<DisplayAttribute>().Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}