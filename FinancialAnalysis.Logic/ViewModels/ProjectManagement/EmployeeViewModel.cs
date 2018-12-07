using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.ProjectManagement;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class EmployeeViewModel : ViewModelBase
    {
        #region Constructor

        public EmployeeViewModel()
        {
            using (var db = new DataLayer())
            {
                Employees = db.Employees.GetAll().ToSvenTechCollection();
            }
        }

        #endregion Constructor

        #region Properties

        public SvenTechCollection<Employee> Employees { get; set; } = new SvenTechCollection<Employee>();

        #endregion Properties

        #region Fields

        #endregion Fields

        #region Methods

        #endregion Methods
    }
}