using DevExpress.Mvvm;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.Accounting.CostCenterManagement
{
    /// <summary>
    /// Flache Sturktur der Kostenstellen für TreeLists
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class CostCenterFlatStructure : BaseClass
    {
        public int Key { get; set; }
        public int ParentKey { get; set; }
        public CostCenterCategory CostCenterCategory { get; set; }
        public CostCenter CostCenter { get; set; }
        public bool IsActive { get; set; }
    }
}