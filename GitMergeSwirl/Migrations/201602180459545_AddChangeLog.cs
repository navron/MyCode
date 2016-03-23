using System.Data.Entity.Migrations;

namespace DevOps.GitMergeSwirl.Migrations
{
    public partial class AddChangeLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChangeLogs",
                c => new
                    {
                        ChangeId = c.Int(false, true),
                        Who = c.String(),
                        When = c.DateTime(false),
                        What = c.String()
                    })
                .PrimaryKey(t => t.ChangeId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ChangeLogs");
        }
    }
}
