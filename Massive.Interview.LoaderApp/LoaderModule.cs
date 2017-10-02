using System;
using System.IO;
using Autofac;
using Autofac.Integration.Wcf;
using Massive.Interview.LoaderApp.Remote;
using Massive.Interview.LoaderApp.Support;

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
            builder.Register(ctx =>
            {
                INodeDocumentReader reader = ctx.Resolve<INodeDocumentReader>();
                DirectoryInfo directory = new DirectoryInfo(_settings.InputDirectory);
                return new NodeDocumentDirectoryBatch(reader, directory, _settings.Pattern);
            }).AsImplementedInterfaces().AsSelf();

            builder.RegisterType<NodeXmlDocumentReader>()
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterType<LoaderServiceClient>()
                .AsImplementedInterfaces()
                .UseWcfSafeRelease();
        }

    }
}
