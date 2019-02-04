using DevExpress.Mvvm;
using System;
using System.Windows.Media;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class StatusViewModel : BindableBase
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public string StatusText { get; set; } = "Placeholder";
        public string StatusValue { get; set; } = "Placeholder";
        public Geometry IconData { get; set; } = Geometry.Parse("M506 114H198c-3.3 0-6-2.7-6-6V84c0-3.3 2.7-6 6-6h308c3.3 0 6 2.7 6 6v24c0 3.3-2.7 6-6 6zm6 154v-24c0-3.3-2.7-6-6-6H198c-3.3 0-6 2.7-6 6v24c0 3.3 2.7 6 6 6h308c3.3 0 6-2.7 6-6zm0 160v-24c0-3.3-2.7-6-6-6H198c-3.3 0-6 2.7-6 6v24c0 3.3 2.7 6 6 6h308c3.3 0 6-2.7 6-6zM68.4 376c-19.9 0-36 16.1-36 36s16.1 36 36 36 35.6-16.1 35.6-36-15.7-36-35.6-36zM170.9 42.9l-7.4-7.4c-4.7-4.7-12.3-4.7-17 0l-78.8 79.2-38.4-38.4c-4.7-4.7-12.3-4.7-17 0l-8.9 8.9c-4.7 4.7-4.7 12.3 0 17l54.3 54.3c4.7 4.7 12.3 4.7 17 0l.2-.2L170.8 60c4.8-4.8 4.8-12.4.1-17.1zm.4 160l-7.4-7.4c-4.7-4.7-12.3-4.7-17 0l-78.8 79.2-38.4-38.4c-4.7-4.7-12.3-4.7-17 0L4 245.2c-4.7 4.7-4.7 12.3 0 17l54.3 54.3c4.7 4.7 12.3 4.7 17 0l-.2-.2 96.3-96.3c4.6-4.8 4.6-12.4-.1-17.1z");
        public Color Color1 { get; set; } = (Color)ColorConverter.ConvertFromString("#ffa726");
        public Color Color2 { get; set; } = (Color)ColorConverter.ConvertFromString("#fb8c00");
        public Brush IconColor { get; set; } = new SolidColorBrush(Colors.White);
        public double IconHeight { get; set; } = 50;
        public double IconWidth { get; set; } = 50;

        public void SetColorToOrange()
        {
            Color1 = (Color)ColorConverter.ConvertFromString("#ffa726");
            Color2 = (Color)ColorConverter.ConvertFromString("#fb8c00");
        }

        public void SetColorToPink()
        {
            Color1 = (Color)ColorConverter.ConvertFromString("#ec407a");
            Color2 = (Color)ColorConverter.ConvertFromString("#d81b60");
        }

        public void SetColorToGreen()
        {
            Color1 = (Color)ColorConverter.ConvertFromString("#66bb6a");
            Color2 = (Color)ColorConverter.ConvertFromString("#43a047");
        }

        public void SetColorToBlue()
        {
            Color1 = (Color)ColorConverter.ConvertFromString("#26c6da");
            Color2 = (Color)ColorConverter.ConvertFromString("#00acc1");
        }

        public void SetColorToRed()
        {
            Color1 = (Color)ColorConverter.ConvertFromString("#ff4444");
            Color2 = (Color)ColorConverter.ConvertFromString("#CC0000");
        }

        public void SetColorToYellow()
        {
            Color1 = (Color)ColorConverter.ConvertFromString("#ffbb33");
            Color2 = (Color)ColorConverter.ConvertFromString("#FF8800");
        }

        public void SetColorToCyan()
        {
            Color1 = (Color)ColorConverter.ConvertFromString("#2BBBAD");
            Color2 = (Color)ColorConverter.ConvertFromString("#00695c");
        }

        public void SetColorToDarkBlue()
        {
            Color1 = (Color)ColorConverter.ConvertFromString("#4285F4");
            Color2 = (Color)ColorConverter.ConvertFromString("#0d47a1");
        }

        public void SetIconData(string dataString)
        {
            IconData = Geometry.Parse(dataString);
        }

        public void SetIconColor(Color color)
        {
            IconColor = new SolidColorBrush(color);
        }

        public void SetColorFromHex(string hex1, string hex2)
        {
            Color1 = (Color)ColorConverter.ConvertFromString(hex1);
            Color2 = (Color)ColorConverter.ConvertFromString(hex2);
        }
    }
}