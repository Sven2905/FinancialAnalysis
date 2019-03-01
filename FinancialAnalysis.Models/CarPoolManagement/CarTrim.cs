using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.CarPoolManagement
{
    /// <summary>
    /// Motorisierungen
    /// </summary>
    public class CarTrim
    {
        /// <summary>
        /// Id der Motorisierungen
        /// </summary>
        public int CarTrimId { get; set; }

        /// <summary>
        /// Name der Motorisierungen
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Jahr
        /// </summary>
        public int Year { get; set; }
    }
}
