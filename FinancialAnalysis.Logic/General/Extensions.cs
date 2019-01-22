using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.Administration;
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
            var categories = (from fc in collection
                              select new CostAccountCategory
                              {
                                  CostAccountCategoryId = fc.CostAccountCategoryId,
                                  Description = fc.Description,
                                  ParentCategoryId = fc.ParentCategoryId
                              }).ToList();

            var lookup = categories.ToLookup(c => c.ParentCategoryId);

            foreach (var c in categories)
            {
                if (lookup.Contains(c.CostAccountCategoryId))
                {
                    c.SubCategories = lookup[c.CostAccountCategoryId].ToList();
                }
            }

            var itemsToRemove = categories.Where(x => x.ParentCategoryId != 0).ToList();

            for (var i = itemsToRemove.Count() - 1; i >= 0; i--)
            {
                categories.Remove(itemsToRemove[i]);
            }

            return new SvenTechCollection<CostAccountCategory>(categories);
        }

        public static void GrantPermission(this User user, Permission permission)
        {
            using (var db = new DataLayer())
            {
                var right = UserManager.Instance.UserRights.Single(x => x.Permission == permission);
                var tempUserRightUserMapping = user.UserRightUserMappings.SingleOrDefault(x => x.RefUserRightId == right.UserRightId);
                if (tempUserRightUserMapping != null)
                {
                    tempUserRightUserMapping.IsGranted = true;
                    db.UserRightUserMappings.UpdateOrInsert(tempUserRightUserMapping);
                }
                else
                {
                    db.UserRightUserMappings.UpdateOrInsert(new UserRightUserMapping(user.UserId, right.UserRightId, true));
                }
            }
        }

        public static void RevokePermission(this User user, Permission permission)
        {
            using (var db = new DataLayer())
            {
                var right = UserManager.Instance.UserRights.Single(x => x.Permission == permission);
                var tempUserRightUserMapping = user.UserRightUserMappings.SingleOrDefault(x => x.RefUserRightId == right.UserRightId);
                if (tempUserRightUserMapping != null)
                {
                    tempUserRightUserMapping.IsGranted = false;
                    db.UserRightUserMappings.UpdateOrInsert(tempUserRightUserMapping);
                }
                else
                {
                    db.UserRightUserMappings.UpdateOrInsert(new UserRightUserMapping(user.UserId, right.UserRightId, false));
                }
            }
        }
    }
}