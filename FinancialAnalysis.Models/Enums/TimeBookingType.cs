using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models
{
    public enum TimeBookingType
    {
        [Display(Name = "Kommen")] Login,
        [Display(Name = "Gehen")] Logout,
        [Display(Name = "Pause Start")] StartBreak,
        [Display(Name = "Pause Ende")] EndBreak,
    }
}
