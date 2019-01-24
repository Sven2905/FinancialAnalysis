using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.PurchaseManagement;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class PurchaseTypeViewModel : ViewModelBase
    {
        #region Fields

        private readonly PurchaseType _SelectedPurchaseType;
        private SvenTechCollection<PurchaseType> _PurchaseTypes = new SvenTechCollection<PurchaseType>();
        private string _FilterText;

        #endregion Fields

        #region Constructor

        public PurchaseTypeViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            _PurchaseTypes = LoadAllPurchaseTypes();
            NewPurchaseTypeCommand = new DelegateCommand(NewPurchaseType);
            SavePurchaseTypeCommand = new DelegateCommand(SavePurchaseType, () => Validation());
            DeletePurchaseTypeCommand = new DelegateCommand(DeletePurchaseType, () => (SelectedPurchaseType != null));
        }

        #endregion Constructor

        #region Methods

        private SvenTechCollection<PurchaseType> LoadAllPurchaseTypes()
        {
            SvenTechCollection<PurchaseType> allPurchaseTypes = new SvenTechCollection<PurchaseType>();
            try
            {
                using (var db = new DataLayer())
                {
                    allPurchaseTypes = db.PurchaseTypes.GetAll().ToSvenTechCollection();
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }

            return allPurchaseTypes;
        }

        private void NewPurchaseType()
        {
            SelectedPurchaseType = new PurchaseType();
            _PurchaseTypes.Add(SelectedPurchaseType);
        }

        private void DeletePurchaseType()
        {
            if (SelectedPurchaseType == null)
            {
                return;
            }

            if (SelectedPurchaseType.PurchaseTypeId == 0)
            {
                _PurchaseTypes.Remove(SelectedPurchaseType);
                SelectedPurchaseType = null;
                return;
            }

            try
            {
                using (var db = new DataLayer())
                {
                    db.PurchaseTypes.Delete(SelectedPurchaseType.PurchaseTypeId);
                    _PurchaseTypes.Remove(SelectedPurchaseType);
                    SelectedPurchaseType = null;
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void SavePurchaseType()
        {
            try
            {
                if (SelectedPurchaseType.PurchaseTypeId != 0)
                {
                    using (var db = new DataLayer())
                    {
                        db.PurchaseTypes.Update(SelectedPurchaseType);
                    }
                }
                else
                {
                    using (var db = new DataLayer())
                    {
                        db.PurchaseTypes.Insert(SelectedPurchaseType);
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
            if (SelectedPurchaseType == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(SelectedPurchaseType.Name))
            {
                return false;
            }
            return true;
        }

        #endregion Methods

        #region Properties

        public SvenTechCollection<PurchaseType> FilteredPurchaseTypes { get; set; } = new SvenTechCollection<PurchaseType>();
        public DelegateCommand NewPurchaseTypeCommand { get; set; }
        public DelegateCommand SavePurchaseTypeCommand { get; set; }
        public DelegateCommand DeletePurchaseTypeCommand { get; set; }
        public string FilterText
        {
            get { return _FilterText; }
            set
            {
                _FilterText = value;
                if (!string.IsNullOrEmpty(_FilterText))
                {
                    FilteredPurchaseTypes = new SvenTechCollection<PurchaseType>();
                    foreach (var item in _PurchaseTypes)
                    {
                        if (item.Name.Contains(FilterText))
                        {
                            FilteredPurchaseTypes.Add(item);
                        }
                    }
                }
                else
                {
                    FilteredPurchaseTypes = _PurchaseTypes;
                }
            }
        }
        public PurchaseType SelectedPurchaseType { get; set; }

        public User ActualUser { get { return Globals.ActualUser; } }

        #endregion Properties
    }
}
