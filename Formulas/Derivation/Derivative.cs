using MathNet.Numerics.Differentiation;
using MathNet.Numerics.RootFinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas.Derivation
{
    public static class Derivative
    {
       public static Func<double, double> Derive(Func<double, double> f, int deriveRank)
        {
            //Func<double, double> f = x => 3 * Math.Pow(x, 3) + 2 * x - 6;
            var n = new NumericalDerivative(5,2);
            var df = n.CreateDerivativeFunctionHandle(f, deriveRank);
            return df;
        }
    }
}
