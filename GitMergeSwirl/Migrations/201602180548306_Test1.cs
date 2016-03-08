using System.Data.Entity.Migrations;

namespace DevOps.GitMergeSwirl.Migrations
{
    public partial class Test1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Branches", "InDatabase");
            DropColumn("dbo.Branches", "BeenUpdated");
            DropColumn("dbo.Branches", "DeleteInDatabase");
            DropColumn("dbo.Branches", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Branches", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Branches", "DeleteInDatabase", c => c.Boolean());
            AddColumn("dbo.Branches", "BeenUpdated", c => c.Boolean());
            AddColumn("dbo.Branches", "InDatabase", c => c.Boolean());
        }
    }
}
