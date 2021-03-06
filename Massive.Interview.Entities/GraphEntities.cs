using System;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace Massive.Interview.Entities
{
    /// <summary>
    /// The database context class for entities representing an undirected graph.
    /// </summary>
    public class GraphEntities : DbContext
    {
        private readonly string _connectionString;

        public DbSet<Node> Nodes { get; set; }
        public DbSet<AdjacentNode> AdjacentNodes { get; set; }

        internal GraphEntities() { }

        public GraphEntities(string connectionString)
        {
            _connectionString = connectionString;
        }

        public GraphEntities(DbContextOptions<GraphEntities> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder = optionsBuilder ?? throw new ArgumentNullException(nameof(optionsBuilder));
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder = modelBuilder ?? throw new ArgumentNullException(nameof(modelBuilder));

            modelBuilder.Entity<AdjacentNode>()
                .HasKey(_ => (new { _.LeftNodeId, _.RightNodeId }));
            modelBuilder.Entity<AdjacentNode>()
                .HasIndex(_ => _.LeftNodeId);
            modelBuilder.Entity<AdjacentNode>()
                .HasIndex(_ => _.RightNodeId);

            modelBuilder.Entity<AdjacentNode>()
                .HasOne(_ => _.LeftNode)
                .WithMany(_ => _.RightAdjacentNodes)
                .HasForeignKey(_ => _.LeftNodeId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<AdjacentNode>()
                .HasOne(_ => _.RightNode)
                .WithMany(_ => _.LeftAdjacentNodes)
                .HasForeignKey(_ => _.RightNodeId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}