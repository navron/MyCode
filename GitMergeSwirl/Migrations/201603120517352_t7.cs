namespace DevOps.GitMergeSwirl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PrivateBranchToReleaseBranchMappings", "BaseAhead", c => c.Int());
            AddColumn("dbo.PrivateBranchToReleaseBranchMappings", "BaseBehind", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PrivateBranchToReleaseBranchMappings", "BaseBehind");
            DropColumn("dbo.PrivateBranchToReleaseBranchMappings", "BaseAhead");
        }
    }
}
