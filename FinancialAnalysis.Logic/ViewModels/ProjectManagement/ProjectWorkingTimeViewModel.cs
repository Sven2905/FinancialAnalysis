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
            try
            {
                DataContext.Instance.ProjectWorkingTimes.Insert(ProjectWorkingTime);
            }
            catch (Exception ex)
            {
                // TODO Exception
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
                Employees = DataContext.Instance.Employees.GetAll().ToSvenTechCollection();
            }
            catch (Exception ex)
            {
                // TODO Exception
            }
        }

        private void LoadProjects()
        {
            try
            {
                Projects = DataContext.Instance.Projects.GetAll().ToSvenTechCollection();
            }
            catch (Exception ex)
            {
                // TODO Exception
            }
        }

        private void LoadProjectWorkingTimes()
        {
            try
            {
                ProjectWorkingTimes = DataContext.Instance.ProjectWorkingTimes.GetAll().ToSvenTechCollection();
            }
            catch (Exception ex)
            {
                // TODO Exception
            }
        }

        private bool Validation()
        {
            if (ProjectWorkingTime.RefEmployeeId == 0 || ProjectWorkingTime.RefProjectId == 0 ||
                ProjectWorkingTime.StartTime == ProjectWorkingTime.EndTime) return false;

            return true;
        }
    }
}