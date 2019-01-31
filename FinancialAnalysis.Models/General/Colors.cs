using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FinancialAnalysis.Models.General
{
    public class Colors
    {
        public static Color Red { get; } = (Color)ColorConverter.ConvertFromString("#FFCC0000");
        public static Color Green { get; } = (Color)ColorConverter.ConvertFromString("#FF007E33");
        public static Color Yellow { get; } = (Color)ColorConverter.ConvertFromString("#FFFF8800");
    }
}
