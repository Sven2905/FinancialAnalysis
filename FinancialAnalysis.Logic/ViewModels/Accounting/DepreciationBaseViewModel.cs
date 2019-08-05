using DevExpress.Mvvm;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using Formulas.DepreciationMethods;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class DepreciationBaseViewModel : ViewModelBase
    {
        public DepreciationBaseViewModel()
        {
            DepreciationItem = new DepreciationItem();
            CalculateCommand = new DelegateCommand(Calculate);
            AddYearlyPowerCommand = new DelegateCommand(AddYearlyPower);
            RemoveYearlyPowerCommand = new DelegateCommand(RemoveYearlyPower);
        }

        private DepreciationItem _DepreciationItem;

        public DepreciationItem DepreciationItem
        {
            get => _DepreciationItem;
            set
            {
                if (_DepreciationItem != null)
                {
                    _DepreciationItem.PropertyChanged -= DepreciationItem_PropertyChanged;
                }

                _DepreciationItem = value;

                if (value != null)
                {
                    _DepreciationItem.PropertyChanged += DepreciationItem_PropertyChanged;
                }
            }
        }

        private void DepreciationItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("IsPerfomanceBased");
            Calculate();
        }

        public SvenTechCollection<DepreciationValue> DepreciationValues { get; set; }
        public DelegateCommand CalculateCommand { get; set; }
        public DelegateCommand AddYearlyPowerCommand { get; set; }
        public DelegateCommand RemoveYearlyPowerCommand { get; set; }
        public SvenTechCollection<PerformanceDepreciationItem> YearlyPowers { get; set; } = new SvenTechCollection<PerformanceDepreciationItem>();
        public bool IsPerfomanceBased { get { return DepreciationItem?.DepreciationType == DepreciationType.PerfomanceBased; } }
        public PerformanceDepreciationItem SelectedPerformanceDepreciationItem { get; set; }
        public int Power { get; set; }

        private void Calculate()
        {
            if (DepreciationItem?.DepreciationType == DepreciationType.PerfomanceBased)
            {
                if (YearlyPowers.Count == 0 || DepreciationItem.InitialValue <= 0)
                    return;
            }
            else if (DepreciationItem.Years <= 0 || DepreciationItem.InitialValue <= 0)
                return;

            switch (DepreciationItem.DepreciationType)
            {
                case DepreciationType.SumOfTheYearsDigitMethod:
                    DepreciationValues = Depreciations.CalculateArithmenticDegressiveValuesForYears(DepreciationItem.InitialValue, DepreciationItem.AssetValue, DepreciationItem.Years).ToSvenTechCollection();
                    break;

                case DepreciationType.DecliningBalanceMethod:
                    DepreciationValues = Depreciations.CalculateGeometryDregressiveForYears(DepreciationItem.InitialValue, DepreciationItem.AssetValue, DepreciationItem.Years).ToSvenTechCollection();
                    break;

                case DepreciationType.Linear:
                    DepreciationValues = Depreciations.CalculateLinearValueForYears(DepreciationItem.InitialValue, DepreciationItem.AssetValue, DepreciationItem.Years).ToSvenTechCollection();
                    break;

                case DepreciationType.PerfomanceBased:
                    if (YearlyPowers.Count > 0)
                        DepreciationValues = Depreciations.CalculatePerfomanceBased(DepreciationItem.InitialValue, YearlyPowers).ToSvenTechCollection();
                    break;
            }
        }

        private void AddYearlyPower()
        {
            PerformanceDepreciationItem performanceDepreciationItem = new PerformanceDepreciationItem(YearlyPowers.Count + 1, Power);
            YearlyPowers.Add(performanceDepreciationItem);
            Calculate();
        }

        private void RemoveYearlyPower()
        {
            if (SelectedPerformanceDepreciationItem == null)
                return;

            YearlyPowers.Remove(SelectedPerformanceDepreciationItem);

            for (int i = 0; i < YearlyPowers.Count; i++)
            {
                YearlyPowers[i].Year = i + 1;
            }

            Calculate();
        }
    }
}