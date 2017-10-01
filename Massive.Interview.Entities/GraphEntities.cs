namespace Massive.Interview.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;

    /// <summary>
    /// The database context class for entities representing an undirected graph.
    /// </summary>
    public class GraphEntities : DbContext
    {
        // Your context has been configured to use a 'GraphModel' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Massive.Interview.Model.GraphEntities' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'GraphModel' 
        // connection string in the application configuration file.
        public GraphEntities() : base("name=GraphEntities") {}

        public GraphEntities(string nameOrConnectionString) : base(nameOrConnectionString) {}

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public DbSet<Node> Nodes { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<Node>()
                .HasMany(_ => _.LeftAdjacentNodes)
                .WithMany(_ => _.RightAdjacentNodes)
                .Map(_ =>
                {
                    _.ToTable("AdjacentNodes");
                    _.MapLeftKey("LeftNodeId");
                    _.MapRightKey("RightNodeId");
                });
        }

    }

    /// <summary>
    /// A node of an undirected graph.
    /// </summary>
    public class Node
    {
        public long NodeId { get; set; }

        /// <summary>
        /// A text label for the node.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The nodes adjacent to this node to its "left" - with a lesser id.
        /// </summary>
        public virtual ICollection<Node> LeftAdjacentNodes { get; } = new HashSet<Node>();

        /// <summary>
        /// The nodes adjacent to this node to its "right" - with a greater id. 
        /// </summary>
        public virtual ICollection<Node> RightAdjacentNodes { get; } = new HashSet<Node>();
    }
    
}