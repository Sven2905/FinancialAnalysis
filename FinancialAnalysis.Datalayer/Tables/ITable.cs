using System.Collections;

namespace FinancialAnalysis.Datalayer
{
    public interface ITable
    {
        string TableName { get; }

        void CheckAndCreateTable();
        void CheckAndCreateStoredProcedures();
    }
}