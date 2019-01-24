using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.ShipmentManagement;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ShipmentTypeViewModel : ViewModelBase
    {
        #region Fields

        private readonly ShipmentType _SelectedShipmentType;
        private SvenTechCollection<ShipmentType> _ShipmentTypes = new SvenTechCollection<ShipmentType>();
        private string _FilterText;

        #endregion Fields

        #region Constructor

        public ShipmentTypeViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            _ShipmentTypes = LoadAllShipmentTypes();
            NewShipmentTypeCommand = new DelegateCommand(NewShipmentType);
            SaveShipmentTypeCommand = new DelegateCommand(SaveShipmentType, () => Validation());
            DeleteShipmentTypeCommand = new DelegateCommand(DeleteShipmentType, () => (SelectedShipmentType != null));
        }

        #endregion Constructor

        #region Methods

        private SvenTechCollection<ShipmentType> LoadAllShipmentTypes()
        {
            SvenTechCollection<ShipmentType> allShipmentTypes = new SvenTechCollection<ShipmentType>();
            try
            {
                using (var db = new DataLayer())
                {
                    allShipmentTypes = db.ShipmentTypes.GetAll().ToSvenTechCollection();
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }

            return allShipmentTypes;
        }

        private void NewShipmentType()
        {
            SelectedShipmentType = new ShipmentType();
            _ShipmentTypes.Add(SelectedShipmentType);
        }

        private void DeleteShipmentType()
        {
            if (SelectedShipmentType == null)
            {
                return;
            }

            if (SelectedShipmentType.ShipmentTypeId == 0)
            {
                _ShipmentTypes.Remove(SelectedShipmentType);
                SelectedShipmentType = null;
                return;
            }

            try
            {
                using (var db = new DataLayer())
                {
                    db.ShipmentTypes.Delete(SelectedShipmentType.ShipmentTypeId);
                    _ShipmentTypes.Remove(SelectedShipmentType);
                    SelectedShipmentType = null;
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void SaveShipmentType()
        {
            try
            {
                if (SelectedShipmentType.ShipmentTypeId != 0)
                {
                    using (var db = new DataLayer())
                    {
                        db.ShipmentTypes.Update(SelectedShipmentType);
                    }
                }
                else
                {
                    using (var db = new DataLayer())
                    {
                        db.ShipmentTypes.Insert(SelectedShipmentType);
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
            if (SelectedShipmentType == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(SelectedShipmentType.Name))
            {
                return false;
            }
            return true;
        }

        #endregion Methods

        #region Properties

        public SvenTechCollection<ShipmentType> FilteredShipmentTypes { get; set; } = new SvenTechCollection<ShipmentType>();
        public DelegateCommand NewShipmentTypeCommand { get; set; }
        public DelegateCommand SaveShipmentTypeCommand { get; set; }
        public DelegateCommand DeleteShipmentTypeCommand { get; set; }
        public string FilterText
        {
            get { return _FilterText; }
            set
            {
                _FilterText = value;
                if (!string.IsNullOrEmpty(_FilterText))
                {
                    FilteredShipmentTypes = new SvenTechCollection<ShipmentType>();
                    foreach (var item in _ShipmentTypes)
                    {
                        if (item.Name.Contains(FilterText))
                        {
                            FilteredShipmentTypes.Add(item);
                        }
                    }
                }
                else
                {
                    FilteredShipmentTypes = _ShipmentTypes;
                }
            }
        }
        public ShipmentType SelectedShipmentType { get; set; }

        public User ActualUser { get { return Globals.ActualUser; } }

        #endregion Properties
    }
}
