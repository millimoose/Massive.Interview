using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Autofac;
using Massive.Interview.Entities;
using Massive.Interview.LoaderApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

[assembly: InternalsVisibleTo("Massive.Interview.LoaderApp.Tests")]

namespace Massive.Interview.LoaderApp
{
    class Program
    {
        readonly INodeDocumentBatch _batch;
        readonly INodeSynchronizer _synchronizer;
        readonly GraphEntities _db;

        public Program(INodeDocumentBatch batch, INodeSynchronizer synchronizer, GraphEntities db)
        {
            _batch = batch ?? throw new ArgumentNullException(nameof(batch));
            _synchronizer = synchronizer ?? throw new ArgumentNullException(nameof(synchronizer));
            _db = db;
        }

        public static void Main(string[] args)
        {
            var settings = NewSettings(args);

            var builder = new ContainerBuilder();
            builder.RegisterModule(new LoaderModule(settings));
            builder.RegisterType<Program>().AsSelf();

            using (var container = builder.Build())
            using (var scope = container.BeginLifetimeScope(nameof(DbContext)))
            {
                scope.Resolve<Program>().SynchronizeAsync().GetAwaiter().GetResult();
            }
        }

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
        
        private async Task SynchronizeAsync()
        {
            await _db.Database.EnsureCreatedAsync().ConfigureAwait(false);
            var inputs = await _batch.LoadDocumentsAsync().ConfigureAwait(false);
            var todo = _synchronizer.NewTodo(inputs);
            await _synchronizer.SynchronizeAsync(todo).ConfigureAwait(false);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
