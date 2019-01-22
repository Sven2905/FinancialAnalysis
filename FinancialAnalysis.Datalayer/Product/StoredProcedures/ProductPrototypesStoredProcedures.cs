﻿using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FinancialAnalysis.Datalayer.Product
{
    public class ProductPrototypesStoredProcedures : IStoredProcedures
    {
        public ProductPrototypesStoredProcedures()
        {
            TableName = "ProductPrototypes";
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
            UpdateData();
            DeleteData();
        }

        private void GetAllData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_GetAll", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine($"CREATE PROCEDURE [{TableName}_GetAll] AS BEGIN SET NOCOUNT ON; " +
                                "SELECT ProductPrototypeId, " +
                                "Name, " +
                                "Description, " +
                                "DimensionX, " +
                                "DimensionY, " +
                                "DimensionZ, " +
                                "Weight, " +
                                "IsStackable, " +
                                "Picture, " +
                                "PackageUnit, " +
                                "BuyingPrice, " +
                                "SalePrice, " +
                                "RefProductCategoryId " +
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
                    $"CREATE PROCEDURE [{TableName}_Insert] @Name nvarchar(150), @Description nvarchar(150), @DimensionX decimal, @DimensionY decimal, @DimensionZ decimal, @Weight decimal, @IsStackable bit, @Picture varbinary(MAX), @PackageUnit int, @BuyingPrice money, @SalePrice money, @RefProductCategoryId int AS BEGIN SET NOCOUNT ON; " +
                    $"INSERT into {TableName} (Name, Description, DimensionX, DimensionY, DimensionZ, Weight, IsStackable, Picture, PackageUnit, BuyingPrice, SalePrice, RefProductCategoryId) " +
                    "VALUES (@Name, @Description, @DimensionX, @DimensionY, @DimensionZ, @Weight, @IsStackable, @Picture, @PackageUnit, @BuyingPrice, @SalePrice, @RefProductCategoryId); " +
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
                    $"CREATE PROCEDURE [{TableName}_GetById] @ProductPrototypeId int AS BEGIN SET NOCOUNT ON; SELECT Name, Description, DimensionX, DimensionY, DimensionZ, Weight, IsStackable, Picture, PackageUnit, BuyingPrice, SalePrice, RefProductCategoryId " +
                    $"FROM {TableName} " +
                    "WHERE ProductPrototypeId = @ProductPrototypeId END");
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

        private void UpdateData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_Update", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_Update] @ProductPrototypeId int, @Name nvarchar(150), @Description nvarchar(150), @DimensionX decimal, @DimensionY decimal, @DimensionZ decimal, @Weight decimal, @IsStackable bit, @Picture varbinary(MAX), @PackageUnit int, @BuyingPrice money, @SalePrice money, @RefProductCategoryId int " +
                    "AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} " +
                    "SET Name = @Name, " +
                    "Description = @Description, " +
                    "DimensionX = @DimensionX, " +
                    "DimensionY = @DimensionY, " +
                    "DimensionZ = @DimensionZ, " +
                    "Weight = @Weight, " +
                    "IsStackable = @IsStackable, " +
                    "Picture = @Picture, " +
                    "PackageUnit = @PackageUnit, " +
                    "BuyingPrice = @BuyingPrice, " +
                    "SalePrice = @SalePrice, " +
                    "RefProductCategoryId = @RefProductCategoryId " +
                    "WHERE ProductPrototypeId = @ProductPrototypeId END");
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

        private void DeleteData()
        {
            if (!Helper.StoredProcedureExists($"dbo.{TableName}_Delete", DatabaseNames.FinancialAnalysisDB))
            {
                var sbSP = new StringBuilder();

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_Delete] @ProductPrototypeId int AS BEGIN SET NOCOUNT ON; DELETE FROM {TableName} WHERE ProductPrototypeId = @ProductPrototypeId END");
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