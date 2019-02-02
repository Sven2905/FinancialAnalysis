using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FinancialAnalysis.Models.General
{
    public static class SvenTechColors
    {
        public static Color Cyan { get => (Color)ColorConverter.ConvertFromString("#FF2BBBAD"); }
        public static Color LightBlue { get => (Color)ColorConverter.ConvertFromString("#FF33b5e5"); }
        public static Color Blue { get => (Color)ColorConverter.ConvertFromString("#FF4285F4"); }
        public static Color Green { get => (Color)ColorConverter.ConvertFromString("#FF007E33"); } 
        public static Color Red { get => (Color)ColorConverter.ConvertFromString("#FFCC0000"); }  
        public static Color Yellow { get => (Color)ColorConverter.ConvertFromString("#FFFF8800"); } 
        public static Color SvenTechOrange { get => (Color)ColorConverter.ConvertFromString("#FFE49D20"); } 
        public static Color SvenTechGrey { get => (Color)ColorConverter.ConvertFromString("#FF919396"); }
    }
}
