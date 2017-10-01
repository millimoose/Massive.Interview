using Autofac;
using Massive.Interview.Entities;
using Massive.Interview.Entities.Module;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Massive.Interview.SmokeTestApp
{
    /// <summary>
    /// Basic sanity test / scratchpad for the model
    /// </summary>
    class Program
    {
        public static void Main(string[] args)
        {
            SmokeTestSettings settings = NewSettings(args);

            var builder = new ContainerBuilder();
            builder.RegisterModule(new GraphEntitiesModule(settings.Entities));
            builder.RegisterType<Program>().AsSelf();
            using (var container = builder.Build())
            {
                WithScopedProgram(container, _ => _.CreateSomeNodes());
            }
        }

        /// <summary>
        /// Load the settings for this program
        /// </summary>
        private static SmokeTestSettings NewSettings(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("SmokeTestApp.json", true)
                .AddCommandLine(args)
                .Build();

            var settings = new SmokeTestSettings();
            config.Bind(settings);
            return settings;
        }

        /// <summary>
        /// Run a method on this class in a new lifetime scope
        /// </summary>
        private static void WithScopedProgram(IContainer container, Action<Program> action)
        {
            using (var scope = container.BeginLifetimeScope(nameof(DbContext)))
            {
                var program = scope.Resolve<Program>();
                action(program);
            }
        }

        private GraphEntities Context { get; }
        private Program(GraphEntities context)
        {
            Context = context;
        }

        private void CreateSomeNodes()
        {
            Context.Database.EnsureCreated();
            var left = new Node
            {
                NodeId = 1,
                Label = "Node1"
            };
            Context.Nodes.Add(left);

            var right = new Node
            {
                NodeId = 2,
                Label = "Node2"
            };

            var link = new AdjacentNode
            {
                LeftNode = left,
                RightNode = right
            };
            Context.Add(link);

            Context.SaveChanges();
        }

    }
}
