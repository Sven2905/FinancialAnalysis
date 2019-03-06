using FinancialAnalysis.Logic.ViewModels;
using FinancialAnalysis.Models.SalesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace FinancialAnalysis.Logic.Messages
{
    public class OpenInvoiceListWindowMessage
    {
        public OpenInvoiceListWindowMessage(SvenTechCollection<Invoice> Invoices)
        {
            this.Invoices = Invoices;
        }

        public SvenTechCollection<Invoice> Invoices { get; set; }
    }
}
