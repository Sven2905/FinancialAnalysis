using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.Models.Accounting
{
    public class Kontenrahmen
    {
        public int KontenrahmenId { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public Standardkontenrahmen Type { get; set; }

        [InverseProperty("Receiver")]
        public virtual ICollection<Accounting> AccountingIncoming { get; set; }

        [InverseProperty("Sender")]
        public virtual ICollection<Accounting> AccountingOutgoing { get; set; }
    }
}
