using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.CarPoolManagement
{
    /// <summary>
    /// Bauart
    /// </summary>
    public class CarBody
    {
        /// <summary>
        /// Id der Bauart
        /// </summary>
        public int CarBodyId { get; set; }

        /// <summary>
        /// Name der Bauart
        /// </summary>
        public string Name { get; set; }
    }
}
