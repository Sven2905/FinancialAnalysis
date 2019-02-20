using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Enums
{
    public enum ReminderType
    {
        [Display(Name = "Postalisch")] Potal,
        [Display(Name = "Mail")] Mail,
        [Display(Name = "Postalisch und Mail")] PostalAndMail
    }
}
