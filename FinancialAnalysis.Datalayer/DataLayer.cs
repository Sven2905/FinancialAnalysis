using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinancialAnalysis.Datalayer.Tables;

namespace FinancialAnalysis.Datalayer
{
    public class DataLayer : IDisposable
    {
        public DataLayer()
        {
            TableVersions.CheckAndCreateStoredProcedures();
            TaxTypes.CheckAndCreateStoredProcedures();
        }

        public TaxTypes TaxTypes { get; set; } = new TaxTypes();
        public TableVersions TableVersions { get; set; } = new TableVersions();

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
