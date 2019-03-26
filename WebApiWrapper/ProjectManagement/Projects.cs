using FinancialAnalysis.Models.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.ProjectManagement
{
    public static class Projects
    {
        private const string controllerName = "Projects";

        public static IEnumerable<Project> GetAll()
        {
            return WebApi.GetData<IEnumerable<Project>>(controllerName);
        }

        public static Project GetById(int id)
        {
            return WebApi.GetDataById<Project>(controllerName, id);
        }

        public static int Insert(Project Project)
        {
            return WebApi.PostAsync(controllerName, Project).Result;
        }

        public static int Insert(IEnumerable<Project> Projects)
        {
            return WebApi.PostAsync(controllerName, Projects).Result;
        }

        public static bool Update(Project Project)
        {
            return WebApi.PutAsync(controllerName, Project, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
