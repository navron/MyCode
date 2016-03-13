namespace DevOps.GitMergeSwirl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PrivateBranchToReleaseBranchMappings", "PrivateBranchSha", c => c.String());
            AddColumn("dbo.PrivateBranchToReleaseBranchMappings", "ReleaseBranchSha", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PrivateBranchToReleaseBranchMappings", "ReleaseBranchSha");
            DropColumn("dbo.PrivateBranchToReleaseBranchMappings", "PrivateBranchSha");
        }
    }
}
