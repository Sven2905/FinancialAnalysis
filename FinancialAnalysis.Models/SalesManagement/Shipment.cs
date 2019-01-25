using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.SalesManagement
{
    public class Shipment : BindableBase
    {
        public int ShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public int RefSalesOrderId { get; set; }
        public DateTime ShipmentDate { get; set; }
        public int ShipmentTypeId { get; set; }
        public ShipmentType ShipmentType { get; set; }
    }
}
