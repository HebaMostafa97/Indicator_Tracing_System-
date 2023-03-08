namespace ITS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ITS_LKP_Country",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name_A = c.String(nullable: false, maxLength: 255),
                        Name_E = c.String(maxLength: 255),
                        Description_A = c.String(maxLength: 2048),
                        Description_E = c.String(maxLength: 2048),
                        IsDefault = c.Int(nullable: false),
                        OwnerID = c.Int(nullable: false),
                        ModifierID = c.Int(),
                        CreateDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifyDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        SortIndex = c.Int(nullable: false),
                        Focus = c.Int(nullable: false),
                        Active = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ITS_User", t => t.ModifierID)
                .ForeignKey("dbo.ITS_User", t => t.OwnerID)
                .Index(t => t.OwnerID)
                .Index(t => t.ModifierID)
                .Index(t => t.CreateDate, unique: true);
            
            CreateTable(
                "dbo.ITS_User",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        NameNormalized = c.String(nullable: false, maxLength: 255),
                        JobTitle = c.String(maxLength: 255),
                        Notes = c.String(maxLength: 4000),
                        Email = c.String(nullable: false, maxLength: 255, unicode: false),
                        HomePage = c.String(maxLength: 255),
                        Photo = c.String(maxLength: 128),
                        Username = c.String(nullable: false, maxLength: 64),
                        Password = c.Binary(nullable: false, maxLength: 100),
                        TempPassword = c.Binary(maxLength: 100),
                        LastLogonDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        LastLogoutDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        LogInCount = c.Int(),
                        Admin = c.Int(),
                        OwnerID = c.Int(nullable: false),
                        ModifierID = c.Int(),
                        CreateDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifyDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        SortIndex = c.Int(nullable: false),
                        Focus = c.Int(nullable: false),
                        Active = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ITS_User", t => t.ModifierID)
                .ForeignKey("dbo.ITS_User", t => t.OwnerID)
                .Index(t => t.Email, unique: true)
                .Index(t => t.Username, unique: true)
                .Index(t => t.OwnerID)
                .Index(t => t.ModifierID)
                .Index(t => t.CreateDate, unique: true);
            
            CreateTable(
                "dbo.ITS_Group",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        NameNormalized = c.String(nullable: false, maxLength: 255),
                        Description = c.String(maxLength: 2048),
                        Email = c.String(maxLength: 255, unicode: false),
                        HomePage = c.String(maxLength: 512),
                        OwnerID = c.Int(nullable: false),
                        ModifierID = c.Int(),
                        CreateDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifyDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        SortIndex = c.Int(nullable: false),
                        Focus = c.Int(nullable: false),
                        Active = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ITS_User", t => t.ModifierID)
                .ForeignKey("dbo.ITS_User", t => t.OwnerID)
                .Index(t => t.OwnerID)
                .Index(t => t.ModifierID)
                .Index(t => t.CreateDate, unique: true);
            
            CreateTable(
                "dbo.ITS_GroupPermission",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OwnerID = c.Int(),
                        ModifierID = c.Int(),
                        CreateDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifyDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        FunctionName = c.String(nullable: false, maxLength: 255),
                        GroupID = c.Int(nullable: false),
                        Permission = c.Int(nullable: false),
                        Restricted = c.Int(nullable: false),
                        Objectname = c.String(nullable: false, maxLength: 64),
                        SortIndex = c.Int(nullable: false),
                        Focus = c.Int(nullable: false),
                        Active = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ITS_Group", t => t.GroupID, cascadeDelete: true)
                .Index(t => t.GroupID);
            
            CreateTable(
                "dbo.ITS_IndicatorAttachment",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ReportName = c.String(nullable: false, maxLength: 255),
                        Attachment = c.String(nullable: false, maxLength: 128),
                        AttachmentTitle = c.String(nullable: false, maxLength: 255),
                        AttachmentUrl = c.String(nullable: false, maxLength: 512),
                        Keyword = c.String(nullable: false, maxLength: 255),
                        Description = c.String(maxLength: 2048),
                        UploadDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ReleaseDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IndicatorID = c.Int(nullable: false),
                        OwnerID = c.Int(nullable: false),
                        ModifierID = c.Int(),
                        CreateDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifyDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        SortIndex = c.Int(nullable: false),
                        Focus = c.Int(nullable: false),
                        Active = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ITS_Indicator", t => t.IndicatorID, cascadeDelete: true)
                .ForeignKey("dbo.ITS_User", t => t.ModifierID)
                .ForeignKey("dbo.ITS_User", t => t.OwnerID)
                .Index(t => t.IndicatorID)
                .Index(t => t.OwnerID)
                .Index(t => t.ModifierID)
                .Index(t => t.CreateDate, unique: true);
            
            CreateTable(
                "dbo.ITS_Indicator",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name_A = c.String(nullable: false, maxLength: 255),
                        Name_E = c.String(nullable: false, maxLength: 255),
                        ReportName_A = c.String(nullable: false, maxLength: 255),
                        ReportName_E = c.String(maxLength: 255),
                        Description_A = c.String(maxLength: 2048),
                        Description_E = c.String(maxLength: 2048),
                        Notes_A = c.String(maxLength: 4000),
                        Notes_E = c.String(maxLength: 4000),
                        WebsiteUrl = c.String(nullable: false, maxLength: 512),
                        IndexUrl = c.String(nullable: false, maxLength: 512),
                        Abbreviation = c.String(nullable: false, maxLength: 16),
                        StartMonth = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        EndMonth = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        PublisherID = c.Int(nullable: false),
                        PeriodicityID = c.Int(nullable: false),
                        OwnerID = c.Int(nullable: false),
                        ModifierID = c.Int(),
                        CreateDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifyDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        SortIndex = c.Int(nullable: false),
                        Focus = c.Int(nullable: false),
                        Active = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ITS_User", t => t.ModifierID)
                .ForeignKey("dbo.ITS_User", t => t.OwnerID)
                .ForeignKey("dbo.ITS_LKP_Periodicity", t => t.PeriodicityID, cascadeDelete: true)
                .ForeignKey("dbo.ITS_Publisher", t => t.PublisherID, cascadeDelete: true)
                .Index(t => t.Abbreviation, unique: true)
                .Index(t => t.PublisherID)
                .Index(t => t.PeriodicityID)
                .Index(t => t.OwnerID)
                .Index(t => t.ModifierID)
                .Index(t => t.CreateDate, unique: true);
            
            CreateTable(
                "dbo.ITS_LKP_Issue",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ParentID = c.Int(),
                        Name_A = c.String(nullable: false, maxLength: 255),
                        Name_E = c.String(maxLength: 255),
                        Description_A = c.String(maxLength: 2048),
                        Description_E = c.String(maxLength: 2048),
                        IsDefault = c.Int(nullable: false),
                        OwnerID = c.Int(nullable: false),
                        ModifierID = c.Int(),
                        CreateDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifyDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        SortIndex = c.Int(nullable: false),
                        Focus = c.Int(nullable: false),
                        Active = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ITS_LKP_Issue", t => t.ParentID)
                .ForeignKey("dbo.ITS_User", t => t.ModifierID)
                .ForeignKey("dbo.ITS_User", t => t.OwnerID)
                .Index(t => t.ParentID)
                .Index(t => t.OwnerID)
                .Index(t => t.ModifierID)
                .Index(t => t.CreateDate, unique: true);
            
            CreateTable(
                "dbo.ITS_LKP_Periodicity",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name_A = c.String(nullable: false, maxLength: 255),
                        Name_E = c.String(maxLength: 255),
                        Description_A = c.String(maxLength: 2048),
                        Description_E = c.String(maxLength: 2048),
                        IsDefault = c.Int(nullable: false),
                        OwnerID = c.Int(nullable: false),
                        ModifierID = c.Int(),
                        CreateDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifyDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        SortIndex = c.Int(nullable: false),
                        Focus = c.Int(nullable: false),
                        Active = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ITS_User", t => t.ModifierID)
                .ForeignKey("dbo.ITS_User", t => t.OwnerID)
                .Index(t => t.OwnerID)
                .Index(t => t.ModifierID)
                .Index(t => t.CreateDate, unique: true);
            
            CreateTable(
                "dbo.ITS_Publisher",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name_A = c.String(nullable: false, maxLength: 255),
                        Name_E = c.String(nullable: false, maxLength: 255),
                        Description_A = c.String(maxLength: 2048),
                        Description_E = c.String(maxLength: 2048),
                        HomePage = c.String(maxLength: 512),
                        CountryID = c.Int(nullable: false),
                        OwnerID = c.Int(nullable: false),
                        ModifierID = c.Int(),
                        CreateDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifyDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        SortIndex = c.Int(nullable: false),
                        Focus = c.Int(nullable: false),
                        Active = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ITS_LKP_Country", t => t.CountryID, cascadeDelete: true)
                .ForeignKey("dbo.ITS_User", t => t.ModifierID)
                .ForeignKey("dbo.ITS_User", t => t.OwnerID)
                .Index(t => t.CountryID)
                .Index(t => t.OwnerID)
                .Index(t => t.ModifierID)
                .Index(t => t.CreateDate, unique: true);
            
            CreateTable(
                "dbo.ITS_LKP_Sector",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ParentID = c.Int(),
                        Name_A = c.String(nullable: false, maxLength: 255),
                        Name_E = c.String(maxLength: 255),
                        Description_A = c.String(maxLength: 2048),
                        Description_E = c.String(maxLength: 2048),
                        IsDefault = c.Int(nullable: false),
                        OwnerID = c.Int(nullable: false),
                        ModifierID = c.Int(),
                        CreateDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifyDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        SortIndex = c.Int(nullable: false),
                        Focus = c.Int(nullable: false),
                        Active = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ITS_LKP_Sector", t => t.ParentID)
                .ForeignKey("dbo.ITS_User", t => t.ModifierID)
                .ForeignKey("dbo.ITS_User", t => t.OwnerID)
                .Index(t => t.ParentID)
                .Index(t => t.OwnerID)
                .Index(t => t.ModifierID)
                .Index(t => t.CreateDate, unique: true);
            
            CreateTable(
                "dbo.ITS_LNK_User_Group",
                c => new
                    {
                        UserID = c.Int(nullable: false),
                        GroupID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserID, t.GroupID })
                .ForeignKey("dbo.ITS_User", t => t.UserID, cascadeDelete: true)
                .ForeignKey("dbo.ITS_Group", t => t.GroupID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.GroupID);
            
            CreateTable(
                "dbo.ITS_LNK_Indicator_Issue",
                c => new
                    {
                        IndicatorID = c.Int(nullable: false),
                        IssueID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IndicatorID, t.IssueID })
                .ForeignKey("dbo.ITS_Indicator", t => t.IndicatorID, cascadeDelete: true)
                .ForeignKey("dbo.ITS_LKP_Issue", t => t.IssueID, cascadeDelete: true)
                .Index(t => t.IndicatorID)
                .Index(t => t.IssueID);
            
            CreateTable(
                "dbo.ITS_LNK_Indicator_Sector",
                c => new
                    {
                        IndicatorID = c.Int(nullable: false),
                        SectorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IndicatorID, t.SectorID })
                .ForeignKey("dbo.ITS_Indicator", t => t.IndicatorID, cascadeDelete: true)
                .ForeignKey("dbo.ITS_LKP_Sector", t => t.SectorID, cascadeDelete: true)
                .Index(t => t.IndicatorID)
                .Index(t => t.SectorID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ITS_LKP_Country", "OwnerID", "dbo.ITS_User");
            DropForeignKey("dbo.ITS_LKP_Country", "ModifierID", "dbo.ITS_User");
            DropForeignKey("dbo.ITS_User", "OwnerID", "dbo.ITS_User");
            DropForeignKey("dbo.ITS_User", "ModifierID", "dbo.ITS_User");
            DropForeignKey("dbo.ITS_IndicatorAttachment", "OwnerID", "dbo.ITS_User");
            DropForeignKey("dbo.ITS_IndicatorAttachment", "ModifierID", "dbo.ITS_User");
            DropForeignKey("dbo.ITS_LNK_Indicator_Sector", "SectorID", "dbo.ITS_LKP_Sector");
            DropForeignKey("dbo.ITS_LNK_Indicator_Sector", "IndicatorID", "dbo.ITS_Indicator");
            DropForeignKey("dbo.ITS_LKP_Sector", "OwnerID", "dbo.ITS_User");
            DropForeignKey("dbo.ITS_LKP_Sector", "ModifierID", "dbo.ITS_User");
            DropForeignKey("dbo.ITS_LKP_Sector", "ParentID", "dbo.ITS_LKP_Sector");
            DropForeignKey("dbo.ITS_Publisher", "OwnerID", "dbo.ITS_User");
            DropForeignKey("dbo.ITS_Publisher", "ModifierID", "dbo.ITS_User");
            DropForeignKey("dbo.ITS_Indicator", "PublisherID", "dbo.ITS_Publisher");
            DropForeignKey("dbo.ITS_Publisher", "CountryID", "dbo.ITS_LKP_Country");
            DropForeignKey("dbo.ITS_LKP_Periodicity", "OwnerID", "dbo.ITS_User");
            DropForeignKey("dbo.ITS_LKP_Periodicity", "ModifierID", "dbo.ITS_User");
            DropForeignKey("dbo.ITS_Indicator", "PeriodicityID", "dbo.ITS_LKP_Periodicity");
            DropForeignKey("dbo.ITS_Indicator", "OwnerID", "dbo.ITS_User");
            DropForeignKey("dbo.ITS_Indicator", "ModifierID", "dbo.ITS_User");
            DropForeignKey("dbo.ITS_LNK_Indicator_Issue", "IssueID", "dbo.ITS_LKP_Issue");
            DropForeignKey("dbo.ITS_LNK_Indicator_Issue", "IndicatorID", "dbo.ITS_Indicator");
            DropForeignKey("dbo.ITS_LKP_Issue", "OwnerID", "dbo.ITS_User");
            DropForeignKey("dbo.ITS_LKP_Issue", "ModifierID", "dbo.ITS_User");
            DropForeignKey("dbo.ITS_LKP_Issue", "ParentID", "dbo.ITS_LKP_Issue");
            DropForeignKey("dbo.ITS_IndicatorAttachment", "IndicatorID", "dbo.ITS_Indicator");
            DropForeignKey("dbo.ITS_LNK_User_Group", "GroupID", "dbo.ITS_Group");
            DropForeignKey("dbo.ITS_LNK_User_Group", "UserID", "dbo.ITS_User");
            DropForeignKey("dbo.ITS_Group", "OwnerID", "dbo.ITS_User");
            DropForeignKey("dbo.ITS_Group", "ModifierID", "dbo.ITS_User");
            DropForeignKey("dbo.ITS_GroupPermission", "GroupID", "dbo.ITS_Group");
            DropIndex("dbo.ITS_LNK_Indicator_Sector", new[] { "SectorID" });
            DropIndex("dbo.ITS_LNK_Indicator_Sector", new[] { "IndicatorID" });
            DropIndex("dbo.ITS_LNK_Indicator_Issue", new[] { "IssueID" });
            DropIndex("dbo.ITS_LNK_Indicator_Issue", new[] { "IndicatorID" });
            DropIndex("dbo.ITS_LNK_User_Group", new[] { "GroupID" });
            DropIndex("dbo.ITS_LNK_User_Group", new[] { "UserID" });
            DropIndex("dbo.ITS_LKP_Sector", new[] { "CreateDate" });
            DropIndex("dbo.ITS_LKP_Sector", new[] { "ModifierID" });
            DropIndex("dbo.ITS_LKP_Sector", new[] { "OwnerID" });
            DropIndex("dbo.ITS_LKP_Sector", new[] { "ParentID" });
            DropIndex("dbo.ITS_Publisher", new[] { "CreateDate" });
            DropIndex("dbo.ITS_Publisher", new[] { "ModifierID" });
            DropIndex("dbo.ITS_Publisher", new[] { "OwnerID" });
            DropIndex("dbo.ITS_Publisher", new[] { "CountryID" });
            DropIndex("dbo.ITS_LKP_Periodicity", new[] { "CreateDate" });
            DropIndex("dbo.ITS_LKP_Periodicity", new[] { "ModifierID" });
            DropIndex("dbo.ITS_LKP_Periodicity", new[] { "OwnerID" });
            DropIndex("dbo.ITS_LKP_Issue", new[] { "CreateDate" });
            DropIndex("dbo.ITS_LKP_Issue", new[] { "ModifierID" });
            DropIndex("dbo.ITS_LKP_Issue", new[] { "OwnerID" });
            DropIndex("dbo.ITS_LKP_Issue", new[] { "ParentID" });
            DropIndex("dbo.ITS_Indicator", new[] { "CreateDate" });
            DropIndex("dbo.ITS_Indicator", new[] { "ModifierID" });
            DropIndex("dbo.ITS_Indicator", new[] { "OwnerID" });
            DropIndex("dbo.ITS_Indicator", new[] { "PeriodicityID" });
            DropIndex("dbo.ITS_Indicator", new[] { "PublisherID" });
            DropIndex("dbo.ITS_Indicator", new[] { "Abbreviation" });
            DropIndex("dbo.ITS_IndicatorAttachment", new[] { "CreateDate" });
            DropIndex("dbo.ITS_IndicatorAttachment", new[] { "ModifierID" });
            DropIndex("dbo.ITS_IndicatorAttachment", new[] { "OwnerID" });
            DropIndex("dbo.ITS_IndicatorAttachment", new[] { "IndicatorID" });
            DropIndex("dbo.ITS_GroupPermission", new[] { "GroupID" });
            DropIndex("dbo.ITS_Group", new[] { "CreateDate" });
            DropIndex("dbo.ITS_Group", new[] { "ModifierID" });
            DropIndex("dbo.ITS_Group", new[] { "OwnerID" });
            DropIndex("dbo.ITS_User", new[] { "CreateDate" });
            DropIndex("dbo.ITS_User", new[] { "ModifierID" });
            DropIndex("dbo.ITS_User", new[] { "OwnerID" });
            DropIndex("dbo.ITS_User", new[] { "Username" });
            DropIndex("dbo.ITS_User", new[] { "Email" });
            DropIndex("dbo.ITS_LKP_Country", new[] { "CreateDate" });
            DropIndex("dbo.ITS_LKP_Country", new[] { "ModifierID" });
            DropIndex("dbo.ITS_LKP_Country", new[] { "OwnerID" });
            DropTable("dbo.ITS_LNK_Indicator_Sector");
            DropTable("dbo.ITS_LNK_Indicator_Issue");
            DropTable("dbo.ITS_LNK_User_Group");
            DropTable("dbo.ITS_LKP_Sector");
            DropTable("dbo.ITS_Publisher");
            DropTable("dbo.ITS_LKP_Periodicity");
            DropTable("dbo.ITS_LKP_Issue");
            DropTable("dbo.ITS_Indicator");
            DropTable("dbo.ITS_IndicatorAttachment");
            DropTable("dbo.ITS_GroupPermission");
            DropTable("dbo.ITS_Group");
            DropTable("dbo.ITS_User");
            DropTable("dbo.ITS_LKP_Country");
        }
    }
}
