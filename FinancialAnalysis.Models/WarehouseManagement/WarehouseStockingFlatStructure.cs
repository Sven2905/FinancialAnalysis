using DevExpress.Mvvm;

namespace FinancialAnalysis.Models.WarehouseManagement
{
    public class WarehouseStockingFlatStructure : BaseClass
    {
        public int Id { get; set; }
        public int ParentKey { get; set; }
        public Warehouse Warehouse { get; set; }
        public Stockyard Stockyard { get; set; }
        public int Quantity { get; set; }
    }
}