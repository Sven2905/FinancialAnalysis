﻿using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.Accounting.CostCenterManagement;
using System.Collections.Generic;

namespace WebApiWrapper.Accounting
{
    public static class CostCenterBudgets
    {
        private const string controllerName = "CostCenterBudgets";

        public static List<CostCenterBudget> GetAll()
        {
            return WebApi<List<CostCenterBudget>>.GetData(controllerName);
        }

        public static CostCenterBudget GetById(int id)
        {
            return WebApi<CostCenterBudget>.GetDataById(controllerName, id);
        }

        public static List<CostCenterCurrentCosts> GetAnnuallyCosts(int RefCostCenterId, int Year)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "RefCostCenterId", RefCostCenterId },
                { "Year", Year }
            };
            return WebApi<List<CostCenterCurrentCosts>>.GetData(controllerName, "GetAnnuallyCosts", parameters);
        }

        public static int Insert(CostCenterBudget CostCenterBudget)
        {
            return WebApi<int>.PostAsync(controllerName, CostCenterBudget, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<CostCenterBudget> CostCenterBudgets)
        {
            return WebApi<int>.PostAsync(controllerName, CostCenterBudgets, "MultiPost").Result;
        }

        public static bool Update(CostCenterBudget CostCenterBudget)
        {
            return WebApi<bool>.PutAsync(controllerName, CostCenterBudget, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}