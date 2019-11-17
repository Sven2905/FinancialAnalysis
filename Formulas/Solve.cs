using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
    public static class Solve
    {
        public static void QuadraticEquations(double a, double b, double c, out double x1, out double x2)
        {
            double d;
            d = b * b - 4 * a * c;
            if (d == 0)
            {
                x1 = -b / (2.0 * a);
                x2 = x1;
            }
            else if (d > 0)
            {
                x1 = (-b + Math.Sqrt(d)) / (2 * a);
                x2 = (-b - Math.Sqrt(d)) / (2 * a);
            }
            else
            {
                x1 = x2 = double.NaN;
            }
        }
    }
}
