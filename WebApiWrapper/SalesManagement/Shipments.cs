using FinancialAnalysis.Models.SalesManagement;
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

        public static IEnumerable<Shipment> GetAll()
        {
            return WebApi.GetData<IEnumerable<Shipment>>(controllerName);
        }

        public static Shipment GetById(int id)
        {
            return WebApi.GetDataById<Shipment>(controllerName, id);
        }

        public static int Insert(Shipment Shipment)
        {
            return WebApi.PostAsync(controllerName, Shipment).Result;
        }

        public static int Insert(IEnumerable<Shipment> Shipments)
        {
            return WebApi.PostAsync(controllerName, Shipments).Result;
        }

        public static bool Update(Shipment Shipment)
        {
            return WebApi.PutAsync(controllerName, Shipment, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
