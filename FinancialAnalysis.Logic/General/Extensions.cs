using FinancialAnalysis.Models.Accounting;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace FinancialAnalysis.Logic
{
    public static class Extensions
    {
        public static IEnumerable<CostAccountCategory> ToHierachicalCollection<T>(
            this IEnumerable<CostAccountCategory> collection)
        {
            List<CostAccountCategory> categories = (from fc in collection
                                                    select new CostAccountCategory
                                                    {
                                                        CostAccountCategoryId = fc.CostAccountCategoryId,
                                                        Description = fc.Description,
                                                        ParentCategoryId = fc.ParentCategoryId
                                                    }).ToList();

            ILookup<int, CostAccountCategory> lookup = categories.ToLookup(c => c.ParentCategoryId);

            foreach (CostAccountCategory c in categories)
            {
                if (lookup.Contains(c.CostAccountCategoryId))
                {
                    c.SubCategories = lookup[c.CostAccountCategoryId].ToList();
                }
            }

            List<CostAccountCategory> itemsToRemove = categories.Where(x => x.ParentCategoryId != 0).ToList();

            for (int i = itemsToRemove.Count() - 1; i >= 0; i--)
            {
                categories.Remove(itemsToRemove[i]);
            }

            return new SvenTechCollection<CostAccountCategory>(categories);
        }
    }
}