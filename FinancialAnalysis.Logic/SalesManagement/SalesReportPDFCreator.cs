using DevExpress.Mvvm;
using DevExpress.XtraPrinting.Drawing;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Reports;
using FinancialAnalysis.Models.SalesManagement;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace FinancialAnalysis.Logic.SalesManagement
{
    public static class SalesReportPDFCreator
    {
        private static SalesOrderReport _SalesOrderReport = new SalesOrderReport();
        private static InvoiceReport _InvoiceReport = new InvoiceReport();

        #region Order

        public static void CreateOrderReport(SalesOrderReportData salesOrderReportData, bool IsPreview)
        {
            var listReportData = new List<SalesOrderReportData> { salesOrderReportData };

            _SalesOrderReport = new SalesOrderReport
            {
                DataSource = listReportData
            };

            if (IsPreview)
            {
                _SalesOrderReport.Watermark.Text = "VORSCHAU";
                _SalesOrderReport.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                _SalesOrderReport.Watermark.Font = new Font(_SalesOrderReport.Watermark.Font.FontFamily, 40);
                _SalesOrderReport.Watermark.ForeColor = Color.DodgerBlue;
                _SalesOrderReport.Watermark.TextTransparency = 150;
                _SalesOrderReport.Watermark.ShowBehind = false;
            }

            _SalesOrderReport.CreateDocument();
        }

        public static void ShowOrderReport()
        {
            MemoryStream ms = new MemoryStream();
            _SalesOrderReport.PrintingSystem.ExportToPdf(ms);

            Messenger.Default.Send(new OpenPDFViewerWindowMessage(ms));
        }

        public static void CreateAndShowOrderReport(SalesOrderReportData salesOrderReportData, bool IsPreview)
        {
            CreateOrderReport(salesOrderReportData, IsPreview);
            ShowOrderReport();
        }

        #endregion Order

        #region Invoice

        public static void CreateInvoiceReport(InvoiceReportData InvoiceReportData, bool IsPreview)
        {
            var listReportData = new List<InvoiceReportData> { InvoiceReportData };

            _InvoiceReport = new InvoiceReport
            {
                DataSource = listReportData
            };

            if (IsPreview)
            {
                _InvoiceReport.Watermark.Text = "VORSCHAU";
                _InvoiceReport.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                _InvoiceReport.Watermark.Font = new Font(_SalesOrderReport.Watermark.Font.FontFamily, 40);
                _InvoiceReport.Watermark.ForeColor = Color.DodgerBlue;
                _InvoiceReport.Watermark.TextTransparency = 150;
                _InvoiceReport.Watermark.ShowBehind = false;
            }

            _InvoiceReport.CreateDocument();
        }

        public static void ShowInvoiceReport()
        {
            MemoryStream ms = new MemoryStream();
            _InvoiceReport.PrintingSystem.ExportToPdf(ms);

            Messenger.Default.Send(new OpenPDFViewerWindowMessage(ms));
        }

        public static void CreateAndShowInvoiceReport(InvoiceReportData invoiceReportData, bool IsPreview)
        {
            CreateInvoiceReport(invoiceReportData, IsPreview);
            ShowInvoiceReport();
        }

        #endregion Invoice
    }
}