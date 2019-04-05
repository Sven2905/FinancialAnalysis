using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;
using Notifications.Wpf;

namespace FinancialAnalysis.Logic
{
    public class Globals : ViewModelBase
    {
        public static User ActiveUser { get; set; }
        public static CoreData CoreData { get; } = new CoreData();
    }
}