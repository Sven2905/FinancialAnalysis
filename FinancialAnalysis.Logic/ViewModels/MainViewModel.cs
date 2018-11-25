using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinancialAnalysis.Logic.ViewModels
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            DataLayer db = new DataLayer();
            db.CreateDatabaseSchema();
            if (db.TaxTypes.GetAll().Count() == 0)
            {
                db.TaxTypes.Seed();

                Import _Import = new Import();
                _Import.ImportCostAccounts(Standardkontenrahmen.SKR03);
            }
        }
    }
}