using DevExpress.Mvvm;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.BaseClasses;
using FinancialAnalysis.Models.ProductManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.SalesManagement
{
    public class SalesOrderPosition : OrderPositionItem
    {
        public int SalesOrderPositionId { get; set; }
        public int RefSalesOrderId { get; set; }
        public bool IsShipped { get; set; }
    }
}
