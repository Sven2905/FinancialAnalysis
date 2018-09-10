using System.Collections;

namespace FinancialAnalysis.Datalayer.Tables
{
    public interface ITable
    {
        string TableName { get; }

        void CheckAndCreateTable();
        void CheckAndCreateStoredProcedures();
    }
}