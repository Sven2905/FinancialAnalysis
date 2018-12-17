using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FinancialAnalysis.Models.ProjectManagement;
using Serilog;

namespace FinancialAnalysis.Datalayer.ProjectManagement
{
    public class Employees : ITable
    {
        private readonly EmployeesStoredProcedures sp = new EmployeesStoredProcedures();

        public Employees()
        {
            TableName = "Employees";
            CheckAndCreateTable();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs\\Tables.txt", rollingInterval: RollingInterval.Month)
                .CreateLogger();
        }

        public string TableName { get; }

        public void CheckAndCreateTable()
        {
            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"If not exists (select name from sysobjects where name = '{TableName}') CREATE TABLE {TableName}(" +
                    "EmployeeId int IDENTITY(1,1) PRIMARY KEY," +
                    "Firstname nvarchar(150) NOT NULL," +
                    "Lastname nvarchar(150) NOT NULL," +
                    "Birthdate date," +
                    "Street nvarchar(150) NOT NULL," +
                    "City nvarchar(150) NOT NULL," +
                    "Postcode int NOT NULL," +
                    "Gender int," +
                    "CivilStatus int," +
                    "TaxId nvarchar(150)," +
                    "HasDrivingLicence bit," +
                    "Nationality nvarchar(150)," +
                    "Confession nvarchar(150)," +
                    "BankName nvarchar(150)," +
                    "BIC nvarchar(150)," +
                    "IBAN nvarchar(150)," +
                    "NationalInsuranceNumber nvarchar(150)," +
                    "Salary money," +
                    "WorkHoursPerWeek real," +
                    "VacationDays real," +
                    "Picture varbinary(MAX)," +
                    "RefHealthInsuranceId int," +
                    "Mail nvarchar(150)," +
                    "Phone nvarchar(150)," +
                    "RefTariffId int)";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating table '{TableName}'", e);
            }
        }

        public void CheckAndCreateStoredProcedures()
        {
            sp.CheckAndCreateProcedures();
        }

        /// <summary>
        ///     Returns all Employee records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Employee> GetAll()
        {
            IEnumerable<Employee> output = new List<Employee>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<Employee>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the Employee item
        /// </summary>
        /// <param name="Employee"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(Employee Employee)
        {
            var id = 0;
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>(
                        $"dbo.{TableName}_Insert @Firstname, @Lastname, @Birthdate, @Street, @City, @Postcode, @Gender, @CivilStatus, @RefTariffId, @TaxId, @RefHealthInsuranceId, @HasDrivingLicence, @Nationality, @Confession, @BankName, @BIC, @IBAN, @NationalInsuranceNumber, @Salary, @WorkHoursPerWeek, @VacationDays, @Picture, @Mail, @Phone",
                        Employee);
                    id = result.Single();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }

            return id;
        }

        /// <summary>
        ///     Inserts the list of Project items
        /// </summary>
        /// <param name="Employees"></param>
        public void Insert(IEnumerable<Employee> Employees)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var Employee in Employees) Insert(Employee);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert items' into table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns Employee by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Employee GetById(int id)
        {
            var output = new Employee();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<Employee>($"dbo.{TableName}_GetById @EmployeeId",
                        new {EmployeeId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Update Employee, if not exist, insert it
        /// </summary>
        /// <param name="Employee"></param>
        public void UpdateOrInsert(Employee Employee)
        {
            if (Employee.EmployeeId == 0 || GetById(Employee.EmployeeId) is null)
            {
                Insert(Employee);
                return;
            }

            Update(Employee);
        }

        /// <summary>
        ///     Update Employees, if not exist insert them
        /// </summary>
        /// <param name="Employees"></param>
        public void UpdateOrInsert(IEnumerable<Employee> Employees)
        {
            foreach (var Employee in Employees) UpdateOrInsert(Employee);
        }

        /// <summary>
        ///     Update Employee
        /// </summary>
        /// <param name="Employee"></param>
        public void Update(Employee Employee)
        {
            if (Employee.EmployeeId == 0 || GetById(Employee.EmployeeId) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @EmployeeId, @Firstname, @Lastname, @Birthdate, @Street, @City, @Postcode, @Gender, @CivilStatus, @RefTariffId, @TaxId, " +
                        $"@RefHealthInsuranceId, @HasDrivingLicence, @Nationality, @Confession, @BankName, @BIC, @IBAN, @NationalInsuranceNumber, @Salary, @WorkHoursPerWeek, " +
                        $"@VacationDays, @Picture, @Mail, @Phone", Employee);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Delete Employee by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @EmployeeId", new { EmployeeId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        public void AddReferences()
        {
            AddHealthInsurancesReference();
        }

        private void AddHealthInsurancesReference()
        {
            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"IF(OBJECT_ID('FK_{TableName}_HealthInsurances', 'F') IS NULL) ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_HealthInsurances FOREIGN KEY(RefHealthInsuranceId) REFERENCES HealthInsurance(HealthInsuranceId)";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating reference between '{TableName}' and HealthInsurance", e);
            }
        }
    }
}