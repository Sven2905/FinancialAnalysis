using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Accounting
{
    public class ScannedDocument : BindableBase
    {
        public int ScannedDocumentId { get; set; }
        public byte[] Content { get; set; }
        public string FileName { get; set; }
        public DateTime Date { get; set; }
        public int RefBookingId { get; set; }
        public string ToolTip {
            get
            {
                return $"Dateiname: {FileName}" + Environment.NewLine + $"Datum: {Date.ToShortDateString()}";
            }
        }
    }
}
