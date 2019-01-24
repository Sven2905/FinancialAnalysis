using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.SalesManagement
{
    public class SalesOrderDetail : BindableBase
    {
        public int SalesOrderDetailId { get; set; }
        public int SalesOrderId { get; set; }
        public SalesOrder SalesOrder { get; set; }
        public int ProductId { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal Total { get; set; }
    }
}
