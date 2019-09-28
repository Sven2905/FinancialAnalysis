using DevExpress.Mvvm;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.ProjectManagement;
using System;
using Utilities;
using WebApiWrapper.Accounting;
using WebApiWrapper.Administration;
using WebApiWrapper.ProjectManagement;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ProjectViewModel : ViewModelBase
    {
        #region Constructor

        public ProjectViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            LoadProjects();
            LoadUsers();
            LoadCostCenters();
            NewProjectCommand = new DelegateCommand(NewProject);
            SaveProjectCommand = new DelegateCommand(SaveProject, () => Validation());
            DeleteProjectCommand = new DelegateCommand(DeleteProject, () => SelectedProject != null);
        }

        #endregion Constructor

        #region Properties

        public SvenTechCollection<Project> ProjectList { get; set; }
        public SvenTechCollection<User> UserList { get; set; }
        public SvenTechCollection<CostCenter> CostCenterList { get; set; }
        public User SelectedLeader { get; set; }
        public DelegateCommand NewProjectCommand { get; set; }
        public DelegateCommand SaveProjectCommand { get; set; }
        public DelegateCommand DeleteProjectCommand { get; set; }
        public Project SelectedProject { get; set; }

        #endregion Properties

        #region Methods

        private void LoadProjects()
        {
            ProjectList = Projects.GetAll().ToSvenTechCollection();
        }

        private void LoadCostCenters()
        {
            CostCenterList = CostCenters.GetAll().ToSvenTechCollection();
        }

        private void LoadUsers()
        {
            UserList = Users.GetAll().ToSvenTechCollection();
        }

        private void NewProject()
        {
            SelectedProject = new Project();
            ProjectList.Add(SelectedProject);
        }

        private void DeleteProject()
        {
            if (SelectedProject == null)
            {
                return;
            }

            if (SelectedProject.ProjectId == 0)
            {
                ProjectList.Remove(SelectedProject);
                SelectedProject = null;
                return;
            }

            CostCenters.Delete(SelectedProject.ProjectId);
            ProjectList.Remove(SelectedProject);
            SelectedProject = null;
        }

        private void SaveProject()
        {
            if (SelectedProject.ProjectId != 0)
            {
                Projects.Update(SelectedProject);
            }
            else
            {
                Projects.Insert(SelectedProject);
            }

            SelectedProject = new Project();
        }

        private bool Validation()
        {
            if (SelectedProject == null ||
                SelectedProject.RefCostCenterId == 0 ||
                string.IsNullOrEmpty(SelectedProject.Name
                ))
            {
                return false;
            }

            return true;
        }

        public TimeSpan CalculateWorkHours(DateTime StartTime, DateTime EndTime)
        {
            TimeSpan WorkTime = StartTime - EndTime;

            if (WorkTime.TotalHours > 6 && WorkTime.TotalHours < new TimeSpan(6, 30, 0).TotalHours)
            {
                TimeSpan requiredBreakTime = WorkTime - new TimeSpan(6, 0, 0);

                return WorkTime - requiredBreakTime;
            }

            if (WorkTime.TotalHours > 6 && WorkTime.TotalHours < 9)
            {
                TimeSpan requiredBreakTime = new TimeSpan(0, 30, 0);

                return WorkTime - requiredBreakTime;
            }

            if (WorkTime.TotalHours > 9 && WorkTime.TotalHours < new TimeSpan(9, 15, 0).TotalHours)
            {
                TimeSpan requiredBreakTime = WorkTime - new TimeSpan(6, 0, 0);

                return WorkTime - requiredBreakTime;
            }

            if (WorkTime.TotalHours > 9)
            {
                TimeSpan requiredBreakTime = new TimeSpan(0, 45, 0);

                return WorkTime - requiredBreakTime;
            }

            return WorkTime;
        }

        #endregion Methods
    }
}