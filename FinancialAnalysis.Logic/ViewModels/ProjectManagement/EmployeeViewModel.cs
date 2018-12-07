using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class EmployeeViewModel : ViewModelBase
    {
        #region Fields

        #endregion Fields

        #region Constructor

        public EmployeeViewModel()
        {
            using (DataLayer db = new DataLayer())
            {
                Employees = db.Employees.GetAll().ToSvenTechCollection();
            }
        }

        #endregion Constructor

        #region Methods

        #endregion Methods

        #region Properties

        public SvenTechCollection<Employee> Employees { get; set; } = new SvenTechCollection<Employee>();

        #endregion Properties
    }
}
