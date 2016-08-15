namespace DHNet.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _new : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TestTable",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        x = c.Decimal(nullable: false, precision: 18, scale: 2),
                        y = c.Decimal(nullable: false, precision: 18, scale: 2),
                        z = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TestTable");
        }
    }
}
