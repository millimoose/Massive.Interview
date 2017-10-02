using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Autofac;
using Massive.Interview.Entities.Module;
using Massive.Interview.LoaderApp.Components;
using Massive.Interview.LoaderApp.Services;
using Microsoft.Extensions.Configuration;

[assembly: InternalsVisibleTo("Massive.Interview.LoaderApp.Tests")]

namespace Massive.Interview.LoaderApp
{
    class LoaderSettings
    {
        public string InputDirectory { get; set; }
        public string Pattern { get; set; } = "*.xml";
        public GraphEntitiesSettings Entities { get; set; }
    }
    class Program
    {

        static LoaderSettings NewSettings(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("LoaderApp.json", true)
                .AddCommandLine(args)
                .Build();

            LoaderSettings result = new LoaderSettings();
            config.Bind(result);
            return result;
        }
        public static void Main(string[] args)
        {
            var settings = NewSettings(args);
            Console.WriteLine(new DirectoryInfo(settings.InputDirectory));
        }
    }

    class LoaderModule : Autofac.Module
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
