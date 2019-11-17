using DevExpress.Mvvm;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.SalesManagement
{
    /// <summary>
    /// Auftragstyp
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class SalesType : BaseClass
    {
        /// <summary>
        /// Id
        /// </summary>
        public int SalesTypeId { get; set; }

        /// <summary>
        /// Name des Typs
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Beschreibung
        /// </summary>
        public string Description { get; set; }
    }
}