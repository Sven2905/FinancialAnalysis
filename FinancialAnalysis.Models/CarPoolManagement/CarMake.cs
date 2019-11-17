using DevExpress.Mvvm;
using Newtonsoft.Json;
using Utilities;

namespace FinancialAnalysis.Models.CarPoolManagement
{
    /// <summary>
    /// Automarke
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class CarMake : BaseClass
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