using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ProjectWorkingTimeViewModel : ViewModelBase
    {
        public ProjectWorkingTimeViewModel()
        {
            if (IsInDesignMode)
                return;

            LoadData();
            SaveProjectWorkingTimeCommand = new DelegateCommand(SaveSaveProjectWorkingTime, () => Validation());
        }

        private void SaveSaveProjectWorkingTime()
        {
            try
            {
                DataLayer.Instance.ProjectWorkingTimes.Insert(ProjectWorkingTime);
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void LoadData()
        {
            LoadProjectWorkingTimes();
            LoadProjects();
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            try
            {
                Employees = DataLayer.Instance.Employees.GetAll().ToSvenTechCollection();
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void LoadProjects()
        {
            try
            {
                Projects = DataLayer.Instance.Projects.GetAll().ToSvenTechCollection();
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void LoadProjectWorkingTimes()
        {
            try
            {
                ProjectWorkingTimes = DataLayer.Instance.ProjectWorkingTimes.GetAll().ToSvenTechCollection();
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private bool Validation()
        {
            if (ProjectWorkingTime.RefEmployeeId == 0 || ProjectWorkingTime.RefProjectId == 0 || ProjectWorkingTime.StartTime == ProjectWorkingTime.EndTime)
            {
                return false;
            }

            return true;
        }

        public SvenTechCollection<ProjectWorkingTime> ProjectWorkingTimes { get; set; }
        public SvenTechCollection<Employee> Employees { get; set; }
        public SvenTechCollection<Project> Projects { get; set; }
        public ProjectWorkingTime ProjectWorkingTime { get; set; } = new ProjectWorkingTime();
        public DelegateCommand SaveProjectWorkingTimeCommand { get; set; }
    }
}
