using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
    public static class Miscellaneous
    {
        public static decimal BerechneKapitalumschlag(decimal umsatz, decimal bilanzsumme)
        {
            return umsatz / bilanzsumme;
        }
    }
}
