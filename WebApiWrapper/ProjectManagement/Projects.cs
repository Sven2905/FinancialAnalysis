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
            return WebApi<int>.PostAsync(controllerName, Project).Result;
        }

        public static int Insert(IEnumerable<Project> Projects)
        {
            return WebApi<int>.PostAsync(controllerName, Projects).Result;
        }

        public static bool Update(Project Project)
        {
            return WebApi<bool>.PutAsync(controllerName, Project, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id).Result;
        }
    }
}
