﻿using FinancialAnalysis.Models.SalesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.SalesManagement
{
    public static class Shipments
    {
        private const string controllerName = "Shipments";

        public static List<Shipment> GetAll()
        {
            return WebApi<List<Shipment>>.GetData(controllerName);
        }

        public static Shipment GetById(int id)
        {
            return WebApi<Shipment>.GetDataById(controllerName, id);
        }

        public static int Insert(Shipment Shipment)
        {
            return WebApi<int>.PostAsync(controllerName, Shipment).Result;
        }

        public static int Insert(IEnumerable<Shipment> Shipments)
        {
            return WebApi<int>.PostAsync(controllerName, Shipments).Result;
        }

        public static bool Update(Shipment Shipment)
        {
            return WebApi<bool>.PutAsync(controllerName, Shipment, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
