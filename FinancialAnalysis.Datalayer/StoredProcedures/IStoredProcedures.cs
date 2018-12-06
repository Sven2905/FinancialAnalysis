namespace FinancialAnalysis.Datalayer
{
    internal interface IStoredProcedures
    {
        string TableName { get; }

        void CheckAndCreateProcedures();
    }
}