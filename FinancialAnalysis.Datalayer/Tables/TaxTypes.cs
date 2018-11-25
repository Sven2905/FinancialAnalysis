using Dapper;
using FinancialAnalysis.Datalayer.StoredProcedures;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAnalysis.Datalayer.Tables
{
    public class TaxTypes : ITable
    {
        private TaxTypesStoredProcedures sp = new TaxTypesStoredProcedures();

        public TaxTypes()
        {
            TableName = "TaxTypes";
            CheckAndCreateTable();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs\\Tables.txt", rollingInterval: RollingInterval.Month)
                .CreateLogger();
        }

        public void CheckAndCreateStoredProcedures()
        {
            sp.CheckAndCreateProcedures();
        }

        /// <summary>
        /// Checks if table exists otherwise creates it
        /// </summary>
        public void CheckAndCreateTable()
        {
            try
            {
                SqlConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr = $"If not exists (select name from sysobjects where name = '{TableName}') " +
                    $"CREATE TABLE {TableName}(" +
                    $"TaxTypeId int IDENTITY(1,1) PRIMARY KEY, " +
                    $"Description nvarchar(50) NOT NULL, " +
                    $"DescriptionShort nvarchar(50) NOT NULL, " +
                    $"AmountOfTax decimal NOT NULL, " +
                    $"TaxCategory int NOT NULL, " +
                    $"RefAccountNumber int, " +
                    $"RefAccountNotPayable int )";

                using (SqlCommand command = new SqlCommand(commandStr, con))
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


        /// <summary>
        /// Returns all TaxType records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TaxType> GetAll()
        {
            IEnumerable<TaxType> output = new List<TaxType>();
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<TaxType>($"dbo.{TableName}_GetAll");
                }

            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }
            return output;
        }

        /// <summary>
        /// Inserts the TaxType item
        /// </summary>
        /// <param name="taxType"></param>
        /// <returns>Id of inserted item</returns>
        public int Insert(TaxType taxType)
        {
            int id = 0;
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    var result = con.Query<int>($"dbo.{TableName}_Insert @Description, @DescriptionShort, @AmountOfTax, @TaxCategory, @RefAccountNumber, @RefAccountNotPayable", taxType);
                    return result.Single();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' from table '{TableName}'", e);
            }
            return id;
        }

        /// <summary>
        /// Inserts the list of TaxType items
        /// </summary>
        /// <param name="taxTypes"></param>
        public void Insert(IEnumerable<TaxType> taxTypes)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    foreach (var taxType in taxTypes)
                    {
                        Insert(taxType);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' into table '{TableName}'", e);
            }
        }

        /// <summary>
        /// Returns TaxType by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TaxType GetById(int id)
        {
            TaxType output = new TaxType();
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<TaxType>($"dbo.{TableName}_GetById @TaxTypeId", new { TaxTypeId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }
            return output;
        }

        /// <summary>
        /// Update TaxType, if not exist, insert it
        /// </summary>
        /// <param name="taxType"></param>
        public void UpdateOrInsert(TaxType taxType)
        {
            if (taxType.TaxTypeId == 0 || GetById(taxType.TaxTypeId) is null)
            {
                Insert(taxType);
                return;
            }

            Update(taxType);
        }

        /// <summary>
        /// Update TaxTypes, if not exist insert them
        /// </summary>
        /// <param name="taxTypes"></param>
        public void UpdateOrInsert(IEnumerable<TaxType> taxTypes)
        {
            foreach (var taxType in taxTypes)
            {
                UpdateOrInsert(taxType);
            }
        }

        /// <summary>
        /// Update TaxType
        /// </summary>
        /// <param name="taxType"></param>
        public void Update(TaxType taxType)
        {
            if (taxType.TaxTypeId == 0 || GetById(taxType.TaxTypeId) is null)
            {
                return;
            }

            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Update @TaxTypeId, @Description, @DescriptionShort, @AmountOfTax, @TaxCategory, @RefAccountNumber, @RefAccountNotPayable", taxType);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }

        /// <summary>
        /// Delete TaxType by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Delete @TaxTypeId", new { TaxTypeId = id });
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Delete' from table '{TableName}'", e);
            }
        }

        /// <summary>
        /// Delete TaxType by Item
        /// </summary>
        /// <param name="id"></param>
        public void Delete(TaxType taxType)
        {
            Delete(taxType.TaxTypeId);
        }

        /// <summary>
        /// Seeds the table with initial data
        /// </summary>
        public void Seed()
        {
            List<TaxType> taxTypes = new List<TaxType>()
            {
                new TaxType() { DescriptionShort = "Keine", Description="Keine", AmountOfTax = 0, TaxCategory = TaxCategory.None },
                new TaxType() { DescriptionShort = "Bau 7%", Description="Bau mit 7% USt/VSt", AmountOfTax = 7, TaxCategory = TaxCategory.thirteenB, RefAccountNumber = 1785},
                new TaxType() { DescriptionShort = "I.g.E 7% USt/VSt", Description="I.g.E 7% USt/VSt", AmountOfTax = 7, TaxCategory = TaxCategory.igE, RefAccountNumber = 1773 },
                new TaxType() { DescriptionShort = "I.g.E 16% USt/VSt", Description="I.g.E 16% USt/VSt", AmountOfTax = 16, TaxCategory = TaxCategory.igE, RefAccountNumber = 1774 },
                new TaxType() { DescriptionShort = "I.g.E 19% USt/VSt", Description="I.g.E 19% USt/VSt", AmountOfTax = 19, TaxCategory = TaxCategory.igE, RefAccountNumber = 1772 },
                new TaxType() { DescriptionShort = "I.g.E Neufahrzeug", Description="I.g.E Neufahrzeuge 19% USt/VSt", AmountOfTax = 19, TaxCategory = TaxCategory.igE, RefAccountNumber = 1784 },
                new TaxType() { DescriptionShort = "Kfz 19% VSt. 50%", Description="Kfz 19% Vorsteuer. 50%", AmountOfTax = 19, TaxCategory = TaxCategory.fiftyPercent, RefAccountNumber = 1570 },
                new TaxType() { DescriptionShort = "Kfz VSt. 50%", Description="Kfz Vorsteuer. 50%", AmountOfTax = 16, TaxCategory = TaxCategory.fiftyPercent, RefAccountNumber = 1570 },
                new TaxType() { DescriptionShort = "USt. 15%", Description="Umsatzsteuer 15%", AmountOfTax = 15, TaxCategory = TaxCategory.Netto, RefAccountNumber = 1770 },
                new TaxType() { DescriptionShort = "USt. 16%", Description="Umsatzsteuer 16%", AmountOfTax = 16, TaxCategory = TaxCategory.Netto, RefAccountNumber = 1775 },
                new TaxType() { DescriptionShort = "USt. 19%", Description="Umsatzsteuer 19%", AmountOfTax = 19, TaxCategory = TaxCategory.Netto, RefAccountNumber = 1776 },
                new TaxType() { DescriptionShort = "USt. 7%", Description="Umsatzsteuer 7%", AmountOfTax = 7, TaxCategory = TaxCategory.Netto, RefAccountNumber = 1771 },
                new TaxType() { DescriptionShort = "USt/VSt 19%", Description="Reverse Charge (Steuerschuld Leistungsempf.) 19% USt/VSt", AmountOfTax = 19, TaxCategory = TaxCategory.thirteenB, RefAccountNumber = 1787 },
                new TaxType() { DescriptionShort = "USt/VSt 7%", Description= "Reverse Charge (Steuerschuld Leistungsempf.) 7% USt/VSt", AmountOfTax = 7, TaxCategory = TaxCategory.thirteenB, RefAccountNumber = 1785 },
                new TaxType() { DescriptionShort = "VSt. 15%", Description="Vorsteuer 15%", AmountOfTax = 15, TaxCategory = TaxCategory.Netto, RefAccountNumber = 1771 },
                new TaxType() { DescriptionShort = "VSt. 16%", Description="Vorsteuer 16%", AmountOfTax = 16, TaxCategory = TaxCategory.Netto, RefAccountNumber = 1575 },
                new TaxType() { DescriptionShort = "VSt. 19%", Description="Vorsteuer 19%", AmountOfTax = 19, TaxCategory = TaxCategory.Netto, RefAccountNumber = 1576 },
                new TaxType() { DescriptionShort = "VSt. 7%", Description="Vorsteuer 7%", AmountOfTax = 7, TaxCategory = TaxCategory.Netto, RefAccountNumber = 1571 },
                
            };

            using (IDbConnection con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
            {
                con.Execute($"dbo.{TableName}_Insert @Description, @DescriptionShort, @AmountOfTax, @TaxCategory, @RefAccountNumber, @RefAccountNotPayable", taxTypes);
            }
        }

        public string TableName { get; }
    }
}
