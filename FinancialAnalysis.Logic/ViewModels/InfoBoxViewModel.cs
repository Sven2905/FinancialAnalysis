﻿using DevExpress.Mvvm;
using FinancialAnalysis.Models.General;
using System.Windows.Media;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class InfoBoxViewModel : ViewModelBase
    {
        public string Description { get; set; } = "Stunden";
        public string Unit { get; set; } = "h";
        public decimal Value { get; set; } = 50;
        public double IconHeight { get; set; } = 25;
        public double IconWidth { get; set; } = 25;
        public Geometry IconData { get; set; } = Geometry.Parse("M256 8C119 8 8 119 8 256s111 248 248 248 248-111 248-248S393 8 256 8zm216 248c0 118.7-96.1 216-216 216-118.7 0-216-96.1-216-216 0-118.7 96.1-216 216-216 118.7 0 216 96.1 216 216zm-148.9 88.3l-81.2-59c-3.1-2.3-4.9-5.9-4.9-9.7V116c0-6.6 5.4-12 12-12h14c6.6 0 12 5.4 12 12v146.3l70.5 51.3c5.4 3.9 6.5 11.4 2.6 16.8l-8.2 11.3c-3.9 5.3-11.4 6.5-16.8 2.6z");
        public Brush Color { get; set; } = SvenTechColors.BrushBlue;
        public Brush IconColor { get; set; } = new SolidColorBrush(Colors.White);

        public void SetIconData(string dataString)
        {
            IconData = Geometry.Parse(dataString);
        }
    }
}