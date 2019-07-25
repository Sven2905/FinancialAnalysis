using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Enums
{
    public enum ItemPriceCalculationItemCostCenterType
    {

        [Display(Name = "Materialgemeinkosten")] MaterialOverheadCosts = 1,
        [Display(Name = "Fertigungsgemeinkosten")] ProductOverheadCosts = 2,
        [Display(Name = "Verwaltungsgemeinkosten")] AdministrativeOverheadCosts = 3,
        [Display(Name = "Vertriebsgemeinkosten")] SalesOverheadCosts = 4
    }
}
