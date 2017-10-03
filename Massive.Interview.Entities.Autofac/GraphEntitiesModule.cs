using System;
using System.Runtime.InteropServices;
using Autofac;

namespace Massive.Interview.Entities.Autofac
{
    /// <summary>
    /// An Autofac module that registers the EF <see cref="DbContext"/> for the graph model.
    /// </summary>
    public class GraphEntitiesModule : global::Autofac.Module
    {
        public GraphEntitiesSettings Settings { get; }

        public GraphEntitiesModule(GraphEntitiesSettings settings)
        {
            Settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(ctx => new GraphEntities(Settings.ConnectionString))
                .InstancePerLifetimeScope()
                .AsSelf();
        }
    }
}