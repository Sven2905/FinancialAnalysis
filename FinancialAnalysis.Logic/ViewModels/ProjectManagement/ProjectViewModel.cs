using System;
using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.ProjectManagement;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ProjectViewModel : ViewModelBase
    {
        #region Constructor

        public ProjectViewModel()
        {
            LoadProjects();
            LoadEmployees();
            LoadCostCenters();
            NewProjectCommand = new DelegateCommand(NewProject);
            SaveProjectCommand = new DelegateCommand(SaveProject, () => Validation());
            DeleteProjectCommand = new DelegateCommand(DeleteProject, () => (SelectedProject != null));
        }

        #endregion Constructor

        #region Fields

        #endregion Fields

        #region Properties

        public SvenTechCollection<Project> Projects { get; set; }
        public SvenTechCollection<Employee> Employees { get; set; }
        public SvenTechCollection<CostCenter> CostCenters { get; set; }
        public Employee SelectedLeader { get; set; }
        public DelegateCommand NewProjectCommand { get; set; }
        public DelegateCommand SaveProjectCommand { get; set; }
        public DelegateCommand DeleteProjectCommand { get; set; }
        public Project SelectedProject { get; set; }

        #endregion Properties

        #region Methods

        private void LoadProjects()
        {
            try
            {
                using (var db = new DataLayer())
                {
                    Projects = db.Projects.GetAll().ToSvenTechCollection();
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void LoadCostCenters()
        {
            try
            {
                using (var db = new DataLayer())
                {
                    CostCenters = db.CostCenters.GetAll().ToSvenTechCollection();
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void LoadEmployees()
        {
            try
            {
                using (var db = new DataLayer())
                {
                    Employees = db.Employees.GetAll().ToSvenTechCollection();
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void NewProject()
        {
            SelectedProject = new Project();
            Projects.Add(SelectedProject);
        }

        private void DeleteProject()
        {
            if (SelectedProject == null)
            {
                return;
            }

            if (SelectedProject.ProjectId == 0)
            {
                Projects.Remove(SelectedProject);
                SelectedProject = null;
                return;
            }

            try
            {
                using (var db = new DataLayer())
                {
                    db.CostCenters.Delete(SelectedProject.ProjectId);
                    Projects.Remove(SelectedProject);
                    SelectedProject = null;
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void SaveProject()
        {
            try
            {
                if (SelectedProject.ProjectId != 0)
                {
                    using (var db = new DataLayer())
                    {
                        db.Projects.Update(SelectedProject);
                    }
                }
                else
                {
                    using (var db = new DataLayer())
                    {
                        db.Projects.Insert(SelectedProject);
                    }
                }

            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private bool Validation()
        {
            if (SelectedProject == null || SelectedProject.RefCostCenterId == 0 || string.IsNullOrEmpty(SelectedProject.Name))
            {
                return false;
            }
            return true;
        }

        public TimeSpan CalculateWorkHours(DateTime StartTime, DateTime EndTime)
        {
            var WorkTime = StartTime - EndTime;

            if (WorkTime.TotalHours > 6 && WorkTime.TotalHours < new TimeSpan(6, 30, 0).TotalHours)
            {
                var requiredBreakTime = WorkTime - new TimeSpan(6, 0, 0);

                return WorkTime - requiredBreakTime;
            }

            if (WorkTime.TotalHours > 6 && WorkTime.TotalHours < 9)
            {
                var requiredBreakTime = new TimeSpan(0, 30, 0);

                return WorkTime - requiredBreakTime;
            }

            if (WorkTime.TotalHours > 9 && WorkTime.TotalHours < new TimeSpan(9, 15, 0).TotalHours)
            {
                var requiredBreakTime = WorkTime - new TimeSpan(6, 0, 0);

                return WorkTime - requiredBreakTime;
            }

            if (WorkTime.TotalHours > 9)
            {
                var requiredBreakTime = new TimeSpan(0, 45, 0);

                return WorkTime - requiredBreakTime;
            }

            return WorkTime;
        }

        #endregion Methods

    }
}