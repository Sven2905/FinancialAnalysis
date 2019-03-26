﻿using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.Accounting
{
    public static class Credits
    {
        private const string controllerName = "Credits";

        public static IEnumerable<Credit> GetAll()
        {
            return WebApi.GetData<IEnumerable<Credit>>(controllerName);
        }

        public static Credit GetById(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Id", id);
            return WebApi.GetData<Credit>(controllerName, "GetById", parameters);
        }

        public static int Insert(Credit Credit)
        {
            return WebApi.PostAsync(controllerName, Credit).Result;
        }

        public static int Insert(IEnumerable<Credit> Credits)
        {
            return WebApi.PostAsync(controllerName, Credits).Result;
        }
    }
}
