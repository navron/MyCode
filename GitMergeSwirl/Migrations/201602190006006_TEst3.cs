namespace DevOps.GitMergeSwirl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TEst3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PrivateBranchToReleaseBranches",
                c => new
                    {
                        PrivateBranchCanonicalName = c.String(nullable: false, maxLength: 128),
                        ReleaseBranchCanonicalName = c.String(nullable: false, maxLength: 128),
                        CommonCommitSha = c.String(),
                        CommitCountToCommonCommit = c.Int(),
                        Branch_CanonicalName = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.PrivateBranchCanonicalName, t.ReleaseBranchCanonicalName })
                .ForeignKey("dbo.Branches", t => t.Branch_CanonicalName)
                .Index(t => t.Branch_CanonicalName);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PrivateBranchToReleaseBranches", "Branch_CanonicalName", "dbo.Branches");
            DropIndex("dbo.PrivateBranchToReleaseBranches", new[] { "Branch_CanonicalName" });
            DropTable("dbo.PrivateBranchToReleaseBranches");
        }
    }
}
