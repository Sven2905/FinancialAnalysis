using FinancialAnalysis.Models.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper.ProjectManagement
{
    public static class Employees
    {
        private const string controllerName = "Employees";

        public static IEnumerable<Employee> GetAll()
        {
            return WebApi.GetData<IEnumerable<Employee>>(controllerName);
        }

        public static Employee GetById(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Id", id);
            return WebApi.GetData<Employee>(controllerName, "GetById", parameters);
        }

        public static int Insert(Employee employee)
        {
            return WebApi.PostAsync(controllerName, employee).Result;
        }

        public static int Insert(IEnumerable<Employee> employees)
        {
            return WebApi.PostAsync(controllerName, employees).Result;
        }

        public static bool Update(Employee employee)
        {
            return WebApi.PutAsync(controllerName, employee, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi.DeleteAsync(controllerName, id).Result;
        }
    }
}
