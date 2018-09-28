﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Datalayer.StoredProcedures
{
    class CompaniesStoredProcedures : IStoredProcedures
    {
        public string TableName { get; }

        public CompaniesStoredProcedures()
        {
            TableName = "Companies";
        }

        public void CheckAndCreateProcedures()
        {
            InsertData();
            GetAllData();
            GetById();
            UpdateData();
            DeleteData();
            IsCompanyInUse();
        }

        private void GetAllData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAll", DatabaseNames.FinancialAnalysisDB))
            {
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_GetAll] AS BEGIN SET NOCOUNT ON; " +
                    $"SELECT CompanyId, Name, Street, Postcode, City, ContactPerson, UStID, TaxNumber, Phone, Fax, eMail, Website, IBAN, BIC, BankName, FederalState " +
                    $"FROM {TableName} " +
                    $"ORDER BY Name END");
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

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_Insert] " +
                                $"@Name nvarchar(50), @Street nvarchar(50), @Postcode int, @City nvarchar(50), @ContactPerson nvarchar(50), @UStID nvarchar(50), @TaxNumber nvarchar(50), @Phone nvarchar(50), @Fax nvarchar(50), @eMail nvarchar(50), @Website nvarchar(50), @IBAN nvarchar(50), @BIC nvarchar(50), @BankName nvarchar(50), @FederalState int " +
                                $"AS BEGIN SET NOCOUNT ON; INSERT into {TableName} (Name,Street,Postcode,City,ContactPerson,UStID,TaxNumber,Phone,Fax,eMail,Website,IBAN,BIC,BankName,FederalState) " +
                                $"VALUES (@Name,@Street,@Postcode,@City,@ContactPerson,@UStID,@TaxNumber,@Phone,@Fax,@eMail,@Website,@IBAN,@BIC,@BankName,@FederalState); " +
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
                    $"CREATE PROCEDURE [{TableName}_GetById] @CompanyId int AS BEGIN SET NOCOUNT ON; SELECT * FROM {TableName} WHERE CompanyId = @CompanyId END");
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
                    $"CREATE PROCEDURE [{TableName}_Update] @CompanyId int, @Name nvarchar(50), @Street nvarchar(50), @Postcode int, @City nvarchar(50), @ContactPerson nvarchar(50), @UStID nvarchar(50), @TaxNumber nvarchar(50), @Phone nvarchar(50), @Fax nvarchar(50), @eMail nvarchar(50), @Website nvarchar(50), @IBAN nvarchar(50), @BIC nvarchar(50), @BankName nvarchar(50), @FederalState int " +
                    $"AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} SET " +
                    $"Name = @Name, " +
                    $"Street = @Street, " +
                    $"Postcode = @Postcode, " +
                    $"City = @City, " +
                    $"ContactPerson = @ContactPerson, " +
                    $"UStID = @UStID, " +
                    $"TaxNumber = @TaxNumber, " +
                    $"Phone = @Phone, " +
                    $"Fax = @Fax, " +
                    $"eMail = @eMail, " +
                    $"Website = @Website, " +
                    $"IBAN = @IBAN, " +
                    $"BIC = @BIC, " +
                    $"BankName = @BankName, " +
                    $"FederalState = @FederalState " +
                    $"WHERE CompanyId = @CompanyId END");
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
                    $"CREATE PROCEDURE [{TableName}_Delete] @CompanyId int AS BEGIN SET NOCOUNT ON; DELETE FROM {TableName} WHERE CompanyId = @CompanyId END");
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

        private void IsCompanyInUse()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_IsCompanyInUse", DatabaseNames.FinancialAnalysisDB))
            {
                StringBuilder sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_IsCompanyInUse] @CompanyId int AS " +
                    $"SELECT CASE WHEN EXISTS ( " +
                    $"SELECT * FROM {TableName} " +
                    $"RIGHT JOIN Creditors ON {TableName}.CompanyId = Creditors.RefCompanyId " +
                    $"RIGHT JOIN Debitors ON {TableName}.CompanyId = Debitors.RefCompanyId " +
                    $"WHERE CompanyId = @CompanyId) " +
                    $"THEN CAST(1 AS BIT) " +
                    $"ELSE CAST(0 AS BIT) END");
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
