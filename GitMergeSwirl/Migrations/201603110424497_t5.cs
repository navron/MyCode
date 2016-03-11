namespace DevOps.GitMergeSwirl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t5 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PrivateBranchToReleaseBranches", newName: "PrivateBranchToReleaseBranchMappings");
            AddColumn("dbo.PrivateBranchToReleaseBranchMappings", "BranchType", c => c.Int(nullable: false));
            DropColumn("dbo.PrivateBranchToReleaseBranchMappings", "CommitCountToBaseCommit");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PrivateBranchToReleaseBranchMappings", "CommitCountToBaseCommit", c => c.Int());
            DropColumn("dbo.PrivateBranchToReleaseBranchMappings", "BranchType");
            RenameTable(name: "dbo.PrivateBranchToReleaseBranchMappings", newName: "PrivateBranchToReleaseBranches");
        }
    }
}
