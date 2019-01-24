using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.InvoiceManagement;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class InvoiceTypeViewModel : ViewModelBase
    {
        #region Fields

        private readonly InvoiceType _SelectedInvoiceType;
        private SvenTechCollection<InvoiceType> _InvoiceTypes = new SvenTechCollection<InvoiceType>();
        private string _FilterText;

        #endregion Fields

        #region Constructor

        public InvoiceTypeViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            _InvoiceTypes = LoadAllInvoiceTypes();
            NewInvoiceTypeCommand = new DelegateCommand(NewInvoiceType);
            SaveInvoiceTypeCommand = new DelegateCommand(SaveInvoiceType, () => Validation());
            DeleteInvoiceTypeCommand = new DelegateCommand(DeleteInvoiceType, () => (SelectedInvoiceType != null));
        }

        #endregion Constructor

        #region Methods

        private SvenTechCollection<InvoiceType> LoadAllInvoiceTypes()
        {
            SvenTechCollection<InvoiceType> allInvoiceTypes = new SvenTechCollection<InvoiceType>();
            try
            {
                using (var db = new DataLayer())
                {
                    allInvoiceTypes = db.InvoiceTypes.GetAll().ToSvenTechCollection();
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }

            return allInvoiceTypes;
        }

        private void NewInvoiceType()
        {
            SelectedInvoiceType = new InvoiceType();
            _InvoiceTypes.Add(SelectedInvoiceType);
        }

        private void DeleteInvoiceType()
        {
            if (SelectedInvoiceType == null)
            {
                return;
            }

            if (SelectedInvoiceType.InvoiceTypeId == 0)
            {
                _InvoiceTypes.Remove(SelectedInvoiceType);
                SelectedInvoiceType = null;
                return;
            }

            try
            {
                using (var db = new DataLayer())
                {
                    db.InvoiceTypes.Delete(SelectedInvoiceType.InvoiceTypeId);
                    _InvoiceTypes.Remove(SelectedInvoiceType);
                    SelectedInvoiceType = null;
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void SaveInvoiceType()
        {
            try
            {
                if (SelectedInvoiceType.InvoiceTypeId != 0)
                {
                    using (var db = new DataLayer())
                    {
                        db.InvoiceTypes.Update(SelectedInvoiceType);
                    }
                }
                else
                {
                    using (var db = new DataLayer())
                    {
                        db.InvoiceTypes.Insert(SelectedInvoiceType);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private bool Validation()
        {
            if (SelectedInvoiceType == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(SelectedInvoiceType.Name))
            {
                return false;
            }
            return true;
        }

        #endregion Methods

        #region Properties

        public SvenTechCollection<InvoiceType> FilteredInvoiceTypes { get; set; } = new SvenTechCollection<InvoiceType>();
        public DelegateCommand NewInvoiceTypeCommand { get; set; }
        public DelegateCommand SaveInvoiceTypeCommand { get; set; }
        public DelegateCommand DeleteInvoiceTypeCommand { get; set; }
        public string FilterText
        {
            get { return _FilterText; }
            set
            {
                _FilterText = value;
                if (!string.IsNullOrEmpty(_FilterText))
                {
                    FilteredInvoiceTypes = new SvenTechCollection<InvoiceType>();
                    foreach (var item in _InvoiceTypes)
                    {
                        if (item.Name.Contains(FilterText))
                        {
                            FilteredInvoiceTypes.Add(item);
                        }
                    }
                }
                else
                {
                    FilteredInvoiceTypes = _InvoiceTypes;
                }
            }
        }
        public InvoiceType SelectedInvoiceType { get; set; }

        public User ActualUser { get { return Globals.ActualUser; } }

        #endregion Properties
    }
}
