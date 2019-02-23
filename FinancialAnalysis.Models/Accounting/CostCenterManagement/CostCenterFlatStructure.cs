namespace FinancialAnalysis.Models.Accounting.CostCenterManagement
{
    /// <summary>
    /// Flache Sturktur der Kostenstellen für TreeLists
    /// </summary>
    public class CostCenterFlatStructure
    {
        public int Key { get; set; }
        public int ParentKey { get; set; }
        public CostCenterCategory CostCenterCategory { get; set; }
        public CostCenter CostCenter { get; set; }
    }
}