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
    public class StockyardViewModel : ViewModelBase
    {
        #region Fields

        #endregion Fields

        #region Constructor

        public StockyardViewModel()
        {
            if (IsInDesignMode)
                return;

            Warehouses = LoadAllWarehouses();
            NewStockyardCommand = new DelegateCommand(NewStockyard);
            SaveStockyardCommand = new DelegateCommand(SaveStockyard, () => Validation());
            DeleteStockyardCommand = new DelegateCommand(DeleteStockyard, () => (SelectedStockyard != null));
        }

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

        #endregion Constructor

        #region Methods

        private SvenTechCollection<Stockyard> LoadStockyards(int WarehouseId)
        {
            SvenTechCollection<Stockyard> allStockyards = new SvenTechCollection<Stockyard>();
            try
            {
                    allStockyards = DataLayer.Instance.Stockyards.GetByRefWarehouseId(WarehouseId).ToSvenTechCollection();
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }

            return allStockyards;
        }

        private void NewStockyard()
        {
            SelectedStockyard = new Stockyard
            {
                RefWarehouseId = SelectedWarehouse.WarehouseId
            };
            SelectedWarehouse.Stockyards.Add(SelectedStockyard);
        }

        private void DeleteStockyard()
        {
            if (SelectedStockyard == null)
            {
                return;
            }

            if (SelectedStockyard.StockyardId == 0)
            {
                SelectedWarehouse.Stockyards.Remove(SelectedStockyard);
                SelectedStockyard = null;
                return;
            }

            try
            {
                DataLayer.Instance.Stockyards.Delete(SelectedStockyard.StockyardId);
                SelectedWarehouse.Stockyards.Remove(SelectedStockyard);
                SelectedStockyard = null;
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void SaveStockyard()
        {
            try
            {
                if (SelectedStockyard.StockyardId != 0)
                {
                    DataLayer.Instance.Stockyards.Update(SelectedStockyard);
                }
                else
                {
                    DataLayer.Instance.Stockyards.Insert(SelectedStockyard);
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private bool Validation()
        {
            if (SelectedStockyard == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(SelectedStockyard.Name))
            {
                return false;
            }
            return true;
        }

        #endregion Methods

        #region Properties

        public SvenTechCollection<Stockyard> FilteredStockyards { get; set; } = new SvenTechCollection<Stockyard>();
        public DelegateCommand NewStockyardCommand { get; set; }
        public DelegateCommand SaveStockyardCommand { get; set; }
        public DelegateCommand DeleteStockyardCommand { get; set; }

        public SvenTechCollection<Warehouse> Warehouses { get; set; } = new SvenTechCollection<Warehouse>();
        public Warehouse SelectedWarehouse { get; set; }
        public Stockyard SelectedStockyard { get; set; }
        public User ActualUser { get { return Globals.ActualUser; } }

        #endregion Properties
    }
}
