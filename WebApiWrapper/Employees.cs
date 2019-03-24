using FinancialAnalysis.Models.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWrapper
{
    public static class Employees
    {
        public static IEnumerable<Employee> GetAll()
        {
            return WebApi.GetData<IEnumerable<Employee>>("Employees");
        }

        public static Employee GetById(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Id", id);
            return WebApi.GetData<Employee>("Employees", "GetById", parameters);
        }
    }
}
