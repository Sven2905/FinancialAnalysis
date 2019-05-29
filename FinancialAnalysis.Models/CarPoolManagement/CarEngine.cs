using DevExpress.Mvvm;
using FinancialAnalysis.Models.Enums;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.CarPoolManagement
{
    /// <summary>
    /// Motor
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class CarEngine : BindableBase
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
