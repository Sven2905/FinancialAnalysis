using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.General;
using System.Collections.Generic;

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