using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using FinancialAnalysis.Models.Accounting;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class AccountingViewModel : ViewModelBase
    {
        public ObservableCollection<CostAccount> CostAccounts { get; set; }
    }
}