using FinancialAnalysis.Models.Accounting;
using System.Collections.Generic;

namespace WebApiWrapper.Accounting
{
    public static class ItemPriceCalculationItems
    {
        private const string controllerName = "ItemPriceCalculationItems";

        public static List<ItemPriceCalculationItem> GetAll()
        {
            return WebApi<List<ItemPriceCalculationItem>>.GetData(controllerName);
        }

        public static ItemPriceCalculationItem GetById(int id)
        {
            return WebApi<ItemPriceCalculationItem>.GetDataById(controllerName, id);
        }

        public static List<ItemPriceCalculationItem> GetByCostCenterAndItemPriceCalculationItemId(int RefCostCenterId, int ItemPriceCalculationItemId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "RefCostCenterId", RefCostCenterId },
                { "ItemPriceCalculationItemId", ItemPriceCalculationItemId },
            };
            return WebApi<List<ItemPriceCalculationItem>>.GetData(controllerName, "GetByCostCenterAndItemPriceCalculationItemId", parameters);
        }

        public static List<ItemPriceCalculationItem> GetByCostCenter(int RefCostCenterId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "RefCostCenterId", RefCostCenterId },
            };
            return WebApi<List<ItemPriceCalculationItem>>.GetData(controllerName, "GetByCostCenter", parameters);
        }

        public static int Insert(ItemPriceCalculationItem ItemPriceCalculationItem)
        {
            return WebApi<int>.PostAsync(controllerName, ItemPriceCalculationItem, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<ItemPriceCalculationItem> ItemPriceCalculationItems)
        {
            return WebApi<int>.PostAsync(controllerName, ItemPriceCalculationItems, "MultiPost").Result;
        }

        public static bool Update(ItemPriceCalculationItem ItemPriceCalculationItem)
        {
            return WebApi<bool>.PutAsync(controllerName, ItemPriceCalculationItem, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}