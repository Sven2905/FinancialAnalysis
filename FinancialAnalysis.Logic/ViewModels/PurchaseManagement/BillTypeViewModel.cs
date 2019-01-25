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
    public class BillTypeViewModel : ViewModelBase
    {
        #region Fields

        private readonly BillType _SelectedBillType;
        private SvenTechCollection<BillType> _BillTypes = new SvenTechCollection<BillType>();
        private string _FilterText;

        #endregion Fields

        #region Constructor

        public BillTypeViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            _BillTypes = LoadAllBillTypes();
            NewBillTypeCommand = new DelegateCommand(NewBillType);
            SaveBillTypeCommand = new DelegateCommand(SaveBillType, () => Validation());
            DeleteBillTypeCommand = new DelegateCommand(DeleteBillType, () => (SelectedBillType != null));
        }

        #endregion Constructor

        #region Methods

        private SvenTechCollection<BillType> LoadAllBillTypes()
        {
            SvenTechCollection<BillType> allBillTypes = new SvenTechCollection<BillType>();
            try
            {
                using (var db = new DataLayer())
                {
                    allBillTypes = db.BillTypes.GetAll().ToSvenTechCollection();
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }

            return allBillTypes;
        }

        private void NewBillType()
        {
            SelectedBillType = new BillType();
            _BillTypes.Add(SelectedBillType);
        }

        private void DeleteBillType()
        {
            if (SelectedBillType == null)
            {
                return;
            }

            if (SelectedBillType.BillTypeId == 0)
            {
                _BillTypes.Remove(SelectedBillType);
                SelectedBillType = null;
                return;
            }

            try
            {
                using (var db = new DataLayer())
                {
                    db.BillTypes.Delete(SelectedBillType.BillTypeId);
                    _BillTypes.Remove(SelectedBillType);
                    SelectedBillType = null;
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void SaveBillType()
        {
            try
            {
                if (SelectedBillType.BillTypeId != 0)
                {
                    using (var db = new DataLayer())
                    {
                        db.BillTypes.Update(SelectedBillType);
                    }
                }
                else
                {
                    using (var db = new DataLayer())
                    {
                        db.BillTypes.Insert(SelectedBillType);
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
            if (SelectedBillType == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(SelectedBillType.Name))
            {
                return false;
            }
            return true;
        }

        #endregion Methods

        #region Properties

        public SvenTechCollection<BillType> FilteredBillTypes { get; set; } = new SvenTechCollection<BillType>();
        public DelegateCommand NewBillTypeCommand { get; set; }
        public DelegateCommand SaveBillTypeCommand { get; set; }
        public DelegateCommand DeleteBillTypeCommand { get; set; }
        public string FilterText
        {
            get { return _FilterText; }
            set
            {
                _FilterText = value;
                if (!string.IsNullOrEmpty(_FilterText))
                {
                    FilteredBillTypes = new SvenTechCollection<BillType>();
                    foreach (var item in _BillTypes)
                    {
                        if (item.Name.Contains(FilterText))
                        {
                            FilteredBillTypes.Add(item);
                        }
                    }
                }
                else
                {
                    FilteredBillTypes = _BillTypes;
                }
            }
        }
        public BillType SelectedBillType { get; set; }

        public User ActualUser { get { return Globals.ActualUser; } }

        #endregion Properties
    }
}
