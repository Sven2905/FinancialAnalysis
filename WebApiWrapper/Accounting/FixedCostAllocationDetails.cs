﻿using FinancialAnalysis.Models.Accounting;
using System.Collections.Generic;

namespace WebApiWrapper.Accounting
{
    public static class FixedCostAllocationDetails
    {
        private const string controllerName = "FixedCostAllocationDetails";

        public static List<FixedCostAllocationDetail> GetAll()
        {
            return WebApi<List<FixedCostAllocationDetail>>.GetData(controllerName);
        }

        public static FixedCostAllocationDetail GetById(int id)
        {
            return WebApi<FixedCostAllocationDetail>.GetDataById(controllerName, id);
        }

        public static int Insert(FixedCostAllocationDetail FixedCostAllocationDetail)
        {
            return WebApi<int>.PostAsync(controllerName, FixedCostAllocationDetail, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<FixedCostAllocationDetail> FixedCostAllocationDetails)
        {
            return WebApi<int>.PostAsync(controllerName, FixedCostAllocationDetails, "MultiPost").Result;
        }

        public static bool Update(FixedCostAllocationDetail FixedCostAllocationDetail)
        {
            return WebApi<bool>.PutAsync(controllerName, FixedCostAllocationDetail, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}