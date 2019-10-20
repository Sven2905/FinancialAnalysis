using DevExpress.Mvvm;
using Formulas.Derivation;
using MathNet.Numerics;
using MathNet.Numerics.RootFinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.ViewModels.Optimization
{
    public class CostOptimizationViewModel : ViewModelBase
    {
        public CostOptimizationViewModel()
        {
            CalculateOptimizeProductionAmountCommand = new DelegateCommand(CalculateOptimizeProductionAmount);
        }

        public double A { get; set; } = 80;
        public double B { get; set; } = 3;
        public double C { get; set; } = 0.05;
        public double OptimizeAmount { get; set; }
        public double OptimizeProductionCosts { get; set; }
        public double SellPrice { get; set; }
        public double Profit { get; set; }
        public double ProfitAmount { get; set; }

        public DelegateCommand CalculateOptimizeProductionAmountCommand { get; set; }

        private void CalculateOptimizeProductionAmount()
        {
            Func<double, double> f = x => A + B* x + C * Math.Pow(x, 2);

            Func<double, double> dtk = x => A/x + B + C * x;

            var dtkFirstDerivative = Derivative.Derive(dtk, 1);
            
            OptimizeAmount = Math.Round(FindRoots.OfFunction(dtkFirstDerivative,1,500));
            OptimizeProductionCosts = dtk(OptimizeAmount);

            if (OptimizeProductionCosts < SellPrice)
            {
                Func<double, double> g = x => SellPrice*x - (A + B* x + C * Math.Pow(x, 2));
                var gFirstDerivative = Derivative.Derive(g, 1);
                ProfitAmount   = Math.Round(FindRoots.OfFunction(gFirstDerivative, 1, 500));
                Profit = g(ProfitAmount);
            }
            else
            {
                Profit = double.NaN;
            }
        }
    }
}
