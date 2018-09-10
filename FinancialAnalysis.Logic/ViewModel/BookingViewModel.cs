using FinancialAnalysis.Logic.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpf.Dialogs;
using DevExpress.Xpf.Core;
using System.IO;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;

namespace FinancialAnalysis.Logic.ViewModel
{
    public class BookingViewModel : ViewModelBase
    {
        #region Fields
        
        private int _CreditorId;
        private int _DebitorId;
        private decimal _Amount;

        #endregion Fields

        #region Constructor

        public BookingViewModel()
        {

        GetCreditorCommand = new RelayCommand(() =>
            {
                MessengerInstance.Send(new OpenKontenrahmenWindowMessage() { AccountingType = AccountingType.Credit });
            });
            GetDebitorCommand = new RelayCommand(() =>
            {
                MessengerInstance.Send(new OpenKontenrahmenWindowMessage() { AccountingType = AccountingType.Debit });
            });
            OpenFileCommand = new RelayCommand(() =>
            {
                DXOpenFileDialog fileDialog = new DXOpenFileDialog();
                if (fileDialog.ShowDialog().Value)
                {
                    var file = File.ReadAllBytes(fileDialog.FileName);
                }
            });

            MessengerInstance.Register<SelectedCostAccount>(this, ChangeSelectedCostAccount);

            DataLayer db = new DataLayer();
            TaxTypes = db.TaxTypes.GetAll().ToList();

            //var ctx = new FinanceContext();
            //CostAccounts = ctx.CostAccounts.ToList();
            //TaxTypes = ctx.TaxTypes.ToList();
        }

        #endregion Constructor

        #region Methods

        public void ChangeSelectedCostAccount(SelectedCostAccount SelectedCostAccount)
        {
            switch (SelectedCostAccount.AccountingType)
            {
                case AccountingType.Credit:
                    Creditor = SelectedCostAccount.CostAccount; CreditorId = SelectedCostAccount.CostAccount.Id; break;
                case AccountingType.Debit:
                    Debitor = SelectedCostAccount.CostAccount; DebitorId = SelectedCostAccount.CostAccount.Id; break;
                default:
                    break;
            }
        }

        #endregion Methods

        #region Properties

        public int CreditorId
        {
            get { return _CreditorId; }
            set { _CreditorId = value; Creditor = CostAccounts.Single(x => x.Id == value); }
        }

        public int DebitorId
        {
            get { return _DebitorId; }
            set { _DebitorId = value; Debitor = CostAccounts.Single(x => x.Id == value); }
        }

        public RelayCommand GetCreditorCommand { get; }
        public RelayCommand GetDebitorCommand { get; }
        public RelayCommand OpenFileCommand { get; }
        public CostAccount Creditor { get; set; }
        public CostAccount Debitor { get; set; }
        public TaxType SelectedTax { get; set; }
        public List<CostAccount> CostAccounts { get; set; }
        public List<TaxType> TaxTypes { get; set; }
        public DateTime Date { get; set; }
        public GrossNetType GrossNetType { get; set; }

        public decimal Amount {
            get { return Math.Round(_Amount, 2); }
            set { _Amount = value; RaisePropertyChanged(); }
        }

        #endregion Properties
    }
}
