namespace App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stepstableadded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Steps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StepName = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ImagePath = c.String(),
                        StepsCount = c.Int(nullable: false),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Steps");
        }
    }
}
