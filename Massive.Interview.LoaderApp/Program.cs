using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Autofac;
using Massive.Interview.LoaderApp.Remote;
using Massive.Interview.LoaderApp.Support;
using Microsoft.Extensions.Configuration;

[assembly: InternalsVisibleTo("Massive.Interview.LoaderApp.Tests")]

namespace Massive.Interview.LoaderApp
{
    class Program
    {
        readonly INodeDocumentBatch _batch;
        private readonly ILoaderService _loaderService;

        public Program(INodeDocumentBatch batch, ILoaderService loaderService)
        {
            _batch = batch ?? throw new ArgumentNullException(nameof(batch));
            _loaderService = loaderService;
        }

        public static void Main(string[] args)
        {
            var settings = NewSettings(args);

            var builder = new ContainerBuilder();
            builder.RegisterModule(new LoaderModule(settings));
            builder.RegisterType<Program>().AsSelf();

            using (var container = builder.Build())
            using (var scope = container.BeginLifetimeScope())
            {
                scope.Resolve<Program>().SynchronizeAsync().GetAwaiter().GetResult();
            }
        }

        static LoaderSettings NewSettings(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            LoaderSettings result = new LoaderSettings();
            config.Bind(result);
            return result;
        }
        
        private async Task SynchronizeAsync()
        {
            var inputs = await _batch.LoadDocumentsAsync().ConfigureAwait(false);
            await _loaderService.LoadNodesAsync(inputs).ConfigureAwait(false);

        }
    }
}
