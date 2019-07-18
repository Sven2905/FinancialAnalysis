using DevExpress.Mvvm;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.General;
using FinancialAnalysis.Models.ProjectManagement;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Windows.Media;
using Utilities;

namespace FinancialAnalysis.Models.SalesManagement
{
    /// <summary>
    /// Auftrag
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class SalesOrder : BindableBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public int SalesOrderId { get; set; }

        /// <summary>
        /// Referenz-Id des Debitors
        /// </summary>
        public int RefDebitorId { get; set; }

        /// <summary>
        /// Debitor
        /// </summary>
        public Debitor Debitor { get; set; }

        /// <summary>
        /// Auftragsdatum
        /// </summary>
        public DateTime OrderDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Referenz-Id des Auftragtyps
        /// </summary>
        public int RefSalesTypeId { get; set; } = 1;

        /// <summary>
        /// Auftragtyp
        /// </summary>
        public SalesType SalesType { get; set; }

        /// <summary>
        /// Anmerkung
        /// </summary>
        public string Remarks { get; set; } // Bemerkung

        /// <summary>
        /// Auftragspositionen
        /// </summary>
        public SvenTechCollection<SalesOrderPosition> SalesOrderPositions { get; set; } = new SvenTechCollection<SalesOrderPosition>();

        /// <summary>
        /// Warenlieferungen
        /// </summary>
        public SvenTechCollection<Shipment> Shipments { get; set; } = new SvenTechCollection<Shipment>();

        /// <summary>
        /// Rechnungen
        /// </summary>
        public SvenTechCollection<Invoice> Invoices { get; set; } = new SvenTechCollection<Invoice>();

        /// <summary>
        /// Farbe des Rechnungsstatus (Rot = Keine, Gelb = Ausstehende, Grün = Alle bezahlt)
        /// </summary>
        public Color InvoiceStatusColor { get; set; } = SvenTechColors.ColorGreen;

        /// <summary>
        /// Farbe des Warenlieferungsstatus (Rot = Keine, Gelb = Ausstehende, Grün = Alle geliefert)
        /// </summary>
        public Color ShippingStatusColor { get; set; } = SvenTechColors.ColorGreen;

        /// <summary>
        /// Ist geschlossen
        /// </summary>
        public bool IsClosed { get; set; }

        /// <summary>
        /// Ausgabe: ORDER + Id
        /// </summary>
        public string Barcode => "ORDER" + SalesOrderId.ToString("00000000");

        /// <summary>
        /// Referenz-Id Mitarbeiter
        /// </summary>
        public int RefEmployeeId { get; set; }

        /// <summary>
        /// Mitarbeiter
        /// </summary>
        public Employee Employee { get; set; }

        /// <summary>
        /// Summe aller Zwischenbeträge der Positionen
        /// </summary>
        public decimal SumSubtotal => SalesOrderPositions.Sum(x => x.Subtotal); // w/o tax

        /// <summary>
        /// Endbetrag des kompletten Auftrags
        /// </summary>
        public decimal SumTotal => SalesOrderPositions.Sum(x => x.Total);

        /// <summary>
        /// Summe aller Zwischenbeträge ohne Rabatt der Positionen
        /// </summary>
        public decimal SumSubtotalWithoutDiscount => SalesOrderPositions.Sum(x => x.SubtotalWithoutDiscount);

        /// <summary>
        /// Summe aller Steuerbeträge der Positionen
        /// </summary>
        public decimal SumTaxAmount => SalesOrderPositions.Sum(x => x.TaxAmount);

        /// <summary>
        /// Überprüft, ob Rechnungen und Warenlieferungen bestehen und setzt entsprechend die Statusfarbe
        /// </summary>
        public void CheckStatus()
        {
            if (Shipments?.Count > 0)
            {
                foreach (SalesOrderPosition item in SalesOrderPositions)
                {
                    int shippedProductsAmount = 0;
                    foreach (Shipment Shipment in Shipments)
                    {
                        foreach (ShippedProduct ShippedProduct in Shipment.ShippedProducts)
                        {
                            if (ShippedProduct.RefSalesOrderPositionId == item.SalesOrderPositionId)
                            {
                                shippedProductsAmount += ShippedProduct.Quantity;
                            }
                        }
                    }

                    if (item.Quantity != shippedProductsAmount)
                    {
                        ShippingStatusColor = SvenTechColors.ColorSvenTechOrange;
                    }
                }
            }
            else
            {
                ShippingStatusColor = SvenTechColors.ColorRed;
            }

            if (Invoices?.Count > 0)
            {
                foreach (Invoice Invoice in Invoices)
                {
                    if (!Invoice.IsPaid && Invoice.PaidAmount > 0)
                    {
                        InvoiceStatusColor = SvenTechColors.ColorSvenTechOrange;
                    }
                }

                if (Invoices.Sum(x => x.PaidAmount) < SumTotal)
                {
                    InvoiceStatusColor = SvenTechColors.ColorSvenTechOrange;
                }
            }
            else
            {
                InvoiceStatusColor = SvenTechColors.ColorRed;
            }
        }
    }
}