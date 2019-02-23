using DevExpress.Mvvm;

namespace FinancialAnalysis.Models.SalesManagement
{
    /// <summary>
    /// Rechnungsposition
    /// </summary>
    public class InvoicePosition : BindableBase
    {
        public InvoicePosition()
        {
        }

        public InvoicePosition(int RefInvoiceId, int RefSalesOrderPositionId, int Quantity)
        {
            this.RefInvoiceId = RefInvoiceId;
            this.RefSalesOrderPositionId = RefSalesOrderPositionId;
            this.Quantity = Quantity;
        }

        /// <summary>
        /// Id
        /// </summary>
        public int InvoicePositionId { get; set; }

        /// <summary>
        /// Referenz-Id Rechnung
        /// </summary>
        public int RefInvoiceId { get; set; }

        /// <summary>
        /// Referenz-Id Auftragsposition
        /// </summary>
        public int RefSalesOrderPositionId { get; set; }

        /// <summary>
        /// Menge
        /// </summary>
        public int Quantity { get; set; }
    }
}