using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models
{
    public enum DepreciationType
    {
        [Display(Name = "Arithmetisch-degressiv")] ArithmenticDegressive,
        [Display(Name = "Geometrisch-degressiv")] GeometryDregressive,
        [Display(Name = "Linear")] Linear
    }
}
