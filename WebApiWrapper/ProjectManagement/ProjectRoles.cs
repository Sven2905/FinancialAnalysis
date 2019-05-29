using FinancialAnalysis.Models.ProjectManagement;
using System.Collections.Generic;

namespace WebApiWrapper.ProjectManagement
{
    public static class ProjectRoles
    {
        private const string controllerName = "ProjectRoles";

        public static List<ProjectRole> GetAll()
        {
            return WebApi<List<ProjectRole>>.GetData(controllerName);
        }

        public static ProjectRole GetById(int id)
        {
            return WebApi<ProjectRole>.GetDataById(controllerName, id);
        }

        public static int Insert(ProjectRole ProjectRole)
        {
            return WebApi<int>.PostAsync(controllerName, ProjectRole, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<ProjectRole> ProjectRoles)
        {
            return WebApi<int>.PostAsync(controllerName, ProjectRoles, "MultiPost").Result;
        }

        public static bool Update(ProjectRole ProjectRole)
        {
            return WebApi<bool>.PutAsync(controllerName, ProjectRole, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
