using FinancialAnalysis.Models.Accounting;
using System.Collections.Generic;

namespace WebApiWrapper.Accounting
{
    public static class ItemPriceCalculationItemCostCenters
    {
        private const string controllerName = "ItemPriceCalculationItemCostCenters";

        public static List<ItemPriceCalculationItemCostCenter> GetAll()
        {
            return WebApi<List<ItemPriceCalculationItemCostCenter>>.GetData(controllerName);
        }

        public static ItemPriceCalculationItemCostCenter GetById(int id)
        {
            return WebApi<ItemPriceCalculationItemCostCenter>.GetDataById(controllerName, id);
        }

        public static int Insert(ItemPriceCalculationItemCostCenter ItemPriceCalculationItemCostCenter)
        {
            return WebApi<int>.PostAsync(controllerName, ItemPriceCalculationItemCostCenter, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<ItemPriceCalculationItemCostCenter> ItemPriceCalculationItemCostCenters)
        {
            return WebApi<int>.PostAsync(controllerName, ItemPriceCalculationItemCostCenters, "MultiPost").Result;
        }

        public static bool Update(ItemPriceCalculationItemCostCenter ItemPriceCalculationItemCostCenter)
        {
            return WebApi<bool>.PutAsync(controllerName, ItemPriceCalculationItemCostCenter, "Put").Result;
        }

        public static bool DeleteByRefItemPriceCalculationItemId(int RefItemPriceCalculationItemId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "RefItemPriceCalculationItemId", RefItemPriceCalculationItemId },
            };
            return WebApi<bool>.GetData(controllerName, "DeleteByRefItemPriceCalculationItemId", parameters);
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}