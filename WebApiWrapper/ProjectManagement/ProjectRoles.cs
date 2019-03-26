using FinancialAnalysis.Models.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.ProjectManagement
{
    public static class ProjectRoles
    {
        private const string controllerName = "ProjectRoles";

        public static IEnumerable<ProjectRole> GetAll()
        {
            return WebApi.GetData<IEnumerable<ProjectRole>>(controllerName);
        }

        public static ProjectRole GetById(int id)
        {
            return WebApi.GetDataById<ProjectRole>(controllerName, id);
        }

        public static int Insert(ProjectRole ProjectRole)
        {
            return WebApi.PostAsync(controllerName, ProjectRole).Result;
        }

        public static int Insert(IEnumerable<ProjectRole> ProjectRoles)
        {
            return WebApi.PostAsync(controllerName, ProjectRoles).Result;
        }

        public static bool Update(ProjectRole ProjectRole)
        {
            return WebApi.PutAsync(controllerName, ProjectRole, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
