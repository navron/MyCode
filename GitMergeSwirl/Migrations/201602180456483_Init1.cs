using System.Data.Entity.Migrations;

namespace DevOps.GitMergeSwirl.Migrations
{
    public partial class Init1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BranchCommits",
                c => new
                    {
                        BranchCommitSha = c.Int(false, true),
                        Sha = c.String(),
                        CommitComment = c.String(),
                        JiraTaskId = c.String(),
                        Branch_CanonicalName = c.String(maxLength: 128)
                    })
                .PrimaryKey(t => t.BranchCommitSha)
                .ForeignKey("dbo.Branches", t => t.Branch_CanonicalName)
                .Index(t => t.Branch_CanonicalName);
            
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        CanonicalName = c.String(false, 128),
                        ShaTip = c.String(),
                        ReleaseParentCanonicalName = c.String(),
                        ReleaseParentSha = c.String(),
                        BranchType = c.Int(false),
                        MergedTested = c.Boolean(false),
                        InDatabase = c.Boolean(),
                        BeenUpdated = c.Boolean(),
                        DeleteInDatabase = c.Boolean(),
                        Discriminator = c.String(false, 128)
                    })
                .PrimaryKey(t => t.CanonicalName);
            
            CreateTable(
                "dbo.BranchMergeTestResults",
                c => new
                    {
                        TestId = c.Int(false, true),
                        ReleaseBranchCanonicalName = c.String(),
                        ReleaseBranchSha = c.String(),
                        Result = c.Boolean(),
                        ResultLog = c.String(),
                        BranchMergeTest_CanonicalName = c.String(maxLength: 128)
                    })
                .PrimaryKey(t => t.TestId)
                .ForeignKey("dbo.BranchMergeTests", t => t.BranchMergeTest_CanonicalName)
                .Index(t => t.BranchMergeTest_CanonicalName);
            
            CreateTable(
                "dbo.BranchMergeTests",
                c => new
                    {
                        CanonicalName = c.String(false, 128),
                        Result = c.Boolean()
                    })
                .PrimaryKey(t => t.CanonicalName);
            
            CreateTable(
                "dbo.ReleaseParentMappings",
                c => new
                    {
                        ReleaseName = c.String(false, 128),
                        ParentName = c.String(false, 128)
                    })
                .PrimaryKey(t => new { t.ReleaseName, t.ParentName });
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BranchMergeTestResults", "BranchMergeTest_CanonicalName", "dbo.BranchMergeTests");
            DropForeignKey("dbo.BranchCommits", "Branch_CanonicalName", "dbo.Branches");
            DropIndex("dbo.BranchMergeTestResults", new[] { "BranchMergeTest_CanonicalName" });
            DropIndex("dbo.BranchCommits", new[] { "Branch_CanonicalName" });
            DropTable("dbo.ReleaseParentMappings");
            DropTable("dbo.BranchMergeTests");
            DropTable("dbo.BranchMergeTestResults");
            DropTable("dbo.Branches");
            DropTable("dbo.BranchCommits");
        }
    }
}
