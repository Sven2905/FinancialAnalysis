using DevExpress.Mvvm;

using FinancialAnalysis.Models.ProductManagement;
using FinancialAnalysis.Models.WarehouseManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using WebApiWrapper.WarehouseManagement;

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

        public SvenTechCollection<Warehouse> WarehouseList { get; set; }
        public SvenTechCollection<WarehouseStockingFlatStructure> FilteredWarehousesFlatStructure { get; set; }

        #endregion Properties

        #region Methods

        public void Refresh()
        {
            WarehouseList = Warehouses.GetByProductId(Product.ProductId).ToSvenTechCollection();
        }

        private SvenTechCollection<WarehouseStockingFlatStructure> CreateFlatStructure()
        {
            Refresh();

            SvenTechCollection<WarehouseStockingFlatStructure> filteredList = new SvenTechCollection<WarehouseStockingFlatStructure>();
            int key = 1;
            int parentKey = 0;
            for (int i = 0; i < WarehouseList.Count; i++)
            {
                for (int j = 0; j < WarehouseList[i].Stockyards.Count; j++)
                {
                    for (int k = 0; k < WarehouseList[i].Stockyards[j].StockedProducts.Count; k++)
                    {
                        if (WarehouseList[i].Stockyards[j].StockedProducts[k].RefProductId == Product.ProductId)
                        {
                            var warehouse = filteredList.FirstOrDefault(x => x.Warehouse == WarehouseList[i]);
                            if (warehouse == null)
                            {
                                filteredList.Add(new WarehouseStockingFlatStructure() { Id = key, ParentKey = 0, Warehouse = WarehouseList[i] });
                                parentKey = key;
                                key++;
                            }

                            var stockyard = filteredList.FirstOrDefault(x => x.Stockyard == WarehouseList[i].Stockyards[j]);
                            if (stockyard == null)
                            {
                                filteredList.Add(new WarehouseStockingFlatStructure() { Id = key, ParentKey = parentKey, Stockyard = WarehouseList[i].Stockyards[j], Quantity = WarehouseList[i].Stockyards[j].StockedProducts[k].Quantity });
                                filteredList.Single(x => x.Id == parentKey).Quantity += WarehouseList[i].Stockyards[j].StockedProducts[k].Quantity;
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
