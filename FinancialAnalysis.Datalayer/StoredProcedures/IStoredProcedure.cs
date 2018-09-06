namespace FinancialAnalysis.Datalayer.StoredProcedures
{
    internal interface IStoredProcedure
    {
        string TableName { get; }

        void CheckAndCreateProcedures();
    }
}