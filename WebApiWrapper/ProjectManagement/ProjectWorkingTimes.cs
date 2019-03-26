using FinancialAnalysis.Models.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.ProjectManagement
{
    public static class ProjectWorkingTimes
    {
        private const string controllerName = "ProjectWorkingTimes";

        public static IEnumerable<ProjectWorkingTime> GetAll()
        {
            return WebApi.GetData<IEnumerable<ProjectWorkingTime>>(controllerName);
        }

        public static ProjectWorkingTime GetById(int id)
        {
            return WebApi.GetDataById<ProjectWorkingTime>(controllerName, id);
        }

        public static int Insert(ProjectWorkingTime ProjectWorkingTime)
        {
            return WebApi.PostAsync(controllerName, ProjectWorkingTime).Result;
        }

        public static int Insert(IEnumerable<ProjectWorkingTime> ProjectWorkingTimes)
        {
            return WebApi.PostAsync(controllerName, ProjectWorkingTimes).Result;
        }

        public static bool Update(ProjectWorkingTime ProjectWorkingTime)
        {
            return WebApi.PutAsync(controllerName, ProjectWorkingTime, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
