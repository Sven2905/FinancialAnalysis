using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;

namespace FinancialAnalysis.Logic
{
    public class Globals : ViewModelBase
    {
        public static User ActualUser { get; set; }
        public static CoreData CoreData { get; } = new CoreData();

        //public static TableVersion Versions { get; }

        //public static TableVersion GetTabelVersion()
        //{
        //    DataLayer db = new DataLayer();
        //    return db.TableVersions.GetById(1);
        //}
    }
}