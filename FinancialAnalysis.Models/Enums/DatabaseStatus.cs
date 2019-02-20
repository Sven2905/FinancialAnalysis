using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Enums
{
    public enum DatabaseStatus
    {
        [Display(Name = "Getrennt")] Disconnected,
        [Display(Name = "Wartend")] Pending,
        [Display(Name = "Verbunden")] Connected
    }
}
