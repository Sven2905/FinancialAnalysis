using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.SalesManagement
{
    public class SalesOrder : BindableBase
    {
        public int SalesOrderId { get; set; }
        public string Name { get; set; }
        public int RefDebitorId { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public DateTimeOffset DeliveryDate { get; set; }
        public int RefSalesTypeId { get; set; }
        public string Remarks { get; set; } // Bemerkung
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public List<SalesOrderPosition> SalesOrderPositions { get; set; } = new List<SalesOrderPosition>();
    }
}
