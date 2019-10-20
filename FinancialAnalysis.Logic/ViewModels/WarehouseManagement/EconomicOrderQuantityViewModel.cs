using DevExpress.Mvvm;
using FinancialAnalysis.Models.ProductManagement;
using Formulas.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class EconomicOrderQuantityViewModel : ViewModelBase
    {
        public EconomicOrderQuantityViewModel()
        {
            CalculateEconomicOrderQuantityCommand = new DelegateCommand(CalculateEconomicOrderQuantity);
        }

        public Product Product { get; set; }
        public int AnnualConsumption { get; set; }
        public decimal CostPerOrder { get; set; }
        public decimal InterestAndStorageCostsRate { get; set; }
        public double InterestRate { get; set; }
        public double Discount { get; set; }
        public double HoldingCosts { get; set; }
        public double EconomicOrderQuantity { get; set; }
        public double Turnus { get; set; }
        public double Frequency { get; set; }
        public bool IsAndlerChecked { get; set; } = true;

        public DelegateCommand CalculateEconomicOrderQuantityCommand { get; set; }

        private void CalculateEconomicOrderQuantity()
        {
            if (IsAndlerChecked)
                EconomicOrderQuantity = OrderOptimization.CalculateEconomicOrderQuantityAndler(AnnualConsumption, CostPerOrder, Product.DefaultSellingPrice, InterestAndStorageCostsRate);
            else
                EconomicOrderQuantity = OrderOptimization.CalculateEconomicOrderQuantityKosiol(AnnualConsumption, CostPerOrder, Product.DefaultSellingPrice, InterestRate, Discount, HoldingCosts);

            Frequency = OrderOptimization.CalculateOptimumOrderFrequency(AnnualConsumption, CostPerOrder, Product.DefaultSellingPrice, InterestAndStorageCostsRate);
            Turnus = OrderOptimization.CalculateOrderRotation(Frequency);
        }
    }
}
