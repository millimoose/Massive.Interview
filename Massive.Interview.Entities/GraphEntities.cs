using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

using Massive.Interview.Support;

namespace Massive.Interview.Entities
{
    /// <summary>
    /// The database context class for entities representing an undirected graph.
    /// </summary>
    public class GraphEntities : DbContext
    {
        private readonly string _connectionString;

        public DbSet<Node> Nodes { get; set; }

        public GraphEntities(string connectionString) : base() {
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

    /// <summary>
    /// A node of an undirected graph.
    /// </summary>
    public class Node : IFormattable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long? NodeId { get; set; }

        /// <summary>
        /// A text label for the node.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The nodes adjacent to this node to its "left" - with a lesser id.
        /// </summary>
        public ICollection<AdjacentNode> LeftAdjacentNodes { get; } = new List<AdjacentNode>(0);

        /// <summary>
        /// The nodes adjacent to this node to its "right" - with a greater id. 
        /// </summary>
        public ICollection<AdjacentNode> RightAdjacentNodes { get; } = new List<AdjacentNode>(0);

        public override string ToString()
        {
            return ToString("G");
        }

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }
        public string ToString(string format, IFormatProvider formatProvider)
        {
            format = format ?? "G";
            formatProvider = formatProvider ?? CultureInfo.CurrentCulture;
            
            switch (format)
            {
                // short format
                case "g":
                    return base.ToString() + new { NodeId, Label };
                case "G":
                default:
                    return base.ToString() + new {
                        NodeId,
                        Label,
                        LeftAdjacentNodes = LeftAdjacentNodes.ToShortString(),
                        RightAdjacentNodes = RightAdjacentNodes.ToShortString()
                    };
            }
        }
    }
    
    /// <summary>
    /// Join entity representing vertices between nodes.
    /// </summary>
    public class AdjacentNode : IFormattable
    {
        /// <summary>
        /// ID of the node on the left side of the vertex. (The node with the lesser ID.)
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long LeftNodeId { get; set; }

        /// <summary>
        /// The node on the left side of the vertex. (The node with the lesser ID.)
        /// </summary>
        [Required]
        public Node LeftNode { get; set; }

        /// <summary>
        /// ID of the node on the left side of the vertex. (The node with the greater ID.)
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long RightNodeId { get; set; }

        /// <summary>
        /// The node on the left side of the vertex. (The node with the greater ID.)
        /// </summary>
        [Required]
        public Node RightNode { get; set; }

        public override string ToString()
        {
            return ToString("G");
        }

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }
        public string ToString(string format, IFormatProvider formatProvider)
        {
            format = format ?? "G";
            formatProvider = formatProvider ?? CultureInfo.CurrentCulture;

            switch (format)
            {
                // short format
                case "g":
                    return base.ToString() + new { LeftNodeId, RightNodeId };
                case "G":
                default:
                    return base.ToString() + new
                    {
                        LeftNode = LeftNode.ToString("g"),
                        RightNode = RightNode.ToString("g")
                    };
            }
        }
    }
}