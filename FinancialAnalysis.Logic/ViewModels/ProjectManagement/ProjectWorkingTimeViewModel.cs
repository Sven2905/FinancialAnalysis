using System;
using System.Windows;
using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.ProjectManagement;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ProjectWorkingTimeViewModel : ViewModelBase
    {
        public ProjectWorkingTimeViewModel()
        {
            if (IsInDesignMode) return;

            LoadData();
            SaveProjectWorkingTimeCommand = new DelegateCommand(SaveSaveProjectWorkingTime, () => Validation());
        }

        public SvenTechCollection<ProjectWorkingTime> ProjectWorkingTimes { get; set; }
        public SvenTechCollection<Employee> Employees { get; set; }
        public SvenTechCollection<Project> Projects { get; set; }
        public ProjectWorkingTime ProjectWorkingTime { get; set; } = new ProjectWorkingTime();
        public DelegateCommand SaveProjectWorkingTimeCommand { get; set; }

        private void SaveSaveProjectWorkingTime()
        {
            DataContext.Instance.ProjectWorkingTimes.Insert(ProjectWorkingTime);
        }

        private void LoadData()
        {
            LoadProjectWorkingTimes();
            LoadProjects();
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            Employees = DataContext.Instance.Employees.GetAll().ToSvenTechCollection();
        }

        private void LoadProjects()
        {
            Projects = DataContext.Instance.Projects.GetAll().ToSvenTechCollection();
        }

        private void LoadProjectWorkingTimes()
        {
            ProjectWorkingTimes = DataContext.Instance.ProjectWorkingTimes.GetAll().ToSvenTechCollection();
        }

        private bool Validation()
        {
            if (ProjectWorkingTime.RefEmployeeId == 0 || ProjectWorkingTime.RefProjectId == 0 ||
                ProjectWorkingTime.StartTime == ProjectWorkingTime.EndTime) return false;

            return true;
        }
    }
}