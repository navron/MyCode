namespace DevOps.GitMergeSwirl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t41 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PrivateBranchToReleaseBranches", "Ahead", c => c.Int());
            AddColumn("dbo.PrivateBranchToReleaseBranches", "Behind", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PrivateBranchToReleaseBranches", "Behind");
            DropColumn("dbo.PrivateBranchToReleaseBranches", "Ahead");
        }
    }
}
