using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models
{
    public enum CostCenterType
    {
        [Display(Name = "Hauptkostenstelle")] Main,
        [Display(Name = "Nebenkostenstelle")] Extension,
        [Display(Name = "Hilfskostenstelle")] Indirect,
    }
}
