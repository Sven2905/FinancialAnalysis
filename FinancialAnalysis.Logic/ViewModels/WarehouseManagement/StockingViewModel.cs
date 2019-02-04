using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.ProductManagement;
using FinancialAnalysis.Models.WarehouseManagement;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class StockingViewModel : ViewModelBase
    {
        #region Constructur

        public StockingViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            StoreCommand = new DelegateCommand(StoreSelectedProduct, () => SelectedStockyard != null && SelectedProduct != null && Quantity > 0);
            TakeOutCommand = new DelegateCommand(TakeOutSelectedProduct, () => SelectedWarehouseTakeOut != null && SelectedWarehouseTakeOut.Stockyards != null && SelectedProduct != null && QuantityTakeOut > 0);
            Task.Run(() => GetData());
        }

        #endregion Constructur

        #region Fields

        private Product _SelectedProduct;
        private Stockyard _Stockyard;
        private Stockyard _SelectedStockyardTakeOut;

        #endregion Fields

        #region Properties

        public Product SelectedProduct
        {
            get { return _SelectedProduct; }
            set
            {
                _SelectedProduct = value;
                ProductStockingStatusViewModel.Product = _SelectedProduct;
                if (value != null)
                {
                    Refresh();
                }
            }
        }

        public SvenTechCollection<Warehouse> Warehouses { get; set; }
        public SvenTechCollection<Product> Products { get; set; }

        public int MaxValue
        {
            get
            {
                if (SelectedStockyardTakeOut != null && SelectedProduct != null)
                {
                    var tempStockedProduct = SelectedStockyardTakeOut.StockedProducts.SingleOrDefault(x => x.RefProductId == SelectedProduct.ProductId);
                    if (tempStockedProduct != null)
                    {
                        return tempStockedProduct.Quantity;
                    }
                    return 0;
                }
                else
                {
                    return 0;
                }
            }
        }

        public Stockyard SelectedStockyard
        {
            get { return _Stockyard; }
            set
            {
                _Stockyard = value;
                StockyardStatusViewModel.Stockyard = value;
            }
        }


        public Stockyard SelectedStockyardTakeOut
        {
            get { return _SelectedStockyardTakeOut; }
            set
            {
                _SelectedStockyardTakeOut = value;
                TakeOutStockyardStatusViewModel.Stockyard = _SelectedStockyardTakeOut;
            }
        }

        public StockyardStatusViewModel StockyardStatusViewModel { get; set; } = new StockyardStatusViewModel();
        public StockyardStatusViewModel TakeOutStockyardStatusViewModel { get; set; } = new StockyardStatusViewModel();
        public ProductStockingStatusViewModel ProductStockingStatusViewModel { get; set; } = new ProductStockingStatusViewModel();
        public Warehouse SelectedWarehouse { get; set; }
        public SvenTechCollection<WarehouseStockingFlatStructure> FilteredWarehousesFlatStructure { get; set; }
        public SvenTechCollection<Warehouse> FilteredWarehouses { get; set; }
        public Warehouse SelectedWarehouseTakeOut { get; set; }
        public int QuantityTakeOut { get; set; }
        public DelegateCommand StoreCommand { get; set; }
        public DelegateCommand TakeOutCommand { get; set; }
        public int Quantity { get; set; }

        #endregion Properties

        #region Methods

        private void Refresh()
        {
            ProductStockingStatusViewModel.Refresh();
            FilteredWarehouses = CreateFilteredWarehouses();
            TakeOutStockyardStatusViewModel.Stockyard = SelectedStockyardTakeOut;
            StockyardStatusViewModel.Stockyard = SelectedStockyard;
        }

        private void StoreSelectedProduct()
        {
            if (SelectedStockyard != null && SelectedProduct != null && Quantity > 0)
            {
                var stockedProductOnStockyard = SelectedStockyard.StockedProducts.SingleOrDefault(x => x.RefProductId == SelectedProduct.ProductId);

                if (stockedProductOnStockyard != null)
                {
                    stockedProductOnStockyard.Quantity += Quantity;
                    DataContext.Instance.StockedProducts.Update(stockedProductOnStockyard);

                    SaveBookingHistoryEntry(false);
                }
                else
                {
                    var newStockedProduct = new StockedProduct(SelectedProduct, SelectedStockyard.StockyardId, Quantity);
                    DataContext.Instance.StockedProducts.Insert(newStockedProduct);
                    SelectedStockyard.StockedProducts.Add(newStockedProduct);
                    SaveBookingHistoryEntry(false);
                }
                Refresh();
            }
        }

        private void SaveBookingHistoryEntry(bool IsTakeOut)
        {
            WarehouseStockingHistory WarehouseStockingHistory = new WarehouseStockingHistory(SelectedProduct, SelectedStockyard, Quantity, Globals.ActualUser);
            if (IsTakeOut)
            {
                WarehouseStockingHistory.Quantity *= -1;
                WarehouseStockingHistory.RefStockyardId = SelectedStockyardTakeOut.StockyardId;
            }
            DataContext.Instance.WarehouseStockingHistories.Insert(WarehouseStockingHistory);
        }

        private void TakeOutSelectedProduct()
        {
            var stockedProduct = SelectedStockyardTakeOut.StockedProducts.SingleOrDefault(x => x.RefProductId == SelectedProduct.ProductId);
            if (stockedProduct != null)
            {
                var lastWarehouseId = SelectedWarehouseTakeOut.WarehouseId;
                var lastStockyardId = SelectedStockyardTakeOut.StockyardId;
                var lastProductid = SelectedProduct.ProductId;

                if (stockedProduct.Quantity == QuantityTakeOut)
                {

                    DataContext.Instance.StockedProducts.Delete(stockedProduct.StockedProductId);
                    SaveBookingHistoryEntry(true);
                }
                else
                {
                    stockedProduct.Quantity -= QuantityTakeOut;
                    DataContext.Instance.StockedProducts.Update(stockedProduct);
                    SaveBookingHistoryEntry(true);
                }
                GetData();
                Refresh();

                SelectedProduct = Products.SingleOrDefault(x => x.ProductId == lastProductid);

                SelectedWarehouseTakeOut = FilteredWarehouses.SingleOrDefault(x => x.WarehouseId == lastWarehouseId);

                if (SelectedWarehouseTakeOut != null)
                {
                    SelectedStockyardTakeOut = SelectedWarehouseTakeOut.Stockyards.SingleOrDefault(x => x.StockyardId == lastStockyardId);
                }
            }
        }

        private void GetData()
        {
            Products = DataContext.Instance.Products.GetAll().ToSvenTechCollection();
            Warehouses = DataContext.Instance.Warehouses.GetAll().ToSvenTechCollection();
        }



        private SvenTechCollection<Warehouse> CreateFilteredWarehouses()
        {
            if (SelectedProduct == null)
            {
                return null;
            }

            SvenTechCollection<Warehouse> filteredWarehouses = new SvenTechCollection<Warehouse>();

            for (int i = 0; i < Warehouses.Count; i++)
            {
                for (int j = 0; j < Warehouses[i].Stockyards.Count; j++)
                {
                    for (int k = 0; k < Warehouses[i].Stockyards[j].StockedProducts.Count; k++)
                    {
                        if (Warehouses[i].Stockyards[j].StockedProducts[k].RefProductId == SelectedProduct.ProductId)
                        {
                            if (filteredWarehouses.SingleOrDefault(x => x.WarehouseId == Warehouses[i].WarehouseId) == null)
                            {
                                var newWarehouse = Warehouses[i].Clone();
                                var newStockyard = Warehouses[i].Stockyards[j].Clone();
                                newWarehouse.Stockyards = new SvenTechCollection<Stockyard>();
                                newStockyard.StockedProducts = new List<StockedProduct>();
                                newStockyard.StockedProducts.Add(Warehouses[i].Stockyards[j].StockedProducts[k].Clone());
                                newWarehouse.Stockyards.Add(newStockyard);
                                filteredWarehouses.Add(newWarehouse);
                            }
                            else
                            {
                                var warehouse = filteredWarehouses.Single(x => x.WarehouseId == Warehouses[i].WarehouseId);
                                Stockyard stockyard = warehouse.Stockyards.SingleOrDefault(x => x.StockyardId == Warehouses[i].Stockyards[j].StockyardId);
                                if (stockyard == null)
                                {
                                    stockyard = Warehouses[i].Stockyards[j].Clone();
                                    stockyard.StockedProducts = new List<StockedProduct>();
                                    warehouse.Stockyards.Add(stockyard);
                                }
                                stockyard.StockedProducts.Add(Warehouses[i].Stockyards[j].StockedProducts[k].Clone());
                            }
                        }
                    }
                }
            }
            return filteredWarehouses;
        }

        #endregion Methods
    }
}