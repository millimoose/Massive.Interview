using Autofac;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Massive.Interview.Entities.Module
{
    /// <summary>
    /// An Autofac module that registers the EF <c>DbContext</c> for the graph model.
    /// </summary>
    public class GraphEntitiesModule : Autofac.Module
    {
        /// <summary>
        /// The connection string to initialize the <c>DbContext</c>.
        /// </summary>
        /// If this property is not set, uses the default connection string.
        [NotNull]
        public string ConnectionString { get; set; }

        private GraphEntities NewDbContext()
        {
            return string.IsNullOrEmpty(ConnectionString) 
                ? new GraphEntities()
                : new GraphEntities(ConnectionString);
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(ctx => NewDbContext())
                .InstancePerMatchingLifetimeScope(nameof(DbContext))
                .AsSelf();
        }
    }
}