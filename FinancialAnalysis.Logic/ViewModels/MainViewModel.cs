using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Administration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.ViewModels
{
    /// <summary>
    ///     This class contains properties that the main View can data bind to.
    ///     <para>
    ///         Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    ///     </para>
    ///     <para>
    ///         You can also use Blend to data bind with the tool's support.
    ///     </para>
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public User ActualUser { get; set; }
        public string CurrentTime
        {
            get { return _currentTime; }
            set { _currentTime = value; RaisePropertiesChanged(); }
        }

        private string _currentTime;

        /// <summary>
        ///     Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ///
            ActualUser = Globals.ActualUser;

            var db = new DataLayer();
            db.CreateDatabaseSchema();
            if (db.TaxTypes.GetAll().Count() == 0)
            {
                db.TaxTypes.Seed();

                var _Import = new Import();
                _Import.ImportCostAccounts(Standardkontenrahmen.SKR03);
            }
            UpdateTime();
        }

        private void UpdateTime()
        {
            Task.Run(() =>
            {
                CurrentTime = DateTime.Now.ToString("G");
                Task.Delay(1000);
                UpdateTime();
            });
        }
    }
}