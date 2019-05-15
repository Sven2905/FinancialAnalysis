using DevExpress.Mvvm;
using FinancialAnalysis.Logic.Calculation;
using FinancialAnalysis.Models.Accounting;
using System;
using System.Linq;
using System.Windows.Input;
using Utilities;
using WebApiWrapper.Accounting;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class BalanceViewModel : ViewModelBase
    {
        public BalanceViewModel()
        {
            CreateCommand = new DelegateCommand(GetData);
        }

        public ICommand ExportCommand { get; set; }

        public BalanceAccountCalculation BalanceAccountCalculation { get; set; } = new BalanceAccountCalculation();

        public DateTime StartDate { get; set; } = new DateTime(DateTime.Now.Year, 1, 1);

        public DateTime EndDate { get; set; } = DateTime.Now;

        public DelegateCommand CreateCommand { get; set; }

        private void GetData()
        {
            BalanceAccountCalculation.GetAndCalculateData(StartDate, EndDate);
        }
    }
}
