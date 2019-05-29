using DevExpress.Mvvm;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.ProjectManagement;
using System;
using Utilities;
using WebApiWrapper.ProjectManagement;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class TimeBookingViewModel : ViewModelBase
    {
        public TimeBookingViewModel()
        {
            GetData();
        }

        private void GetData()
        {
            ProjectList = Projects.GetAll().ToSvenTechCollection();
        }

        public int RefEmployeeId { get; set; }
        public DateTime BookingTime { get; set; } = DateTime.Now;
        public int SelectedProjectId { get; set; }
        public TimeBookingType TimeBookingType { get; set; }
        public SvenTechCollection<Project> ProjectList { get; private set; }
    }
}
