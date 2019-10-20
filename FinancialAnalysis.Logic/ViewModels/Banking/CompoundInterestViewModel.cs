using DevExpress.Mvvm;
using Formulas;
using Formulas.Banking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class CompoundInterestViewModel : ViewModelBase
    {
        public CompoundInterestViewModel()
        {
            CalculateFinalCapitalCommand = new DelegateCommand(CalculateFinalCapital);
            CalculateRateCommand = new DelegateCommand(CalculateRate);
            CalculateYearsCommand = new DelegateCommand(CalculateYear);
        }

        public CompoundInterestIntervall CompoundInterestIntervall { get; set; } = CompoundInterestIntervall.Yearly;
        public DelegateCommand CalculateFinalCapitalCommand { get; set; }
        public DelegateCommand CalculateRateCommand { get; set; }
        public DelegateCommand CalculateYearsCommand { get; set; }
        public CompoundInterestItem CompoundInterestItemFinalCapital { get; set; } = new CompoundInterestItem();
        public CompoundInterestItem CompoundInterestItemRate { get; set; } = new CompoundInterestItem();
        public CompoundInterestItem CompoundInterestItemYear { get; set; } = new CompoundInterestItem();

        private void CalculateFinalCapital()
        {
            CompoundInterestItemFinalCapital.FinalCapital = CompoundInterest.CalculateFinalCapital(CompoundInterestItemFinalCapital);
            RaisePropertyChanged("CompoundInterestItemFinalCapital");
        }

        private void CalculateRate()
        {
            CompoundInterestItemRate.Rate = CompoundInterest.CalculateRate(CompoundInterestItemRate);
            RaisePropertyChanged("CompoundInterestItemRate");
        }

        private void CalculateYear()
        {
            CompoundInterestItemYear.Years = CompoundInterest.CalculateYears(CompoundInterestItemYear);
            RaisePropertyChanged("CompoundInterestItemYear");
        }
    }
}
