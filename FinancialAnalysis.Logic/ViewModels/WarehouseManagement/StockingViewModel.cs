using DevExpress.Mvvm;

using FinancialAnalysis.Models.ProductManagement;
using FinancialAnalysis.Models.WarehouseManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using WebApiWrapper.ProductManagement;
using WebApiWrapper.WarehouseManagement;

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
        private int _Quantity;
        private int _QuantityTakeOut;

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
                RefreshLastBookings();
            }
        }

        private void RefreshLastBookings()
        {
            if (_SelectedProduct == null)
            {
                LastBookingViewModel.RefProductId = 0;
                return;
            }

            LastBookingViewModel.RefProductId = _SelectedProduct.ProductId;

            if (IsTakeOut)
            {
                if (_SelectedStockyardTakeOut != null)
                    LastBookingViewModel.RefStockyardId = _SelectedStockyardTakeOut.StockyardId;
                else
                    LastBookingViewModel.RefStockyardId = 0;
            }
            else
            {
                if (_Stockyard != null)
                    LastBookingViewModel.RefStockyardId = _Stockyard.StockyardId;
                else
                    LastBookingViewModel.RefStockyardId = 0;
            }
        }

        public SvenTechCollection<Warehouse> WarehouseList { get; set; }
        public SvenTechCollection<Product> ProductList { get; set; }

        public int MaxValue
        {
            get
            {
                if (SelectedStockyardTakeOut != null && SelectedProduct != null)
                {
                    var tempStockedProduct = SelectedStockyardTakeOut.StockedProducts.SingleOrDefault(x => x.RefProductId == SelectedProduct.ProductId);
                    if (tempStockedProduct != null)
                        return tempStockedProduct.Quantity;
                }
                return 0;
            }
        }

        public Stockyard SelectedStockyard
        {
            get { return _Stockyard; }
            set
            {
                _Stockyard = value;
                if (value != null)
                {
                    StockyardStatusViewModel.Stockyard = Stockyards.GetStockById(SelectedStockyard.StockyardId);
                }
                else
                {
                    StockyardStatusViewModel.Stockyard = null;
                }

                if (value != null && SelectedProduct != null)
                {
                    LastBookingViewModel.RefProductId = SelectedProduct.ProductId;
                    LastBookingViewModel.RefStockyardId = value.StockyardId;
                }
            }
        }

        public Stockyard SelectedStockyardTakeOut
        {
            get { return _SelectedStockyardTakeOut; }
            set
            {
                _SelectedStockyardTakeOut = value;
                if (_SelectedStockyardTakeOut != null)
                {
                    //TakeOutStockyardStatusViewModel.Stockyard = Warehouses.Single(x => x.WarehouseId == _SelectedStockyardTakeOut.RefWarehouseId).Stockyards.Single(x => x.StockyardId == _SelectedStockyardTakeOut.StockyardId);
                    TakeOutStockyardStatusViewModel.Stockyard = Stockyards.GetStockById(value.StockyardId);
                }
                else
                {
                    TakeOutStockyardStatusViewModel.Stockyard = null;
                }
                if (value != null && SelectedProduct != null)
                {
                    LastBookingViewModel.RefProductId = SelectedProduct.ProductId;
                    LastBookingViewModel.RefStockyardId = value.StockyardId;
                }
            }
        }

        public int Quantity
        {
            get { return _Quantity; }
            set
            {
                if (value < 1)
                {
                    _Quantity = 1;
                }
                else
                {
                    _Quantity = value;
                }
            }
        }

        public int QuantityTakeOut
        {
            get { return _QuantityTakeOut; }
            set
            {
                if (value < 1)
                {
                    _QuantityTakeOut = 1;
                }
                else if (value > MaxValue)
                {
                    _QuantityTakeOut = MaxValue;
                }
                else
                {
                    _QuantityTakeOut = value;
                }
            }
        }

        public bool IsTakeOut { get; set; } = false;
        public StockyardStatusViewModel StockyardStatusViewModel { get; set; } = new StockyardStatusViewModel();
        public StockyardStatusViewModel TakeOutStockyardStatusViewModel { get; set; } = new StockyardStatusViewModel();
        public ProductStockingStatusViewModel ProductStockingStatusViewModel { get; set; } = new ProductStockingStatusViewModel();
        public LastBookingViewModel LastBookingViewModel { get; set; } = new LastBookingViewModel();
        public Warehouse SelectedWarehouse { get; set; }
        //public SvenTechCollection<WarehouseStockingFlatStructure> FilteredWarehousesFlatStructure { get; set; }
        public SvenTechCollection<Warehouse> FilteredWarehouses { get; set; }
        public Warehouse SelectedWarehouseTakeOut { get; set; }
        public DelegateCommand StoreCommand { get; set; }
        public DelegateCommand TakeOutCommand { get; set; }

        #endregion Properties

        #region Methods

        private void Refresh()
        {
            ProductStockingStatusViewModel.Refresh();
            FilteredWarehouses = Warehouses.GetByProductId(_SelectedProduct.ProductId).ToSvenTechCollection();
        }

        private void StoreSelectedProduct()
        {
            if (SelectedStockyard != null && SelectedProduct != null && Quantity > 0)
            {
                var stockedProductOnStockyard = StockedProducts.GetByRefProductIdAndRefStockyardId(SelectedProduct.ProductId, SelectedStockyard.StockyardId);
                //var stockedProductOnStockyard = SelectedStockyard.StockedProducts.SingleOrDefault(x => x.RefProductId == SelectedProduct.ProductId);

                if (stockedProductOnStockyard != null)
                {
                    stockedProductOnStockyard.Quantity += Quantity;
                    StockedProducts.Update(stockedProductOnStockyard);

                    SaveBookingHistoryEntry();
                }
                else
                {
                    var newStockedProduct = new StockedProduct(SelectedProduct, SelectedStockyard.StockyardId, Quantity);
                    StockedProducts.Insert(newStockedProduct);
                    SaveBookingHistoryEntry();
                }
                Refresh();
                LastBookingViewModel.LoadData();
                StockyardStatusViewModel.Stockyard = null;
                StockyardStatusViewModel.Stockyard = Stockyards.GetStockById(SelectedStockyard.StockyardId);
                ProductStockingStatusViewModel.Product = null;
                ProductStockingStatusViewModel.Product = _SelectedProduct;
            }
        }

        private void SaveBookingHistoryEntry()
        {
            WarehouseStockingHistory WarehouseStockingHistory = new WarehouseStockingHistory(SelectedProduct, SelectedStockyard, Quantity, Globals.ActiveUser);
            if (IsTakeOut)
            {
                WarehouseStockingHistory.Quantity = QuantityTakeOut * (-1);
                WarehouseStockingHistory.RefStockyardId = SelectedStockyardTakeOut.StockyardId;
            }
            WarehouseStockingHistories.Insert(WarehouseStockingHistory);
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
                    StockedProducts.Delete(stockedProduct.StockedProductId);
                    SaveBookingHistoryEntry();
                }
                else
                {
                    stockedProduct.Quantity -= QuantityTakeOut;
                    StockedProducts.Update(stockedProduct);
                    SaveBookingHistoryEntry();
                }
                GetData();

                SelectedProduct = ProductList.SingleOrDefault(x => x.ProductId == lastProductid);
                Refresh();

                SelectedWarehouseTakeOut = FilteredWarehouses.SingleOrDefault(x => x.WarehouseId == lastWarehouseId);

                if (SelectedWarehouseTakeOut != null)
                {
                    SelectedStockyardTakeOut = SelectedWarehouseTakeOut.Stockyards.SingleOrDefault(x => x.StockyardId == lastStockyardId);
                }

                TakeOutStockyardStatusViewModel.Stockyard = null;
                TakeOutStockyardStatusViewModel.Stockyard = Stockyards.GetStockById(SelectedStockyardTakeOut.StockyardId);
                ProductStockingStatusViewModel.Product = null;
                ProductStockingStatusViewModel.Product = _SelectedProduct;
            }
        }

        private void GetData()
        {
            ProductList = Products.GetAll().ToSvenTechCollection();
            WarehouseList = Warehouses.GetAllWithoutStock().ToSvenTechCollection();
        }



        //private SvenTechCollection<Warehouse> CreateFilteredWarehouses()
        //{
        //    if (SelectedProduct == null)
        //    {
        //        return null;
        //    }

        //    SvenTechCollection<Warehouse> filteredWarehouses = new SvenTechCollection<Warehouse>();

        //    for (int i = 0; i < Warehouses.Count; i++)
        //    {
        //        for (int j = 0; j < Warehouses[i].Stockyards.Count; j++)
        //        {
        //            for (int k = 0; k < Warehouses[i].Stockyards[j].StockedProducts.Count; k++)
        //            {
        //                if (Warehouses[i].Stockyards[j].StockedProducts[k].RefProductId == SelectedProduct.ProductId)
        //                {
        //                    if (filteredWarehouses.SingleOrDefault(x => x.WarehouseId == Warehouses[i].WarehouseId) == null)
        //                    {
        //                        var newWarehouse = Warehouses[i].Clone();
        //                        var newStockyard = Warehouses[i].Stockyards[j].Clone();
        //                        newWarehouse.Stockyards = new SvenTechCollection<Stockyard>();
        //                        newStockyard.StockedProducts = new List<StockedProduct>();
        //                        newStockyard.StockedProducts.Add(Warehouses[i].Stockyards[j].StockedProducts[k].Clone());
        //                        newWarehouse.Stockyards.Add(newStockyard);
        //                        filteredWarehouses.Add(newWarehouse);
        //                    }
        //                    else
        //                    {
        //                        var warehouse = filteredWarehouses.Single(x => x.WarehouseId == Warehouses[i].WarehouseId);
        //                        Stockyard stockyard = warehouse.Stockyards.SingleOrDefault(x => x.StockyardId == Warehouses[i].Stockyards[j].StockyardId);
        //                        if (stockyard == null)
        //                        {
        //                            stockyard = Warehouses[i].Stockyards[j].Clone();
        //                            stockyard.StockedProducts = new List<StockedProduct>();
        //                            warehouse.Stockyards.Add(stockyard);
        //                        }
        //                        stockyard.StockedProducts.Add(Warehouses[i].Stockyards[j].StockedProducts[k].Clone());
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return filteredWarehouses;
        //}

        #endregion Methods
    }
}