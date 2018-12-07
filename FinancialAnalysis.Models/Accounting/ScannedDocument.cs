using System;
using DevExpress.Mvvm;

namespace FinancialAnalysis.Models.Accounting
{
    public class ScannedDocument : BindableBase
    {
        public int ScannedDocumentId { get; set; }
        public byte[] Content { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public DateTime Date { get; set; }
        public int RefBookingId { get; set; }

        public string ToolTip => $"Dateiname: {FileName}" + Environment.NewLine + $"Datum: {Date.ToShortDateString()}";
    }
}