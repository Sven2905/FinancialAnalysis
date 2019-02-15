using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Enums
{
    public enum AccountType
    {
        [Display(Name = "Aktivkonto")] Active,
        [Display(Name = "Passivkonto")] Passive
    }
}
