using FinancialAnalysis.Models.SalesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.Messages
{
    public class OpenInvoiceWindowMessage
    {
        public OpenInvoiceWindowMessage(SalesOrder SalesOrder)
        {
            this.SalesOrder = SalesOrder;
        }

        public SalesOrder SalesOrder { get; set; }
    }
}
