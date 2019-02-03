using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.WarehouseManagement;
using System;
using System.Linq;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class WarehouseViewModel : ViewModelBase
    {
        #region Constructor

        public WarehouseViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            FilteredWarehouses = _Warehouses = LoadAllWarehouses();
            NewWarehouseCommand = new DelegateCommand(NewWarehouse);
            SaveWarehouseCommand = new DelegateCommand(SaveWarehouse, Validation);
            DeleteWarehouseCommand = new DelegateCommand(DeleteWarehouse,
                () => SelectedWarehouse != null && SelectedWarehouse.Stockyards.All(x => x.IsEmpty));
            SelectedCommand = new DelegateCommand(() =>
            {
                SendSelectedToParent();
                CloseAction();
            });
        }

        #endregion Constructor

        #region UserRights

        public bool AllowSave =>
            UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessWarehouseSave) ||
            Globals.ActualUser.IsAdministrator;

        public bool AllowDelete =>
            UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessWarehouseDelete) ||
            Globals.ActualUser.IsAdministrator;

        #endregion UserRights

        #region Fields

        private readonly SvenTechCollection<Warehouse> _Warehouses = new SvenTechCollection<Warehouse>();
        private string _FilterText;

        #endregion Fields

        #region Methods

        private SvenTechCollection<Warehouse> LoadAllWarehouses()
        {
            var allWarehouses = new SvenTechCollection<Warehouse>();
            try
            {
                allWarehouses = DataContext.Instance.Warehouses.GetAll().ToSvenTechCollection();
            }
            catch (Exception ex)
            {
                // TODO Exception
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
                DataContext.Instance.Warehouses.Delete(SelectedWarehouse.WarehouseId);
                _Warehouses.Remove(SelectedWarehouse);
                SelectedWarehouse = null;
            }
            catch (Exception ex)
            {
                // TODO Exception
            }
        }

        private void SaveWarehouse()
        {
            try
            {
                if (SelectedWarehouse.WarehouseId != 0)
                {
                    DataContext.Instance.Warehouses.Update(SelectedWarehouse);
                }
                else
                {
                    SelectedWarehouse.WarehouseId = DataContext.Instance.Warehouses.Insert(SelectedWarehouse);
                }
            }
            catch (Exception ex)
            {
                // TODO Exception
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

        public void SendSelectedToParent()
        {
            if (SelectedWarehouse == null)
            {
                return;
            }

            if (SelectedWarehouse.WarehouseId == 0)
            {
                SaveWarehouse();
            }

            Messenger.Default.Send(new SelectedWarehouse { Warehouse = SelectedWarehouse });
        }

        #endregion Methods

        #region Properties

        public SvenTechCollection<Warehouse> FilteredWarehouses { get; set; } = new SvenTechCollection<Warehouse>();
        public DelegateCommand NewWarehouseCommand { get; set; }
        public DelegateCommand SaveWarehouseCommand { get; set; }
        public DelegateCommand DeleteWarehouseCommand { get; set; }
        public DelegateCommand SelectedCommand { get; }
        public Action CloseAction { get; set; }

        public string FilterText
        {
            get => _FilterText;
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