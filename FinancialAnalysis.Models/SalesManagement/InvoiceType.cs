using DevExpress.Mvvm;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.SalesManagement
{
    /// <summary>
    /// Rechnungstyp
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class InvoiceType : BaseClass
    {
        /// <summary>
        /// Id
        /// </summary>
        public int InvoiceTypeId { get; set; }

        /// <summary>
        /// Name des Rechnungstyps
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Beschreibung
        /// </summary>
        public string Description { get; set; }
    }
}