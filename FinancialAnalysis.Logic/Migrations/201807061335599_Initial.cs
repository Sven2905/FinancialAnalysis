namespace FinancialAnalysis.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CostAccountCategories",
                c => new
                    {
                        CostAccountCategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ParentCategoryId = c.Int(),
                        CostAccountTypeId = c.Int(),
                    })
                .PrimaryKey(t => t.CostAccountCategoryId)
                .ForeignKey("dbo.CostAccountTypes", t => t.CostAccountTypeId)
                .Index(t => t.CostAccountTypeId);
            
            CreateTable(
                "dbo.CostAccounts",
                c => new
                    {
                        CostAccountId = c.Int(nullable: false, identity: true),
                        AccountNumber = c.Int(nullable: false),
                        Name = c.String(),
                        GainsOutputAllocation = c.String(),
                        SalesTaxAllocation = c.Int(nullable: false),
                        TaxTypeId = c.Int(nullable: false),
                        CostAccountCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CostAccountId)
                .ForeignKey("dbo.CostAccountCategories", t => t.CostAccountCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.TaxTypes", t => t.TaxTypeId, cascadeDelete: true)
                .Index(t => t.TaxTypeId)
                .Index(t => t.CostAccountCategoryId);
            
            CreateTable(
                "dbo.TaxTypes",
                c => new
                    {
                        TaxTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AmountOfTax = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.TaxTypeId);
            
            CreateTable(
                "dbo.CostAccountTypes",
                c => new
                    {
                        CostAccountTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CostAccountTypeId);
            
            CreateTable(
                "dbo.CostCenters",
                c => new
                    {
                        CostCenterId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CostCenterId);
            
            CreateTable(
                "dbo.Kontenrahmen",
                c => new
                    {
                        KontenrahmenId = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        Name = c.String(),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.KontenrahmenId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CostAccountCategories", "CostAccountTypeId", "dbo.CostAccountTypes");
            DropForeignKey("dbo.CostAccounts", "TaxTypeId", "dbo.TaxTypes");
            DropForeignKey("dbo.CostAccounts", "CostAccountCategoryId", "dbo.CostAccountCategories");
            DropIndex("dbo.CostAccounts", new[] { "CostAccountCategoryId" });
            DropIndex("dbo.CostAccounts", new[] { "TaxTypeId" });
            DropIndex("dbo.CostAccountCategories", new[] { "CostAccountTypeId" });
            DropTable("dbo.Kontenrahmen");
            DropTable("dbo.CostCenters");
            DropTable("dbo.CostAccountTypes");
            DropTable("dbo.TaxTypes");
            DropTable("dbo.CostAccounts");
            DropTable("dbo.CostAccountCategories");
        }
    }
}
