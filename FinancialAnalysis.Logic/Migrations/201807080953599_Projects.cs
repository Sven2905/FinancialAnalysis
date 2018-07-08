namespace FinancialAnalysis.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Projects : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accountings",
                c => new
                    {
                        AccountingId = c.Int(nullable: false, identity: true),
                        SenderId = c.Int(nullable: false),
                        ReceiverId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AccountingId)
                .ForeignKey("dbo.Kontenrahmen", t => t.ReceiverId, cascadeDelete: false)
                .ForeignKey("dbo.Kontenrahmen", t => t.SenderId, cascadeDelete: false)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Street = c.String(),
                        Postcode = c.Int(nullable: false),
                        City = c.String(),
                        EmployeeId = c.Int(nullable: false),
                        UStID = c.String(),
                        TaxNumber = c.String(),
                        Phone = c.String(),
                        Fax = c.String(),
                        Mail = c.String(),
                        Website = c.String(),
                        IBAN = c.String(),
                        BIC = c.String(),
                        BankName = c.String(),
                        FederalState = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false, identity: true),
                        Firstname = c.String(),
                        Lastname = c.String(),
                        Birthdate = c.DateTime(nullable: false),
                        Street = c.String(),
                        City = c.String(),
                        Postcode = c.Int(nullable: false),
                        Gender = c.Int(nullable: false),
                        CivilStatus = c.Int(nullable: false),
                        TaxId = c.String(),
                        HealthInsuranceId = c.Int(nullable: false),
                        DrivingLicence = c.Boolean(nullable: false),
                        Nationality = c.String(),
                        Confession = c.String(),
                        BankName = c.String(),
                        BIC = c.String(),
                        IBAN = c.String(),
                        Salary = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HoursPerWeek = c.Single(nullable: false),
                        VacationDays = c.Int(nullable: false),
                        NationalInsuranceNumber = c.String(),
                        ProjectEmployeeMappingId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeId)
                .ForeignKey("dbo.HealthInsurances", t => t.HealthInsuranceId, cascadeDelete: true)
                .Index(t => t.HealthInsuranceId);
            
            CreateTable(
                "dbo.HealthInsurances",
                c => new
                    {
                        HealthInsuranceId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Street = c.String(),
                        Postcode = c.Int(nullable: false),
                        City = c.String(),
                        ContactName = c.String(),
                    })
                .PrimaryKey(t => t.HealthInsuranceId);
            
            CreateTable(
                "dbo.ProjectEmployeeMappings",
                c => new
                    {
                        ProjectEmployeeMappingId = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        ProjectRoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProjectEmployeeMappingId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.ProjectRoles", t => t.ProjectRoleId, cascadeDelete: true)
                .Index(t => t.EmployeeId)
                .Index(t => t.ProjectId)
                .Index(t => t.ProjectRoleId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Budget = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartDate = c.DateTime(nullable: false),
                        ExpectedEndDate = c.DateTime(nullable: false),
                        TotalEndDate = c.DateTime(nullable: false),
                        Ended = c.Boolean(nullable: false),
                        Costs = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CustomerId = c.Int(),
                        ProjectEmployeeMappingId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProjectId)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        Street = c.String(),
                        Postcode = c.Int(nullable: false),
                        City = c.String(),
                        FederalState = c.Int(nullable: false),
                        ContactName = c.String(),
                        Phone = c.String(),
                        Mail = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CustomerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LocationId)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.ProjectWorkingTimes",
                c => new
                    {
                        ProjectWorkingTimeId = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProjectWorkingTimeId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.EmployeeId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.ProjectRoles",
                c => new
                    {
                        ProjectRoleId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ProjectRoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Companies", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.ProjectEmployeeMappings", "ProjectRoleId", "dbo.ProjectRoles");
            DropForeignKey("dbo.ProjectWorkingTimes", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ProjectWorkingTimes", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.ProjectEmployeeMappings", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Projects", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Locations", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.ProjectEmployeeMappings", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Employees", "HealthInsuranceId", "dbo.HealthInsurances");
            DropForeignKey("dbo.Accountings", "SenderId", "dbo.Kontenrahmen");
            DropForeignKey("dbo.Accountings", "ReceiverId", "dbo.Kontenrahmen");
            DropIndex("dbo.ProjectWorkingTimes", new[] { "ProjectId" });
            DropIndex("dbo.ProjectWorkingTimes", new[] { "EmployeeId" });
            DropIndex("dbo.Locations", new[] { "CustomerId" });
            DropIndex("dbo.Projects", new[] { "CustomerId" });
            DropIndex("dbo.ProjectEmployeeMappings", new[] { "ProjectRoleId" });
            DropIndex("dbo.ProjectEmployeeMappings", new[] { "ProjectId" });
            DropIndex("dbo.ProjectEmployeeMappings", new[] { "EmployeeId" });
            DropIndex("dbo.Employees", new[] { "HealthInsuranceId" });
            DropIndex("dbo.Companies", new[] { "EmployeeId" });
            DropIndex("dbo.Accountings", new[] { "ReceiverId" });
            DropIndex("dbo.Accountings", new[] { "SenderId" });
            DropTable("dbo.ProjectRoles");
            DropTable("dbo.ProjectWorkingTimes");
            DropTable("dbo.Locations");
            DropTable("dbo.Customers");
            DropTable("dbo.Projects");
            DropTable("dbo.ProjectEmployeeMappings");
            DropTable("dbo.HealthInsurances");
            DropTable("dbo.Employees");
            DropTable("dbo.Companies");
            DropTable("dbo.Accountings");
        }
    }
}
