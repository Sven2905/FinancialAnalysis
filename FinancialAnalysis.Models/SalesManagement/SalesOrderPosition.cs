using DevExpress.Mvvm;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.ProductManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.SalesManagement
{
    public class SalesOrderPosition : BindableBase
    {
        public int SalesOrderPositionId { get; set; }
        public int RefSalesOrderId { get; set; }
        public int RefProductId { get; set; }
        public Product Product { get; set; }
        public int RefTaxTypeId { get; set; }
        public TaxType TaxType { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPercentage { get; set; }
        public bool IsShipped { get; set; }
        public bool IsCanceled { get; set; }
    }
}
