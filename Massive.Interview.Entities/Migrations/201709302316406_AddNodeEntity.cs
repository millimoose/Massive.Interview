namespace Massive.Interview.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNodeEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Nodes",
                c => new
                    {
                        NodeId = c.Long(nullable: false, identity: true),
                        Label = c.String(),
                    })
                .PrimaryKey(t => t.NodeId);
            
            CreateTable(
                "dbo.AdjacentNodes",
                c => new
                    {
                        LeftNodeId = c.Long(nullable: false),
                        RightNodeId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.LeftNodeId, t.RightNodeId })
                .ForeignKey("dbo.Nodes", t => t.LeftNodeId)
                .ForeignKey("dbo.Nodes", t => t.RightNodeId)
                .Index(t => t.LeftNodeId)
                .Index(t => t.RightNodeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AdjacentNodes", "RightNodeId", "dbo.Nodes");
            DropForeignKey("dbo.AdjacentNodes", "LeftNodeId", "dbo.Nodes");
            DropIndex("dbo.AdjacentNodes", new[] { "RightNodeId" });
            DropIndex("dbo.AdjacentNodes", new[] { "LeftNodeId" });
            DropTable("dbo.AdjacentNodes");
            DropTable("dbo.Nodes");
        }
    }
}
