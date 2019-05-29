using FinancialAnalysis.Models.ProjectManagement;
using System.Collections.Generic;

namespace WebApiWrapper.ProjectManagement
{
    public static class Employees
    {
        private const string controllerName = "Employees";

        public static List<Employee> GetAll()
        {
            return WebApi<List<Employee>>.GetData(controllerName);
        }

        public static Employee GetById(int id)
        {
            return WebApi<Employee>.GetDataById(controllerName, id);
        }

        public static int Insert(Employee employee)
        {
            return WebApi<int>.PostAsync(controllerName, employee, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<Employee> employees)
        {
            return WebApi<int>.PostAsync(controllerName, employees, "MultiPost").Result;
        }

        public static bool Update(Employee employee)
        {
            return WebApi<bool>.PutAsync(controllerName, employee, "Put").Result;
        }

        public static bool Delete(int id)
        {
            return WebApi<bool>.DeleteAsync(controllerName, id);
        }
    }
}
