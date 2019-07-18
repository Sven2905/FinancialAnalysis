﻿using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;
using System.Globalization;

namespace FinancialAnalysis.Logic
{
    public class Globals : ViewModelBase
    {
        public static User ActiveUser { get; set; }
        public static CoreData CoreData { get; } = new CoreData();
        public static CultureInfo CultureInfo { get; } = new CultureInfo("de-DE");
    }
}