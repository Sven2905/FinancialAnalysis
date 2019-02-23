using DevExpress.Mvvm;

namespace FinancialAnalysis.Models.SalesManagement
{
    /// <summary>
    /// Lieferposition
    /// </summary>
    public class ShippedProduct : BindableBase
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