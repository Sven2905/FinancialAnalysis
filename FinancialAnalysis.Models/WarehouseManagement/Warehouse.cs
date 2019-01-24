using DevExpress.Mvvm;
using System.Collections.Generic;

namespace FinancialAnalysis.Models.WarehouseManagement
{
    public class Warehouse : BindableBase
    {
        public int WarehouseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Street { get; set; }
        public int Postcode { get; set; }
        public string City { get; set; }
        public List<Stockyard> Stockyards { get; set; }
    }
}