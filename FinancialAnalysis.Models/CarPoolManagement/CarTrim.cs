using DevExpress.Mvvm;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.CarPoolManagement
{
    /// <summary>
    /// Motorisierungen
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class CarTrim : BaseClass
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

        /// <summary>
        /// Referenz-Id auf die Generation
        /// </summary>
        public int RefCarGenerationId { get; set; }

        /// <summary>
        /// Referenz-Id auf die Bauart
        /// </summary>
        public int RefCarBodyId { get; set; }
    }
}