using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.SalesManagement;
using Utilities;
using WebApiWrapper.SalesManagement;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ShipmentTypeViewModel : ViewModelBase
    {
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
            DeleteShipmentTypeCommand = new DelegateCommand(DeleteShipmentType, () => SelectedShipmentType != null);
        }

        #endregion Constructor

        #region Fields

        private readonly ShipmentType _SelectedShipmentType;
        private readonly SvenTechCollection<ShipmentType> _ShipmentTypes = new SvenTechCollection<ShipmentType>();
        private string _FilterText;

        #endregion Fields

        #region Methods

        private SvenTechCollection<ShipmentType> LoadAllShipmentTypes()
        {
            SvenTechCollection<ShipmentType> allShipmentTypes = new SvenTechCollection<ShipmentType>();
            allShipmentTypes = ShipmentTypes.GetAll().ToSvenTechCollection();

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

            ShipmentTypes.Delete(SelectedShipmentType.ShipmentTypeId);
            _ShipmentTypes.Remove(SelectedShipmentType);
            SelectedShipmentType = null;
        }

        private void SaveShipmentType()
        {
            if (SelectedShipmentType.ShipmentTypeId != 0)
            {
                ShipmentTypes.Update(SelectedShipmentType);
            }
            else
            {
                ShipmentTypes.Insert(SelectedShipmentType);
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

        public SvenTechCollection<ShipmentType> FilteredShipmentTypes { get; set; } =
            new SvenTechCollection<ShipmentType>();

        public DelegateCommand NewShipmentTypeCommand { get; set; }
        public DelegateCommand SaveShipmentTypeCommand { get; set; }
        public DelegateCommand DeleteShipmentTypeCommand { get; set; }

        public string FilterText
        {
            get => _FilterText;
            set
            {
                _FilterText = value;
                if (!string.IsNullOrEmpty(_FilterText))
                {
                    FilteredShipmentTypes = new SvenTechCollection<ShipmentType>();
                    foreach (ShipmentType item in _ShipmentTypes)
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

        public User ActualUser => Globals.ActiveUser;

        #endregion Properties
    }
}