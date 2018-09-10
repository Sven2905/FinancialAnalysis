using FinancialAnalysis.Datalayer.Tables;
using System;

namespace FinancialAnalysis.Datalayer
{
    public class DataLayer : IDisposable
    {
        public DataLayer(bool CheckForUpdates = false)
        {
            if (CheckForUpdates)
            {
                TableVersions.CheckAndCreateStoredProcedures();
                TaxTypes.CheckAndCreateStoredProcedures();
                Companies.CheckAndCreateStoredProcedures();
                CostAccountCategories.CheckAndCreateStoredProcedures();
                CostAccounts.CheckAndCreateStoredProcedures();
            }
        }

        public TaxTypes TaxTypes { get; set; } = new TaxTypes();
        public TableVersions TableVersions { get; set; } = new TableVersions();
        public Companies Companies { get; set; } = new Companies();
        public CostAccountCategories CostAccountCategories { get; set; } = new CostAccountCategories();
        public CostAccounts CostAccounts { get; set; } = new CostAccounts();

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
