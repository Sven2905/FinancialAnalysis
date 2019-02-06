using System.Windows.Media;

namespace FinancialAnalysis.Models.General
{
    public static class SvenTechColors
    {
        public static Color ColorCyan => (Color)ColorConverter.ConvertFromString("#FF2BBBAD");
        public static Color ColorLightBlue => (Color)ColorConverter.ConvertFromString("#FF33b5e5");
        public static Color ColorDarkBlue => (Color)ColorConverter.ConvertFromString("#FF173F5F");
        public static Color ColorBlue => (Color)ColorConverter.ConvertFromString("#FF4285F4");
        public static Color ColorAltBlue => (Color)ColorConverter.ConvertFromString("#FF20639B");
        public static Color ColorGreen => (Color)ColorConverter.ConvertFromString("#FF007E33");
        public static Color ColorLightGreen => (Color)ColorConverter.ConvertFromString("#FF3CAEA3");
        public static Color ColorRed => (Color)ColorConverter.ConvertFromString("#FFCC0000");
        public static Color ColorLightRed => (Color)ColorConverter.ConvertFromString("#FFED553B");
        public static Color ColorYellow => (Color)ColorConverter.ConvertFromString("#FFFF8800");
        public static Color ColorLightYellow => (Color)ColorConverter.ConvertFromString("#FFF6D55C");
        public static Color ColorSvenTechOrange => (Color)ColorConverter.ConvertFromString("#FFE49D20");
        public static Color ColorSvenTechGrey => (Color)ColorConverter.ConvertFromString("#FF919396");
        public static Color ColorSvenTechBlue => (Color)ColorConverter.ConvertFromString("#FF3f729b");

        public static Brush BrushCyan => new SolidColorBrush(ColorCyan);
        public static Brush BrushLightBlue => new SolidColorBrush(ColorLightBlue);
        public static Brush BrushBlue => new SolidColorBrush(ColorBlue);
        public static Brush BrushGreen => new SolidColorBrush(ColorGreen);
        public static Brush BrushRed => new SolidColorBrush(ColorRed);
        public static Brush BrushYellow => new SolidColorBrush(ColorYellow);
        public static Brush BrushSvenTechOrange => new SolidColorBrush(ColorSvenTechOrange);
        public static Brush BrushSvenTechGrey => new SolidColorBrush(ColorSvenTechGrey);
    }
}