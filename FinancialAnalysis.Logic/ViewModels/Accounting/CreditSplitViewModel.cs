using DevExpress.Mvvm;
using FinancialAnalysis.Logic.Manager;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.BaseClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using WebApiWrapper.Accounting;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class CreditSplitViewModel : ViewModelBase
    {
        public CreditSplitViewModel()
        {
            GetDataFromDB();
            AddCommand = new DelegateCommand(() => AddToCollection(), () => (Amount > 0 && CostAccount != null));
            DeleteCommand = new DelegateCommand(DeleteSelectedItem, () => SelectedCredit != null && Credits.Contains(SelectedCredit));
            SaveCommand = new DelegateCommand(SendSelectedToParent, () => Credits.Count > 0);
        }

        private decimal amount;
        private CostAccount costAccount;

        public decimal TotalAmount { get; set; } 
        public decimal Amount
        {
            get { return amount; }
            set
            {
                if (RemainingAmount >= 0 && value >= RemainingAmount)
                    amount = RemainingAmount;
                else
                    amount = value;
                RaisePropertyChanged("Amount");
                RaisePropertyChanged("TaxValue");
            }
        }

        public DelegateCommand AddCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public ObservableCollection<Credit> Credits { get; set; } = new ObservableCollection<Credit>();
        public ObservableCollection<CostAccount> CostAccountList { get; set; } = new ObservableCollection<CostAccount>();
        public ObservableCollection<TaxType> TaxTypeList { get; set; } = new ObservableCollection<TaxType>();
        public Credit SelectedCredit { get; set; } = new Credit();
        public TaxType SelectedTax { get; set; }
        public GrossNetType GrossNetType { get; set; } = GrossNetType.Brutto;
        public BookingType BookingType { get; set; }
        public string Description { get; set; }
        public Action CloseAction { get; set; }

        public CostAccount CostAccount
        {
            get => costAccount;
            set
            {
                costAccount = value;
                SelectedTax = TaxTypeList.Single(x => x.TaxTypeId == costAccount.RefTaxTypeId);
            }
        }
        public decimal RemainingAmount
        {
            get
            {
                if (Credits.Count > 0)
                    return TotalAmount - Credits.Sum(x => x.Amount);
                else
                    return TotalAmount;
            }
        }

        private void GetDataFromDB()
        {
            CostAccountList = CostAccounts.GetAll().ToSvenTechCollection();
            TaxTypeList = TaxTypes.GetAll().ToSvenTechCollection();
        }

        private void Reset()
        {
            Amount = RemainingAmount;
            RaisePropertyChanged("Amount");
            RaisePropertyChanged("RemainingAmount");
            Description = "";
        }

        private void AddToCollection()
        {
            Credits.AddRange(AccountBookingManager.Instance.CreateCredits(GrossNetType, SelectedTax, Amount, CostAccount, Description));
            Reset();
        }

        private void DeleteSelectedItem()
        {
            if (SelectedCredit == null)
                return;

            if (SelectedCredit.RefCreditId != 0)
            {
                var refCreditToRemove = Credits.SingleOrDefault(x => x.CreditId == SelectedCredit.RefCreditId);

                if (refCreditToRemove != null)
                Credits.Remove(refCreditToRemove);
            }

            var creditToRemove = Credits.SingleOrDefault(x => x.RefCreditId == SelectedCredit.CreditId);
            if (creditToRemove != null)
                Credits.Remove(creditToRemove);

            Credits.Remove(SelectedCredit);
        }

        public void SendSelectedToParent()
        {
            Messenger.Default.Send(new CreditSplitList { Credits = Credits });
            CloseAction();
        }
    }
}
