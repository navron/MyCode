namespace DevOps.GitMergeSwirl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PrivateBranchToReleaseBranches", "BaseCommitSha", c => c.String());
            AddColumn("dbo.PrivateBranchToReleaseBranches", "CommitCountToBaseCommit", c => c.Int());
            DropColumn("dbo.PrivateBranchToReleaseBranches", "CommonCommitSha");
            DropColumn("dbo.PrivateBranchToReleaseBranches", "CommitCountToCommonCommit");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PrivateBranchToReleaseBranches", "CommitCountToCommonCommit", c => c.Int());
            AddColumn("dbo.PrivateBranchToReleaseBranches", "CommonCommitSha", c => c.String());
            DropColumn("dbo.PrivateBranchToReleaseBranches", "CommitCountToBaseCommit");
            DropColumn("dbo.PrivateBranchToReleaseBranches", "BaseCommitSha");
        }
    }
}
