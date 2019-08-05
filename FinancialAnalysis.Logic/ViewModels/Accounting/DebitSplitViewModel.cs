using DevExpress.Mvvm;
using FinancialAnalysis.Logic.Manager;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Utilities;
using WebApiWrapper.Accounting;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class DebitSplitViewModel : ViewModelBase
    {
        public DebitSplitViewModel()
        {
            GetDataFromDB();
            AddCommand = new DelegateCommand(() => AddToCollection(), () => (Amount > 0 && CostAccount != null));
            DeleteCommand = new DelegateCommand(DeleteSelectedItem, () => SelectedDebit != null && Debits.Contains(SelectedDebit));
            SaveCommand = new DelegateCommand(SendSelectedToParent, () => Debits.Count > 0);
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
        public ObservableCollection<Debit> Debits { get; set; } = new ObservableCollection<Debit>();
        public ObservableCollection<CostAccount> CostAccountList { get; set; } = new ObservableCollection<CostAccount>();
        public ObservableCollection<TaxType> TaxTypeList { get; set; } = new ObservableCollection<TaxType>();
        public TaxType SelectedTax { get; set; }
        public GrossNetType GrossNetType { get; set; } = GrossNetType.Brutto;
        public BookingType BookingType { get; set; }
        public string Description { get; set; }
        public Debit SelectedDebit;
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
                if (Debits.Count > 0)
                    return TotalAmount - Debits.Sum(x => x.Amount);
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
            Debits.AddRange(AccountBookingManager.Instance.CreateDebits(GrossNetType, SelectedTax, Amount, CostAccount, Description));
            Reset();
        }

        private void DeleteSelectedItem()
        {
            if (SelectedDebit == null)
                return;

            if (SelectedDebit.RefDebitId != 0)
            {
                var refDebitToRemove = Debits.SingleOrDefault(x => x.DebitId == SelectedDebit.RefDebitId);

                if (refDebitToRemove != null)
                    Debits.Remove(refDebitToRemove);
            }

            var debitToRemove = Debits.SingleOrDefault(x => x.RefDebitId == SelectedDebit.DebitId);
            if (debitToRemove != null)
                Debits.Remove(debitToRemove);

            Debits.Remove(SelectedDebit);
        }

        public void SendSelectedToParent()
        {
            Messenger.Default.Send(new DebitSplitList { Debits = Debits });
            CloseAction();
        }
    }
}