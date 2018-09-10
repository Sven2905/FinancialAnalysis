namespace FinancialAnalysis.Datalayer.StoredProcedures
{
    internal interface IStoredProcedures
    {
        string TableName { get; }

        void CheckAndCreateProcedures();
    }
}