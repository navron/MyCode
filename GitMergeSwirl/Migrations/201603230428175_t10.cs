namespace DevOps.GitMergeSwirl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t10 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PrivateBranchToReleaseBranchMappings", newName: "PrivateToReleaseMappings");
            RenameTable(name: "dbo.ReleaseParentMappings", newName: "ReleaseMappings");
            DropColumn("dbo.PrivateToReleaseMappings", "BranchType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PrivateToReleaseMappings", "BranchType", c => c.Int(nullable: false));
            RenameTable(name: "dbo.ReleaseMappings", newName: "ReleaseParentMappings");
            RenameTable(name: "dbo.PrivateToReleaseMappings", newName: "PrivateBranchToReleaseBranchMappings");
        }
    }
}
