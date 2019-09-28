using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.ProjectManagement;
using Utilities;
using WebApiWrapper.Administration;
using WebApiWrapper.ProjectManagement;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ProjectWorkingTimeViewModel : ViewModelBase
    {
        public ProjectWorkingTimeViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            LoadData();
            SaveProjectWorkingTimeCommand = new DelegateCommand(SaveSaveProjectWorkingTime, () => Validation());
        }

        public SvenTechCollection<ProjectWorkingTime> ProjectWorkingTimeList { get; set; }
        public SvenTechCollection<User> UserList { get; set; }
        public SvenTechCollection<Project> ProjectList { get; set; }
        public ProjectWorkingTime ProjectWorkingTime { get; set; } = new ProjectWorkingTime();
        public DelegateCommand SaveProjectWorkingTimeCommand { get; set; }

        private void SaveSaveProjectWorkingTime()
        {
            ProjectWorkingTimes.Insert(ProjectWorkingTime);
        }

        private void LoadData()
        {
            LoadProjectWorkingTimes();
            LoadProjects();
            LoadUsers();
        }

        private void LoadUsers()
        {
            UserList = Users.GetAll().ToSvenTechCollection();
        }

        private void LoadProjects()
        {
            ProjectList = Projects.GetAll().ToSvenTechCollection();
        }

        private void LoadProjectWorkingTimes()
        {
            ProjectWorkingTimeList = ProjectWorkingTimes.GetAll().ToSvenTechCollection();
        }

        private bool Validation()
        {
            if (ProjectWorkingTime.RefUserId == 0 || ProjectWorkingTime.RefProjectId == 0 ||
                ProjectWorkingTime.StartTime == ProjectWorkingTime.EndTime)
            {
                return false;
            }

            return true;
        }
    }
}