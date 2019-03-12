using FinancialAnalysis.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.CarPoolManagement
{
    /// <summary>
    /// Motor
    /// </summary>
    public class CarEngine
    {
        /// <summary>
        /// Id des Motors
        /// </summary>
        public int CarEngineId { get; set; }

        /// <summary>
        /// Hubraum
        /// </summary>
        public int Volume { get; set; }

        /// <summary>
        /// Leistung (PS)
        /// </summary>
        public int Power { get; set; }

        /// <summary>
        /// Referenz-Id auf die Motorisierung
        /// </summary>
        public int RefCarTrimId { get; set; }

        /// <summary>
        /// Kraftstoffart
        /// </summary>
        public EngineType EngineType { get; set; }

        /// <summary>
        /// Getriebeart
        /// </summary>
        public CarGear CarGear { get; set; }
    }
}
