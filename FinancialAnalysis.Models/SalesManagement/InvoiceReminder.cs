using FinancialAnalysis.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.SalesManagement
{
    public class InvoiceReminder
    {
        public int InvoiceReminderId { get; set; }
        public int RefInvoiceId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Username { get; set; }
        public ReminderType ReminderType { get; set; }
        public bool IsLastReminder { get; set; } = false;
    }
}
