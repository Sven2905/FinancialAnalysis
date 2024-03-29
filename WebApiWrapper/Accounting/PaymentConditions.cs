﻿using FinancialAnalysis.Models.Accounting;
using System.Collections.Generic;

namespace WebApiWrapper.Accounting
{
    public static class PaymentConditions
    {
        private const string controllerName = "PaymentConditions";

        public static List<PaymentCondition> GetAll()
        {
            return WebApi<List<PaymentCondition>>.GetData(controllerName);
        }

        public static PaymentCondition GetById(int id)
        {
            return WebApi<PaymentCondition>.GetDataById(controllerName, id);
        }

        public static int Insert(PaymentCondition PaymentCondition)
        {
            return WebApi<int>.PostAsync(controllerName, PaymentCondition, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<PaymentCondition> PaymentConditions)
        {
            return WebApi<int>.PostAsync(controllerName, PaymentConditions, "MultiPost").Result;
        }

        public static bool Update(PaymentCondition PaymentCondition)
        {
            return WebApi<bool>.PutAsync(controllerName, PaymentCondition, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}