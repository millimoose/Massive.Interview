using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Massive.Interview.Entities.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Nodes",
                columns: table => new
                {
                    NodeId = table.Column<long>(type: "bigint", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nodes", x => x.NodeId);
                });

            migrationBuilder.CreateTable(
                name: "AdjacentNodes",
                columns: table => new
                {
                    LeftNodeId = table.Column<long>(type: "bigint", nullable: false),
                    RightNodeId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdjacentNodes", x => new { x.LeftNodeId, x.RightNodeId });
                    table.ForeignKey(
                        name: "FK_AdjacentNodes_Nodes_LeftNodeId",
                        column: x => x.LeftNodeId,
                        principalTable: "Nodes",
                        principalColumn: "NodeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdjacentNodes_Nodes_RightNodeId",
                        column: x => x.RightNodeId,
                        principalTable: "Nodes",
                        principalColumn: "NodeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdjacentNodes_RightNodeId",
                table: "AdjacentNodes",
                column: "RightNodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdjacentNodes");

            migrationBuilder.DropTable(
                name: "Nodes");
        }
    }
}
