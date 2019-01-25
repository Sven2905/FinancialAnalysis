﻿using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FinancialAnalysis.Datalayer.ProductManagement
{
    public class ProductsStoredProcedures : IStoredProcedures
    {
        public ProductsStoredProcedures()
        {
            TableName = "Products";
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
                                "SELECT ProductId, " +
                                "Name, " +
                                "Description, " +
                                "Barcode, " +
                                "DimensionX, " +
                                "DimensionY, " +
                                "DimensionZ, " +
                                "Weight, " +
                                "IsStackable, " +
                                "Picture, " +
                                "PackageUnit, " +
                                "DefaultBuyingPrice, " +
                                "DefaultSellingPrice, " +
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
                    $"CREATE PROCEDURE [{TableName}_Insert] @Name nvarchar(150), @Description nvarchar(150), @Barcode nvarchar(150), @DimensionX decimal, @DimensionY decimal, @DimensionZ decimal, @Weight decimal, @IsStackable bit, @Picture varbinary(MAX), @PackageUnit int, @DefaultBuyingPrice money, @DefaultSellingPrice money, @RefProductCategoryId int AS BEGIN SET NOCOUNT ON; " +
                    $"INSERT into {TableName} (Name, Description, Barcode, DimensionX, DimensionY, DimensionZ, Weight, IsStackable, Picture, PackageUnit, DefaultBuyingPrice, DefaultSellingPrice, RefProductCategoryId) " +
                    "VALUES (@Name, @Description, @Barcode, @DimensionX, @DimensionY, @DimensionZ, @Weight, @IsStackable, @Picture, @PackageUnit, @DefaultBuyingPrice, @DefaultSellingPrice, @RefProductCategoryId); " +
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
                    $"CREATE PROCEDURE [{TableName}_GetById] @ProductId int AS BEGIN SET NOCOUNT ON; SELECT Name, Description, Barcode, DimensionX, DimensionY, DimensionZ, Weight, IsStackable, RefProductCategoryId " +
                    $"FROM {TableName} " +
                    "WHERE ProductId = @ProductId END");
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
                    $"CREATE PROCEDURE [{TableName}_Update] @ProductId int, @Name nvarchar(150), @Description nvarchar(150), @Barcode nvarchar(150), @DimensionX decimal, @DimensionY decimal, @DimensionZ decimal, @Weight decimal, @IsStackable bit, @Picture varbinary(MAX), @PackageUnit int, @DefaultBuyingPrice money, @DefaultSellingPrice money, @RefProductCategoryId int " +
                    "AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} " +
                    "SET Name = @Name, " +
                    "Description = @Description, " +
                    "Barcode = @Barcode, " +
                    "DimensionX = @DimensionX, " +
                    "DimensionY = @DimensionY, " +
                    "DimensionZ = @DimensionZ, " +
                    "Weight = @Weight, " +
                    "IsStackable = @IsStackable, " +
                    "Picture = @Picture, " +
                    "PackageUnit = @PackageUnit, " +
                    "DefaultBuyingPrice = @DefaultBuyingPrice, " +
                    "DefaultSellingPrice = @DefaultSellingPrice, " +
                    "RefProductCategoryId = @RefProductCategoryId " +
                    "WHERE ProductId = @ProductId END");
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
                    $"CREATE PROCEDURE [{TableName}_Delete] @ProductId int AS BEGIN SET NOCOUNT ON; DELETE FROM {TableName} WHERE ProductId = @ProductId END");
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