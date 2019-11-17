using DevExpress.Mvvm;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.CarPoolManagement
{
    /// <summary>
    /// Generation
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class CarGeneration : BaseClass
    {
        /// <summary>
        /// Id der Generation
        /// </summary>
        public int CarGenerationId { get; set; }

        /// <summary>
        /// Name der Generation
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Referenz-Id auf das Model
        /// </summary>
        public int RefCarModelId { get; set; }
    }
}