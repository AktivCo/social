namespace SocialPackage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bids",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        Comment = c.String(),
                        ImageUrl = c.String(nullable: false),
                        Summ = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FixBid = c.Boolean(nullable: false),
                        BidSender_id = c.Int(),
                        BidUser_id = c.Int(),
                        category_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Users", t => t.BidSender_id)
                .ForeignKey("dbo.Users", t => t.BidUser_id)
                .ForeignKey("dbo.Categories", t => t.category_id)
                .Index(t => t.BidSender_id)
                .Index(t => t.BidUser_id)
                .Index(t => t.category_id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false),
                        Email = c.String(),
                        Login = c.String(),
                        Limit_Id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Limits", t => t.Limit_Id)
                .Index(t => t.Limit_Id);
            
            CreateTable(
                "dbo.Limits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Proezd = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Fitnes = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CultureEvents = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Color = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Rules = c.String(),
                        Email = c.String(),
                        Domain = c.String(nullable: false),
                        Host = c.String(nullable: false),
                        Port = c.Int(nullable: false),
                        ServiceEmailUser = c.String(nullable: false),
                        ServiceEmailPassword = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.UsersRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.UserRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bids", "category_id", "dbo.Categories");
            DropForeignKey("dbo.Bids", "BidUser_id", "dbo.Users");
            DropForeignKey("dbo.Bids", "BidSender_id", "dbo.Users");
            DropForeignKey("dbo.UsersRoles", "RoleId", "dbo.UserRoles");
            DropForeignKey("dbo.UsersRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "Limit_Id", "dbo.Limits");
            DropIndex("dbo.UsersRoles", new[] { "RoleId" });
            DropIndex("dbo.UsersRoles", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "Limit_Id" });
            DropIndex("dbo.Bids", new[] { "category_id" });
            DropIndex("dbo.Bids", new[] { "BidUser_id" });
            DropIndex("dbo.Bids", new[] { "BidSender_id" });
            DropTable("dbo.UsersRoles");
            DropTable("dbo.Settings");
            DropTable("dbo.Categories");
            DropTable("dbo.UserRoles");
            DropTable("dbo.Limits");
            DropTable("dbo.Users");
            DropTable("dbo.Bids");
        }
    }
}
