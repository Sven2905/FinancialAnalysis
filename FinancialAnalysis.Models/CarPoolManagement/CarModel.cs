using DevExpress.Mvvm;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.CarPoolManagement
{
    /// <summary>
    /// Automodell
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class CarModel : BindableBase
    {
        /// <summary>
        /// Id des Automodells
        /// </summary>
        public int CarModelId { get; set; }

        /// <summary>
        /// Name des Automodells
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Referenz-Id auf Automarke
        /// </summary>
        public int RefCarMakeId { get; set; }
    }
}
