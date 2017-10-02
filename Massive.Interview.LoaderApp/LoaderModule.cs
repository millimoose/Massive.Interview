using System;
using System.IO;
using System.Runtime.CompilerServices;
using Autofac;
using Massive.Interview.Entities.Module;
using Massive.Interview.LoaderApp.Components;
using Massive.Interview.LoaderApp.Services;

[assembly: InternalsVisibleTo("Massive.Interview.LoaderApp.Tests")]

namespace Massive.Interview.LoaderApp
{
    class LoaderModule : Module
    {
        LoaderSettings _settings;

        public LoaderModule(LoaderSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new GraphEntitiesModule(_settings.Entities));
            builder.Register(ctx =>
            {
                INodeDocumentReader reader = ctx.Resolve<INodeDocumentReader>();
                DirectoryInfo directory = new DirectoryInfo(_settings.InputDirectory);
                return new NodeDocumentDirectoryBatch(reader, directory, _settings.Pattern);
            }).AsImplementedInterfaces().AsSelf();

            builder.RegisterType<NodeSynchronizer>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<NodeXmlDocumentReader>().AsImplementedInterfaces().AsSelf();
        }

    }
}
