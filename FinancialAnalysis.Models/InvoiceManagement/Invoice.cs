using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.InvoiceManagement
{
    public class Invoice : BindableBase
    {
        public int InvoiceId { get; set; }
        public int ShipmentId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime InvoiceDueDate { get; set; }
        public int InvoiceTypeId { get; set; }
    }
}
