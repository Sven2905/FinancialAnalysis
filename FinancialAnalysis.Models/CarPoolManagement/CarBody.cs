using DevExpress.Mvvm;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.CarPoolManagement
{
    /// <summary>
    /// Bauart
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class CarBody : BindableBase
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