using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.WarehouseManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class StockyardViewModel : ViewModelBase
    {
        #region Constructor

        public StockyardViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            Messenger.Default.Register<SelectedWarehouse>(this, ChangeSelectedWarehouse);

            SetCommands();
            LoadAllWarehouses();
        }

        #endregion Constructor

        #region Methods

        private void SetCommands()
        {
            NewStockyardCommand = new DelegateCommand(NewStockyard);
            SaveStockyardCommand = new DelegateCommand(SaveStockyard, () => Validation());
            DeleteStockyardCommand = new DelegateCommand(DeleteStockyard,
                () => SelectedStockyard != null && SelectedStockyard.IsEmpty);
            OpenWarehousesWindowCommand = new DelegateCommand(OpenProductCategoriesWindow);
            StockyardsGenerationCommand = new DelegateCommand(GenerateStockyards,
                () => NumberOfStockyardsToCreate > 0 && SelectedWarehouse != null);
        }

        private void OpenProductCategoriesWindow()
        {
            Messenger.Default.Send(new OpenWarehousesWindowMessage());
        }

        private void LoadAllWarehouses()
        {
            var allWarehouses = new SvenTechCollection<Warehouse>();
            Warehouses = DataContext.Instance.Warehouses.GetAllWithoutStock().ToSvenTechCollection();
        }

        private SvenTechCollection<Stockyard> LoadStockyards(int WarehouseId)
        {
            var allStockyards = new SvenTechCollection<Stockyard>();
            allStockyards = DataContext.Instance.Stockyards.GetByRefWarehouseId(WarehouseId).ToSvenTechCollection();

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

            DataContext.Instance.Stockyards.Delete(SelectedStockyard.StockyardId);
            SelectedWarehouse.Stockyards.Remove(SelectedStockyard);
            SelectedStockyard = null;
        }

        private void SaveStockyard()
        {
            if (SelectedStockyard.StockyardId != 0)
            {
                DataContext.Instance.Stockyards.Update(SelectedStockyard);
            }
            else
            {
                DataContext.Instance.Stockyards.Insert(SelectedStockyard);
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

        private void ChangeSelectedWarehouse(SelectedWarehouse SelectedWarehouse)
        {
            Warehouses = DataContext.Instance.Warehouses.GetAll().ToSvenTechCollection();
            if (SelectedStockyard == null)
            {
                SelectedStockyard = new Stockyard();
            }
            SelectedStockyard.Warehouse = SelectedWarehouse.Warehouse;
            SelectedStockyard.RefWarehouseId = SelectedWarehouse.Warehouse.WarehouseId;
            RaisePropertyChanged("SelectedStockyard");
        }

        private void GenerateStockyards()
        {
            var newStockyards = new List<Stockyard>();

            var lastStockyard = SelectedWarehouse.Stockyards.LastOrDefault(x =>
                x.Name.ToLower().Contains(Prefix.ToLower()) && x.Name.ToLower().Contains(Suffix.ToLower()));

            var lastNumber = 1;

            if (lastStockyard != null)
            {
                var lastNumberString = lastStockyard.Name.ToLower();
                if (!string.IsNullOrEmpty(Prefix))
                {
                    lastNumberString = lastNumberString.Replace(Prefix.ToLower(), "");
                }

                if (!string.IsNullOrEmpty(Suffix))
                {
                    lastNumberString = lastNumberString.Replace(Suffix.ToLower(), "");
                }

                lastNumber = Convert.ToInt32(lastNumberString) + 1;
            }

            for (var i = 0; i < NumberOfStockyardsToCreate; i++)
            {
                newStockyards.Add(new Stockyard
                { Name = Prefix + lastNumber + Suffix, RefWarehouseId = SelectedWarehouse.WarehouseId });
                lastNumber++;
            }

            DataContext.Instance.Stockyards.Insert(newStockyards);

            var selectedWarehouseId = SelectedWarehouse.WarehouseId;

            LoadAllWarehouses();

            SelectedWarehouse = Warehouses.SingleOrDefault(x => x.WarehouseId == selectedWarehouseId);
        }

        #endregion Methods

        #region Fields

        private Stockyard _SelectedStockyard;

        #endregion Fields

        #region Properties

        public SvenTechCollection<Stockyard> FilteredStockyards { get; set; } = new SvenTechCollection<Stockyard>();
        public DelegateCommand NewStockyardCommand { get; set; }
        public DelegateCommand SaveStockyardCommand { get; set; }
        public DelegateCommand DeleteStockyardCommand { get; set; }
        public DelegateCommand OpenWarehousesWindowCommand { get; set; }
        public User ActualUser => Globals.ActiveUser;
        public StockyardStatusViewModel StockyardStatusViewModel { get; set; } = new StockyardStatusViewModel();
        public SvenTechCollection<Warehouse> Warehouses { get; set; } = new SvenTechCollection<Warehouse>();
        public Warehouse SelectedWarehouse { get; set; }

        public Stockyard SelectedStockyard
        {
            get { return _SelectedStockyard; }
            set
            {
                _SelectedStockyard = value;
                if (value != null)
                {
                    StockyardStatusViewModel.Stockyard = DataContext.Instance.Stockyards.GetStockById(_SelectedStockyard.StockyardId);
                }
                else
                {
                    StockyardStatusViewModel.Stockyard = null;
                }
            }
        }

        #region For stockyard generation

        public DelegateCommand StockyardsGenerationCommand { get; set; }
        public int NumberOfStockyardsToCreate { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }

        #endregion For stockyard generation

        #endregion Properties
    }
}