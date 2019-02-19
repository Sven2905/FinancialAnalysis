using DevExpress.Mvvm;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.General;
using System;
using System.Linq;
using System.Windows.Media;
using Utilities;

namespace FinancialAnalysis.Models.SalesManagement
{
    public class SalesOrder : BindableBase
    {
        public int SalesOrderId { get; set; }
        public int RefDebitorId { get; set; }
        public Debitor Debitor { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public int RefSalesTypeId { get; set; }
        public SalesType SalesType { get; set; }
        public int RefShipmentTypeId { get; set; }
        public ShipmentType ShipmentType { get; set; }
        public string Remarks { get; set; } // Bemerkung
        public SvenTechCollection<SalesOrderPosition> SalesOrderPositions { get; set; } = new SvenTechCollection<SalesOrderPosition>();
        public SvenTechCollection<Shipment> Shipments { get; set; } = new SvenTechCollection<Shipment>();
        public SvenTechCollection<Invoice> Invoices { get; set; } = new SvenTechCollection<Invoice>();
        public Color InvoiceStatusColor { get; set; } = SvenTechColors.ColorGreen;
        public Color ShippingStatusColor { get; set; } = SvenTechColors.ColorRed;
        public bool IsClosed { get; set; }
        public string Barcode => "ORDER" + SalesOrderId.ToString("00000000");

        public int ReminderCounter { get; set; }

        public decimal SumSubtotal => SalesOrderPositions.Sum(x => x.Subtotal); // w/o tax
        public decimal SumTotal => SalesOrderPositions.Sum(x => x.Total);
        public decimal SumDiscountAmount => SalesOrderPositions.Sum(x => x.DiscountAmount);
        public decimal SumSubtotalWithoutDiscount => SalesOrderPositions.Sum(x => x.SubtotalWithoutDiscount);
        public decimal SumTaxAmount => SalesOrderPositions.Sum(x => x.TaxAmount);
    }
}