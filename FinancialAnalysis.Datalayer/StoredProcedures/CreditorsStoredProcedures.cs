using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Datalayer.StoredProcedures
{
    public class CreditorsStoredProcedures : IStoredProcedures
    {
        public string TableName { get; }

        public CreditorsStoredProcedures()
        {
            TableName = "Creditors";
        }

        /// <summary>
        /// Check if all Stored Procedures are created, otherwise create them
        /// </summary>
        public void CheckAndCreateProcedures()
        {
            InsertData();
            GetAllData();
            GetById();
            UpdateData();
            DeleteData();
        }

        private void GetAllData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAll", DatabaseNames.FinancialAnalysisDB))
            {
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_GetAll] AS BEGIN SET NOCOUNT ON; " +
                    $"SELECT c.CreditorId, c.RefCompanyId, c.RefCostAccountId, " +
                    $"co.CompanyId, co.Name, co.Street, co.Postcode, co.City, co.ContactPerson, co.UStID, co.TaxNumber, co.Phone, co.Fax, co.eMail, co.Website, co.IBAN, co.BIC, co.BankName, co.FederalState, " +
                    $"a.CostAccountId, a.Description, a.AccountNumber, a.RefTaxTypeId, a.RefCostAccountCategoryId, a.IsVisible " +
                    $"FROM {TableName} c " +
                    $"INNER JOIN Companies co ON RefCompanyId = co.CompanyId " +
                    $"INNER JOIN CostAccounts a ON RefCostAccountId = a.CostAccountId END");
                using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (SqlCommand cmd = new SqlCommand(sbSP.ToString(), connection))
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
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_Insert] @RefCompanyId int, @RefCostAccountId int AS BEGIN SET NOCOUNT ON; " +
                                $"INSERT into {TableName} (RefCompanyId, RefCostAccountId) " +
                                $"VALUES (@RefCompanyId, @RefCostAccountId); " +
                                $"SELECT CAST(SCOPE_IDENTITY() as int) END");
                using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (SqlCommand cmd = new SqlCommand(sbSP.ToString(), connection))
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
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_GetById] @CreditorId int AS BEGIN SET NOCOUNT ON; SELECT CreditorId, RefCompanyId, RefCostAccountId " +
                    $"FROM {TableName} " +
                    $"WHERE CreditorId = @CreditorId END");
                using (SqlConnection connection =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (SqlCommand cmd = new SqlCommand(sbSP.ToString(), connection))
                    {
                        connection.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
        }

        private void UpdateData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_Update", DatabaseNames.FinancialAnalysisDB))
            {
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_Update] @CreditorId int, @RefCompanyId int, @RefCostAccountId int " +
                    $"AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} " +
                    $"SET RefCompanyId = @RefCompanyId, RefCostAccountId = @RefCostAccountId " +
                    $"WHERE CreditorId = @CreditorId END");
                using (SqlConnection connection =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (SqlCommand cmd = new SqlCommand(sbSP.ToString(), connection))
                    {
                        connection.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
        }

        private void DeleteData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_Delete", DatabaseNames.FinancialAnalysisDB))
            {
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_Delete] @CreditorId int AS BEGIN SET NOCOUNT ON; DELETE FROM {TableName} WHERE CreditorId = @CreditorId END");
                using (SqlConnection connection =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    using (SqlCommand cmd = new SqlCommand(sbSP.ToString(), connection))
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
