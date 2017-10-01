using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;
using Massive.Interview.Entities.Module;
using Massive.Interview.Entities;

namespace Massive.Interview.SmokeApp
{
    /// <summary>
    /// Basic sanity test / scratchpad for the model
    /// </summary>
    class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new GraphEntitiesModule());
            builder.RegisterType<Program>().AsSelf();
            using (var container = builder.Build())
            using (var scope = container.BeginLifetimeScope())
            {
                var program = scope.Resolve<Program>();
                program.CreateSomeNodesAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }

        private GraphEntities Context { get; }
        public Program(GraphEntities context)
        {
            Context = context;
        }

        private async Task CreateSomeNodesAsync()
        {
            var left = Context.Nodes.Create();
            left.NodeId = 1;
            left.Label = "Node1";
            left = Context.Nodes.Add(left);

            var right = Context.Nodes.Create();
            right.NodeId = 2;
            right.Label = "Node2";
            right = Context.Nodes.Add(right);

            left.RightAdjacentNodes.Add(right);
            right.LeftAdjacentNodes.Add(left);

            await Context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
