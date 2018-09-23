using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Accounting
{
    public class Kontenrahmen : BindableBase
    {
        public int KontenrahmenId { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public Standardkontenrahmen Type { get; set; }
        public List<Credit> Credits { get; set; }
        public List<Debit> Debits { get; set; }
    }
}
