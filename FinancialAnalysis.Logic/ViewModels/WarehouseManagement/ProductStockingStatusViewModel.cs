using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.ProductManagement;
using FinancialAnalysis.Models.WarehouseManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ProductStockingStatusViewModel : ViewModelBase
    {
        #region Constructor

        public ProductStockingStatusViewModel()
        {
            
        }

        #endregion Constructor

        #region Fields

        private Product _Product;

        #endregion Fields

        #region Properties

        public Product Product
        {
            get { return _Product; }
            set
            {
                _Product = value;
                if (_Product != null)
                {
                FilteredWarehousesFlatStructure = CreateFlatStructure();
                }
                else
                {
                    FilteredWarehousesFlatStructure = null;
                }
            }
        }

        public SvenTechCollection<Warehouse> Warehouses { get; set; }
        public SvenTechCollection<WarehouseStockingFlatStructure> FilteredWarehousesFlatStructure { get; set; }

        #endregion Properties

        #region Methods

        public void Refresh()
        {
            Warehouses = DataContext.Instance.Warehouses.GetByProductId(Product.ProductId).ToSvenTechCollection();
        }

        private SvenTechCollection<WarehouseStockingFlatStructure> CreateFlatStructure()
        {
            Refresh();

            SvenTechCollection<WarehouseStockingFlatStructure> filteredList = new SvenTechCollection<WarehouseStockingFlatStructure>();
            int key = 1;
            int parentKey = 0;
            for (int i = 0; i < Warehouses.Count; i++)
            {
                for (int j = 0; j < Warehouses[i].Stockyards.Count; j++)
                {
                    for (int k = 0; k < Warehouses[i].Stockyards[j].StockedProducts.Count; k++)
                    {
                        if (Warehouses[i].Stockyards[j].StockedProducts[k].RefProductId == Product.ProductId)
                        {
                            var warehouse = filteredList.FirstOrDefault(x => x.Warehouse == Warehouses[i]);
                            if (warehouse == null)
                            {
                                filteredList.Add(new WarehouseStockingFlatStructure() { Id = key, ParentKey = 0, Warehouse = Warehouses[i] });
                                parentKey = key;
                                key++;
                            }

                            var stockyard = filteredList.FirstOrDefault(x => x.Stockyard == Warehouses[i].Stockyards[j]);
                            if (stockyard == null)
                            {
                                filteredList.Add(new WarehouseStockingFlatStructure() { Id = key, ParentKey = parentKey, Stockyard = Warehouses[i].Stockyards[j], Quantity = Warehouses[i].Stockyards[j].StockedProducts[k].Quantity });
                                filteredList.Single(x => x.Id == parentKey).Quantity += Warehouses[i].Stockyards[j].StockedProducts[k].Quantity;
                                key++;
                            }
                        }
                    }
                }
            }

            return filteredList;
        }

        #endregion Methods

    }
}
