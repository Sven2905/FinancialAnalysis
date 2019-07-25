using FinancialAnalysis.Models.ProjectManagement;
using System.Collections.Generic;

namespace WebApiWrapper.ProjectManagement
{
    public static class Projects
    {
        private const string controllerName = "Projects";

        public static List<Project> GetAll()
        {
            return WebApi<List<Project>>.GetData(controllerName);
        }

        public static Project GetById(int id)
        {
            return WebApi<Project>.GetDataById(controllerName, id);
        }

        public static int Insert(Project Project)
        {
            return WebApi<int>.PostAsync(controllerName, Project, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<Project> Projects)
        {
            return WebApi<int>.PostAsync(controllerName, Projects, "MultiPost").Result;
        }

        public static bool Update(Project Project)
        {
            return WebApi<bool>.PutAsync(controllerName, Project, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}