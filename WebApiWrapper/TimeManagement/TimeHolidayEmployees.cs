﻿using FinancialAnalysis.Models.TimeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.Accounting
{
    public static class TimeHolidayEmployees
    {
        private const string controllerName = "TimeHolidayEmployees";

        public static List<TimeHolidayEmployee> GetAll()
        {
            return WebApi<List<TimeHolidayEmployee>>.GetData(controllerName);
        }

        public static TimeHolidayEmployee GetById(int id)
        {
            return WebApi<TimeHolidayEmployee>.GetDataById(controllerName, id);
        }

        public static int Insert(TimeHolidayEmployee TimeHolidayEmployee)
        {
            return WebApi<int>.PostAsync(controllerName, TimeHolidayEmployee, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<TimeHolidayEmployee> TimeHolidayEmployees)
        {
            return WebApi<int>.PostAsync(controllerName, TimeHolidayEmployees, "MultiPost").Result;
        }

        public static bool Update(TimeHolidayEmployee TimeHolidayEmployee)
        {
            return WebApi<bool>.PutAsync(controllerName, TimeHolidayEmployee, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}