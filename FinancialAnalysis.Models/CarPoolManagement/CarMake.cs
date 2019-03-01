using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace FinancialAnalysis.Models.CarPoolManagement
{
    /// <summary>
    /// Automarke
    /// </summary>
    public class CarMake
    {
        /// <summary>
        /// Id der Automarke
        /// </summary>
        public int CarMakeId { get; set; }

        /// <summary>
        /// Name der Automarke
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Modelle der Automarke
        /// </summary>
        public SvenTechCollection<CarModel> Models { get; set; } = new SvenTechCollection<CarModel>();
    }
}
