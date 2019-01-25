using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.WarehouseManagement;
using System;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class WarehouseViewModel : ViewModelBase
    {
        #region UserRights
        public bool AllowSave { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessWarehouseSave) || Globals.ActualUser.IsAdministrator; } }
        public bool AllowDelete { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessWarehouseDelete) || Globals.ActualUser.IsAdministrator; } }
        #endregion UserRights

        #region Fields

        private SvenTechCollection<Warehouse> _Warehouses = new SvenTechCollection<Warehouse>();
        private string _FilterText;

        #endregion Fields

        #region Constructor

        public WarehouseViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            _Warehouses = LoadAllWarehouses();
            NewWarehouseCommand = new DelegateCommand(NewWarehouse);
            SaveWarehouseCommand = new DelegateCommand(SaveWarehouse, () => Validation());
            DeleteWarehouseCommand = new DelegateCommand(DeleteWarehouse, () => (SelectedWarehouse != null));
        }

        #endregion Constructor

        #region Methods

        private SvenTechCollection<Warehouse> LoadAllWarehouses()
        {
            SvenTechCollection<Warehouse> allWarehouses = new SvenTechCollection<Warehouse>();
            try
            {
                allWarehouses = DataLayer.Instance.Warehouses.GetAll().ToSvenTechCollection();
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }

            return allWarehouses;
        }

        private void NewWarehouse()
        {
            SelectedWarehouse = new Warehouse();
            _Warehouses.Add(SelectedWarehouse);
        }

        private void DeleteWarehouse()
        {
            if (SelectedWarehouse == null)
            {
                return;
            }

            if (SelectedWarehouse.WarehouseId == 0)
            {
                _Warehouses.Remove(SelectedWarehouse);
                SelectedWarehouse = null;
                return;
            }

            try
            {
                DataLayer.Instance.Warehouses.Delete(SelectedWarehouse.WarehouseId);
                _Warehouses.Remove(SelectedWarehouse);
                SelectedWarehouse = null;
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void SaveWarehouse()
        {
            try
            {
                if (SelectedWarehouse.WarehouseId != 0)
                    DataLayer.Instance.Warehouses.Update(SelectedWarehouse);
                else
                    DataLayer.Instance.Warehouses.Insert(SelectedWarehouse);
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private bool Validation()
        {
            if (SelectedWarehouse == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(SelectedWarehouse.Name))
            {
                return false;
            }
            return true;
        }

        #endregion Methods

        #region Properties

        public SvenTechCollection<Warehouse> FilteredWarehouses { get; set; } = new SvenTechCollection<Warehouse>();
        public DelegateCommand NewWarehouseCommand { get; set; }
        public DelegateCommand SaveWarehouseCommand { get; set; }
        public DelegateCommand DeleteWarehouseCommand { get; set; }
        public string FilterText
        {
            get { return _FilterText; }
            set
            {
                _FilterText = value;
                if (!string.IsNullOrEmpty(_FilterText))
                {
                    FilteredWarehouses = new SvenTechCollection<Warehouse>();
                    foreach (var item in _Warehouses)
                    {
                        if (item.Name.Contains(FilterText))
                        {
                            FilteredWarehouses.Add(item);
                        }
                    }
                }
                else
                {
                    FilteredWarehouses = _Warehouses;
                }
            }
        }

        public Warehouse SelectedWarehouse { get; set; }
        public int SelectedWarehouseStockyardCount { get; set; } = 0;

        #endregion Properties
    }
}
