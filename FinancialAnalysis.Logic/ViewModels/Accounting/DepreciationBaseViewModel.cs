using DevExpress.Mvvm;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using Formulas.DepreciationMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class DepreciationBaseViewModel : ViewModelBase
    {
        public DepreciationBaseViewModel()
        {
            CalculateCommand = new DelegateCommand(Calculate);
        }

        private DepreciationItem _DepreciationItem;

        public DepreciationItem DepreciationItem
        {
            get { return _DepreciationItem; }
            set
            {
                if (_DepreciationItem != null)
                    _DepreciationItem.PropertyChanged -= DepreciationItem_PropertyChanged;

                _DepreciationItem = value;

                if (value != null)
                    _DepreciationItem.PropertyChanged += DepreciationItem_PropertyChanged;
            }
        }

        private void DepreciationItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Calculate();
        }

        public SvenTechCollection<DepreciationValue> DepreciationValues { get; set; }
        public DelegateCommand CalculateCommand { get; set; }

        private void Calculate()
        {
            if (DepreciationItem.Years <= 0 || DepreciationItem.InitialValue <= 0)
            {
                return;
            }

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
                default:
                    break;
            }
        }
    }
}
