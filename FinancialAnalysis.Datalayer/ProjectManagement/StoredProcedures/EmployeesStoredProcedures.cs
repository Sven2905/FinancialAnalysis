using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FinancialAnalysis.Datalayer.ProjectManagement
{
    public class EmployeesStoredProcedures : IStoredProcedures
    {
        public EmployeesStoredProcedures()
        {
            TableName = "Employees";
        }

        public string TableName { get; }

        /// <summary>
        ///     Check if all Stored Procedures are created, otherwise create them
        /// </summary>
        public void CheckAndCreateProcedures()
        {
            InsertData();
            GetAllData();
            GetById();
        }

        private void GetAllData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAll", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_GetAll] AS BEGIN SET NOCOUNT ON; " +
                                "SELECT EmployeeId, Firstname, Lastname, Birthdate, Street, City, Postcode, Gender, CivilStatus, RefTariffId, TaxId, RefHealthInsuranceId, HasDrivingLicence, Nationality, Confession, BankName, BIC, IBAN, NationalInsuranceNumber, Salary, WorkHoursPerWeek, VacationDays, Picture, PictureName " +
                                $"FROM {TableName} " +
                                "END");
                using (var connection =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (var cmd = new SqlCommand(sbSP.ToString(), connection))
                    {
                        connection.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
        }

        private void InsertData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_Insert", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_Insert] @Firstname nvarchar(150), @Lastname nvarchar(150), @Birthdate date, @Street nvarchar(150), @City nvarchar(150), @Postcode int, @Gender int, @CivilStatus int, @RefTariffId int, @TaxId nvarchar(150), @RefHealthInsuranceId int, @HasDrivingLicence bit, @Nationality nvarchar(150), @Confession nvarchar(150), @BankName nvarchar(150), @BIC nvarchar(150), @IBAN nvarchar(150), @NationalInsuranceNumber nvarchar(150), @Salary money, @WorkHoursPerWeek real, @VacationDays real, @Picture varbinary(MAX), @PictureName nvarchar(150) AS BEGIN SET NOCOUNT ON; " +
                    $"INSERT into {TableName} (Firstname, Lastname, Birthdate, Street, City, Postcode, Gender, CivilStatus, RefTariffId, TaxId, RefHealthInsuranceId, HasDrivingLicence, Nationality, Confession, BankName, BIC, IBAN, NationalInsuranceNumber, Salary, WorkHoursPerWeek, VacationDays, Picture, PictureName) " +
                    "VALUES (@Firstname, @Lastname, @Birthdate, @Street, @City, @Postcode, @Gender, @CivilStatus, @RefTariffId, @TaxId, @RefHealthInsuranceId, @HasDrivingLicence, @Nationality, @Confession, @BankName, @BIC, @IBAN, @NationalInsuranceNumber, @Salary, @WorkHoursPerWeek, @VacationDays, @Picture, @PictureName); " +
                    "SELECT CAST(SCOPE_IDENTITY() as int) END");
                using (var connection =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (var cmd = new SqlCommand(sbSP.ToString(), connection))
                    {
                        connection.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
        }

        private void GetById()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetById", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_GetById] @EmployeeId int AS BEGIN SET NOCOUNT ON; SELECT EmployeeId, Firstname, Lastname, Birthdate, Street, City, Postcode, Gender, CivilStatus, RefTariffId, TaxId, RefHealthInsuranceId, HasDrivingLicence, Nationality, Confession, BankName, BIC, IBAN, NationalInsuranceNumber, Salary, WorkHoursPerWeek, VacationDays, Picture, PictureName " +
                    $"FROM {TableName} " +
                    "WHERE EmployeeId = @EmployeeId END");
                using (var connection =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (var cmd = new SqlCommand(sbSP.ToString(), connection))
                    {
                        connection.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
        }
    }
}