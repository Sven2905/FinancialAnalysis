using FinancialAnalysis.Logic.Models.Accounting;
using FinancialAnalysis.Logic.Models.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.Models
{
    class FinanceContext : DbContext
    {
        public FinanceContext() : base("name=FinanceContext")
        {

        }

        public DbSet<CostAccount> CostAccounts { get; set; }
        public DbSet<CostAccountCategory> CostAccountCategories { get; set; }
        public DbSet<CostAccountType> CostAccountTypes { get; set; }
        public DbSet<CostCenter> CostCenters { get; set; }
        public DbSet<TaxType> TaxTypes { get; set; }
        public DbSet<Kontenrahmen> Kontenrahmen { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<HealthInsurance> HealthInsurances { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<ProjectRole> ProjectRoles { get; set; }
        public DbSet<ProjectEmployeeMapping> ProjectEmployeeMappings { get; set; }
        public DbSet<ProjectWorkingTime> ProjectWorkingTimes { get; set; }
        public DbSet<Accounting.Accounting> Accountings { get; set; }
    }
}
