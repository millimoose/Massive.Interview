using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

using Massive.Interview.Support;
using Microsoft.EntityFrameworkCore.Design;

namespace Massive.Interview.Entities
{
    /// <summary>
    /// The database context class for entities representing an undirected graph.
    /// </summary>
    public class GraphEntities : DbContext
    {
        private readonly string _connectionString;

        public DbSet<Node> Nodes { get; set; }

        internal GraphEntities() { }

        public GraphEntities(string connectionString) {
            _connectionString = connectionString;
        }

        public GraphEntities(DbContextOptions<GraphEntities> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var adjacentNode = modelBuilder.Entity<AdjacentNode>();
            adjacentNode.HasKey(_ => (new { _.LeftNodeId, _.RightNodeId }));
            adjacentNode.HasOne(_ => _.LeftNode).WithMany(_ => _.RightAdjacentNodes).HasForeignKey(_ => _.LeftNodeId);
            adjacentNode.HasOne(_ => _.RightNode).WithMany(_ => _.LeftAdjacentNodes).HasForeignKey(_ => _.RightNodeId);
        }
    }
}