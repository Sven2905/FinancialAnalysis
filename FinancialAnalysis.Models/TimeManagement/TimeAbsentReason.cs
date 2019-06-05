using DevExpress.Mvvm;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.TimeManagement
{
    [JsonObject(MemberSerialization.OptOut)]
    public class TimeAbsentReason : BindableBase
    {
        public int TimeAbsentReasonId { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public bool IsDeletable { get; set; } = true;
    }
}
