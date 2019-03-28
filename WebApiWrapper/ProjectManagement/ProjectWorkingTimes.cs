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

        public static List<ProjectWorkingTime> GetAll()
        {
            return WebApi<List<ProjectWorkingTime>>.GetData(controllerName);
        }

        public static ProjectWorkingTime GetById(int id)
        {
            return WebApi<ProjectWorkingTime>.GetDataById(controllerName, id);
        }

        public static int Insert(ProjectWorkingTime ProjectWorkingTime)
        {
            return WebApi<int>.PostAsync(controllerName, ProjectWorkingTime).Result;
        }

        public static int Insert(IEnumerable<ProjectWorkingTime> ProjectWorkingTimes)
        {
            return WebApi<int>.PostAsync(controllerName, ProjectWorkingTimes).Result;
        }

        public static bool Update(ProjectWorkingTime ProjectWorkingTime)
        {
            return WebApi<bool>.PutAsync(controllerName, ProjectWorkingTime, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
