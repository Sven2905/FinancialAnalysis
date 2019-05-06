using DevExpress.Mvvm;
using FinancialAnalysis.Models;
using Formulas.DepreciationMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class DepreciationViewModel : ViewModelBase
    {
        public DepreciationViewModel()
        {
            CalculateCommand = new DelegateCommand(Calculate);
        }

        private DepreciationType _DepreciationType;
        private int _Years;
        private decimal _InitialValue;
        private decimal _AssetValue;

        public DepreciationType DepreciationType
        {
            get { return _DepreciationType; }
            set { _DepreciationType = value; Calculate(); }
        }

        public int Years
        {
            get { return _Years; }
            set { _Years = value; Calculate(); }
        }

        public decimal InitialValue
        {
            get { return _InitialValue; }
            set { _InitialValue = value; Calculate(); }
        }

        public decimal AssetValue
        {
            get { return _AssetValue; }
            set { _AssetValue = value; Calculate(); }
        }

        public SvenTechCollection<DepreciationValue> DepreciationValues { get; set; }
        public DelegateCommand CalculateCommand { get; set; }

        private void Calculate()
        {
            if (Years <= 0 || InitialValue <= 0)
            {
                return;
            }

            switch (DepreciationType)
            {
                case DepreciationType.SumOfTheYearsDigitMethod:
                    DepreciationValues = Depreciations.CalculateArithmenticDegressiveValuesForYears(InitialValue, AssetValue, Years).ToSvenTechCollection();
                    break;
                case DepreciationType.DecliningBalanceMethod:
                    DepreciationValues = Depreciations.CalculateGeometryDregressiveForYears(InitialValue, AssetValue, Years).ToSvenTechCollection();
                    break;
                case DepreciationType.Linear:
                    DepreciationValues = Depreciations.CalculateLinearValueForYears(InitialValue, AssetValue, Years).ToSvenTechCollection();
                    break;
                default:
                    break;
            }
        }
    }
}
