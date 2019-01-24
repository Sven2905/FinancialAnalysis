using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.SalesManagement;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class SalesTypeViewModel : ViewModelBase
    {
        #region Fields

        private readonly SalesType _SelectedSalesType;
        private SvenTechCollection<SalesType> _SalesTypes = new SvenTechCollection<SalesType>();
        private string _FilterText;

        #endregion Fields

        #region Constructor

        public SalesTypeViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            _SalesTypes = LoadAllSalesTypes();
            NewSalesTypeCommand = new DelegateCommand(NewSalesType);
            SaveSalesTypeCommand = new DelegateCommand(SaveSalesType, () => Validation());
            DeleteSalesTypeCommand = new DelegateCommand(DeleteSalesType, () => (SelectedSalesType != null));
        }

        #endregion Constructor

        #region Methods

        private SvenTechCollection<SalesType> LoadAllSalesTypes()
        {
            SvenTechCollection<SalesType> allSalesTypes = new SvenTechCollection<SalesType>();
            try
            {
                using (var db = new DataLayer())
                {
                    allSalesTypes = db.SalesTypes.GetAll().ToSvenTechCollection();
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }

            return allSalesTypes;
        }

        private void NewSalesType()
        {
            SelectedSalesType = new SalesType();
            _SalesTypes.Add(SelectedSalesType);
        }

        private void DeleteSalesType()
        {
            if (SelectedSalesType == null)
            {
                return;
            }

            if (SelectedSalesType.SalesTypeId == 0)
            {
                _SalesTypes.Remove(SelectedSalesType);
                SelectedSalesType = null;
                return;
            }

            try
            {
                using (var db = new DataLayer())
                {
                    db.SalesTypes.Delete(SelectedSalesType.SalesTypeId);
                    _SalesTypes.Remove(SelectedSalesType);
                    SelectedSalesType = null;
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void SaveSalesType()
        {
            try
            {
                if (SelectedSalesType.SalesTypeId != 0)
                {
                    using (var db = new DataLayer())
                    {
                        db.SalesTypes.Update(SelectedSalesType);
                    }
                }
                else
                {
                    using (var db = new DataLayer())
                    {
                        db.SalesTypes.Insert(SelectedSalesType);
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
            if (SelectedSalesType == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(SelectedSalesType.Name))
            {
                return false;
            }
            return true;
        }

        #endregion Methods

        #region Properties

        public SvenTechCollection<SalesType> FilteredSalesTypes { get; set; } = new SvenTechCollection<SalesType>();
        public DelegateCommand NewSalesTypeCommand { get; set; }
        public DelegateCommand SaveSalesTypeCommand { get; set; }
        public DelegateCommand DeleteSalesTypeCommand { get; set; }
        public string FilterText
        {
            get { return _FilterText; }
            set
            {
                _FilterText = value;
                if (!string.IsNullOrEmpty(_FilterText))
                {
                    FilteredSalesTypes = new SvenTechCollection<SalesType>();
                    foreach (var item in _SalesTypes)
                    {
                        if (item.Name.Contains(FilterText))
                        {
                            FilteredSalesTypes.Add(item);
                        }
                    }
                }
                else
                {
                    FilteredSalesTypes = _SalesTypes;
                }
            }
        }
        public SalesType SelectedSalesType { get; set; }

        public User ActualUser { get { return Globals.ActualUser; } }

        #endregion Properties
    }
}
