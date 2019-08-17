using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.Data.Browsing.Design;
using FinancialAnalysis.Models.Accounting;
using Formulas.PriceCalculationMethods;
using WebApiWrapper.Accounting;

namespace FinancialAnalysis.Logic.Manager
{
    public class PriceCalculationManager
    {
        public PriceCalculationManager()
        {
            Task.Factory.StartNew(GetData);
        }

        public static PriceCalculationManager Instance { get; } = new PriceCalculationManager();

        private void GetData()
        {
            bookingCostCenterMappings = BookingCostCenterMappings.GetAll();
        }

        public StandardItemPriceCalculation StandardItemPrice { get; set; } = new StandardItemPriceCalculation();

        private List<BookingCostCenterMapping> bookingCostCenterMappings = new List<BookingCostCenterMapping>();
        
    }
}
