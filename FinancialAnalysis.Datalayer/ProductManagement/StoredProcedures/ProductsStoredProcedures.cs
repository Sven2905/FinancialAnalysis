using System.Data;
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

                sbSP.AppendLine(
                    $"CREATE PROCEDURE [{TableName}_GetAll] AS BEGIN SET NOCOUNT ON; " +
                    "SELECT ProductId, p.Name, p.Description, p.Barcode, p.RefTaxTypeId, p.DimensionX, p.DimensionY, p.DimensionZ, p.Weight, p.IsStackable, p.Picture, p.PackageUnit, p.DefaultBuyingPrice, p.DefaultSellingPrice, p.RefProductCategoryId, " +
                    "pc.ProductCategoryId, pc.Name, pc.Description, " +
                    "t.TaxTypeId, t.Description, t.DescriptionShort, t.AmountOfTax, t.TaxCategory, t.RefCostAccount, t.RefAccountNotPayable " +
                    $"FROM {TableName} p " +
                    $"LEFT JOIN ProductCategories pc ON p.RefProductCategoryId = pc.ProductCategoryId " +
                    $"LEFT JOIN TaxTypes t ON p.RefTaxTypeID = t.TaxTypeId " +
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
                    $"CREATE PROCEDURE [{TableName}_Insert] @Name nvarchar(150), @Description nvarchar(150), @Barcode nvarchar(150), @RefTaxTypeId int, @DimensionX decimal(7,2), @DimensionY decimal(7,2), @DimensionZ decimal(7,2), @Weight decimal(7,3), @IsStackable bit, @Picture varbinary(MAX), @PackageUnit int, @DefaultBuyingPrice money, @DefaultSellingPrice money, @RefProductCategoryId int AS BEGIN SET NOCOUNT ON; " +
                    $"INSERT into {TableName} (Name, Description, Barcode, RefTaxTypeId, DimensionX, DimensionY, DimensionZ, Weight, IsStackable, Picture, PackageUnit, DefaultBuyingPrice, DefaultSellingPrice, RefProductCategoryId) " +
                    "VALUES (@Name, @Description, @Barcode, @RefTaxTypeId, @DimensionX, @DimensionY, @DimensionZ, @Weight, @IsStackable, @Picture, @PackageUnit, @DefaultBuyingPrice, @DefaultSellingPrice, @RefProductCategoryId); " +
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
                    $"CREATE PROCEDURE [{TableName}_GetById] @ProductId int AS BEGIN SET NOCOUNT ON; " +
                    "SELECT ProductId, p.Name, p.Description, p.Barcode, p.RefTaxTypeId, p.DimensionX, p.DimensionY, p.DimensionZ, p.Weight, p.IsStackable, p.Picture, p.PackageUnit, p.DefaultBuyingPrice, p.DefaultSellingPrice, p.RefProductCategoryId, " +
                    "pc.ProductCategoryId, pc.Name, pc.Description, " +
                    "t.TaxTypeId, t.Description, t.DescriptionShort, t.AmountOfTax, t.TaxCategory, t.RefCostAccount, t.RefAccountNotPayable " +
                    $"FROM {TableName} p " +
                    $"LEFT JOIN ProductCategories pc ON p.RefProductCategoryId = pc.ProductCategoryId " +
                    $"LEFT JOIN TaxTypes t ON p.RefTaxTypeID = t.TaxTypeId " +
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
                    $"CREATE PROCEDURE [{TableName}_Update] @ProductId int, @Name nvarchar(150), @Description nvarchar(150), @Barcode nvarchar(150), @RefTaxTypeId int, @DimensionX decimal(7,2), @DimensionY decimal(7,2), @DimensionZ decimal(7,2), @Weight decimal(7,3), @IsStackable bit, @Picture varbinary(MAX), @PackageUnit int, @DefaultBuyingPrice money, @DefaultSellingPrice money, @RefProductCategoryId int " +
                    "AS BEGIN SET NOCOUNT ON; " +
                    $"UPDATE {TableName} " +
                    "SET Name = @Name, " +
                    "Description = @Description, " +
                    "Barcode = @Barcode, " +
                    "RefTaxTypeId = @RefTaxTypeId, " +
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