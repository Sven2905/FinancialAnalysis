using DevExpress.Mvvm;
using DevExpress.Xpf.Dialogs;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class BookingViewModel : ViewModelBase
    {
        #region Fields

        private int _CreditorId;
        private int _DebitorId;
        private decimal _Amount;
        private CostAccount _Creditor;

        #endregion Fields

        #region Constructor

        public BookingViewModel()
        {

            GetCreditorCommand = new DelegateCommand(() =>
                {
                    Messenger.Default.Send(new OpenKontenrahmenWindowMessage() { AccountingType = AccountingType.Credit });
                });
            GetDebitorCommand = new DelegateCommand(() =>
            {
                Messenger.Default.Send(new OpenKontenrahmenWindowMessage() { AccountingType = AccountingType.Debit });
            });
            OpenFileCommand = new DelegateCommand(() =>
            {
                DXOpenFileDialog fileDialog = new DXOpenFileDialog();
                if (fileDialog.ShowDialog().Value)
                {
                    SaveFileToDatabase(fileDialog.FileName);
                }
            });
            DoubleClickListBoxCommand = new DelegateCommand(() =>
            {
                Messenger.Default.Send(new OpenPDFViewerWindowMessage(SelectedScannedDocument.ScannedDocumentId));
            });

            Messenger.Default.Register<SelectedCostAccount>(this, ChangeSelectedCostAccount);

            DataLayer db = new DataLayer();
            TaxTypes = db.TaxTypes.GetAll().ToList();
            CostAccounts = db.CostAccounts.GetAllVisible().ToList();
            ScannedDocuments = db.ScannedDocuments.GetAll().ToSvenTechCollection();
        }

        private void SaveFileToDatabase(string path)
        {
            var file = File.ReadAllBytes(path);
            FileInfo fileInfo = new FileInfo(path);

            var temp = path.Split('\\');
            var fileName = temp[temp.Length - 1].Replace(".pdf", "").Replace(".PDF", "");

            ScannedDocument scannedDocument = new ScannedDocument()
            {
                Content = file,
                Date = DateTime.Now,
                FileName = fileName,
                RefBookingId = 1
            };
            using(DataLayer db = new DataLayer())
            {
                db.ScannedDocuments.Insert(scannedDocument);
            }
        }

        #endregion Constructor

        #region Methods

        public void ChangeSelectedCostAccount(SelectedCostAccount SelectedCostAccount)
        {
            switch (SelectedCostAccount.AccountingType)
            {
                case AccountingType.Credit:
                    Creditor = SelectedCostAccount.CostAccount; CreditorId = SelectedCostAccount.CostAccount.CostAccountId; break;
                case AccountingType.Debit:
                    Debitor = SelectedCostAccount.CostAccount; DebitorId = SelectedCostAccount.CostAccount.CostAccountId; break;
                default:
                    break;
            }
        }

        #endregion Methods

        #region Properties

        public int CreditorId
        {
            get { return _CreditorId; }
            set { _CreditorId = value; Creditor = CostAccounts.Single(x => x.CostAccountId == value); }
        }

        public int DebitorId
        {
            get { return _DebitorId; }
            set { _DebitorId = value; Debitor = CostAccounts.Single(x => x.CostAccountId == value); }
        }

        public DelegateCommand GetCreditorCommand { get; }
        public DelegateCommand GetDebitorCommand { get; }
        public DelegateCommand OpenFileCommand { get; }
        public DelegateCommand DoubleClickListBoxCommand { get; set; }

        public CostAccount Creditor
        {
            get { return _Creditor; }
            set { _Creditor = value; SelectedTax = TaxTypes.Single(x => x.TaxTypeId == _Creditor.RefTaxTypeId); }
        }
        public CostAccount Debitor { get; set; }
        public TaxType SelectedTax { get; set; }
        public List<CostAccount> CostAccounts { get; set; }
        public List<TaxType> TaxTypes { get; set; }
        public DateTime Date { get; set; }
        public GrossNetType GrossNetType { get; set; }
        public ScannedDocument SelectedScannedDocument { get; set; }
        public SvenTechCollection<ScannedDocument> ScannedDocuments { get; set; }

        public decimal Amount
        {
            get { return Math.Round(_Amount, 2); }
            set { _Amount = value; RaisePropertyChanged(); }
        }

        #endregion Properties
    }
}
