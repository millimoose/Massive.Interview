using Autofac;
using System;
using Microsoft.EntityFrameworkCore;

namespace Massive.Interview.Entities.Module
{
    /// <summary>
    /// An Autofac module that registers the EF <c>DbContext</c> for the graph model.
    /// </summary>
    public class GraphEntitiesModule : Autofac.Module
    {
        public GraphEntitiesSettings Settings { get; }

        public GraphEntitiesModule(GraphEntitiesSettings settings)
        {
            Settings = settings;
        }
        
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(ctx => new GraphEntities(Settings.ConnectionString)).InstancePerLifetimeScope()
                .AsSelf();
        }
    }
}