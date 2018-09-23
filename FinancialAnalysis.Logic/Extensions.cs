using FinancialAnalysis.Models.Accounting;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace FinancialAnalysis.Logic
{
    public static class Extensions
    {
        public static IEnumerable<CostAccountCategory> ToHierachicalCollection<T>(this IEnumerable<CostAccountCategory> collection)
        {
            var categories = (from fc in collection
                              select new CostAccountCategory()
                              {
                                  CostAccountCategoryId = fc.CostAccountCategoryId,
                                  Description = fc.Description,
                                  ParentCategoryId = fc.ParentCategoryId,
                              }).ToList();

            var lookup = categories.ToLookup(c => c.ParentCategoryId);

            foreach (var c in categories)
            {
                // you can skip the check if you want an empty list instead of null
                // when there is no children
                if (lookup.Contains(c.CostAccountCategoryId))
                {
                    c.SubCategories = lookup[c.CostAccountCategoryId].ToList();
                }
            }

            var itemsToRemove = categories.Where(x => x.ParentCategoryId != 0).ToList();

            for (int i = itemsToRemove.Count()-1; i >= 0; i--)
            {
                categories.Remove(itemsToRemove[i]);
            }

            return new SvenTechCollection<CostAccountCategory>(categories);




            SvenTechCollection<CostAccountCategory> finalList = new SvenTechCollection<CostAccountCategory>();
            var ids = collection.Where(x => x.ParentCategoryId == 0).Select(x => x.CostAccountCategoryId);

            finalList.AddRange(collection.Where(x => ids.Contains(x.CostAccountCategoryId)));

            foreach (var item in collection)
            {

            }

            return finalList;
        }
    }
}
