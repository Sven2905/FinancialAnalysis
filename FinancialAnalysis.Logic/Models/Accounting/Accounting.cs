using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.Models.Accounting
{
    public class Accounting
    {
        public int AccountingId { get; set; }

        [ForeignKey("SenderId")]
        public Kontenrahmen Sender { get; set; }

        [ForeignKey("ReceiverId")]
        public Kontenrahmen Receiver { get; set; }

        public int SenderId { get; set; }
        public int ReceiverId { get; set; }

        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
