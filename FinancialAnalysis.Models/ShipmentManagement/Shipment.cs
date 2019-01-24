using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.ShipmentManagement
{
    public class Shipment : BindableBase
    {
        public int ShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public int SalesOrderId { get; set; }
        public DateTime ShipmentDate { get; set; }
        public int ShipmentTypeId { get; set; }
        public int StockyardId { get; set; }
        public bool IsFullShipment { get; set; } = true;
    }
}
