using Autofac;
using Massive.Interview.Entities;
using Massive.Interview.Entities.Module;
using System;
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
            var settings = NewSettings(args);

            var builder = new ContainerBuilder();
            builder.RegisterModule(new GraphEntitiesModule(settings.Entities));
            builder.RegisterType<Program>().AsSelf();
            using (var container = builder.Build())
            {
                WithScopedProgram(container, _ => _.CreateSomeNodes());
                WithScopedProgram(container, _ => _.RemoveLink());
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

        readonly GraphEntities _context;
        public Program(GraphEntities context)
        {
            _context = context;
        }

        private void CreateSomeNodes()
        {
            _context.Database.EnsureCreated();
            _context.RemoveRange(_context.AdjacentNodes);
            _context.RemoveRange(_context.Nodes);
            _context.SaveChanges();
            var left = new Node {
                NodeId = 1,
                Label = "Node1"
            };
            _context.Nodes.Add(left);

            var right = new Node {
                NodeId = 2,
                Label = "Node2"
            };

            var link = new AdjacentNode {
                LeftNode = left,
                RightNode = right
            };
            _context.Add(link);

            _context.SaveChanges();
        }

        private void RemoveLink()
        {
            var node1 = _context.Nodes.Find(1L);
            _context.RemoveRange(node1.LeftAdjacentNodes);
            _context.RemoveRange(node1.RightAdjacentNodes);
            var node2 = _context.Nodes.Find(2L);
            Console.WriteLine(node1);
            Console.WriteLine(node2);
        }
    }
}
