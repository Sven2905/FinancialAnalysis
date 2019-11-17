using DevExpress.Mvvm;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.SalesManagement
{
    /// <summary>
    /// Lieferposition
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class ShippedProduct : BaseClass
    {
        /// <summary>
        /// Id
        /// </summary>
        public int ShippedProductId { get; set; }

        /// <summary>
        /// Referenz-Id der Warenlieferung
        /// </summary>
        public int RefShipmentId { get; set; }

        /// <summary>
        /// Referenz-Id der Auftragsposition
        /// </summary>
        public int RefSalesOrderPositionId { get; set; }

        /// <summary>
        /// Menge
        /// </summary>
        public int Quantity { get; set; }
    }
}