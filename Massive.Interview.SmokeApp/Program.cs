using Autofac;
using Massive.Interview.Entities;
using Massive.Interview.Entities.Module;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

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
            {
                WithScopedProgram(container, _ => _.CreateSomeNodes());
                WithScopedProgram(container, _ => _.TestLeftToRight());
                WithScopedProgram(container, _ => _.TestRightToLeft());
            }
        }

        private void TestRightToLeft()
        {
        }

        private void TestLeftToRight()
        {
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

        /// <summary>
        /// Run an asynchronous method on this class in a new lifetime scope
        /// </summary>
        private static void WithScopedProgram<TTask>(IContainer container, Func<Program, TTask> func) where TTask : Task
        {
            using (var scope = container.BeginLifetimeScope(nameof(DbContext)))
            {
                var program = scope.Resolve<Program>();
                func(program).GetAwaiter().GetResult();
            }
        }

        private GraphEntities Context { get; }
        public Program(GraphEntities context)
        {
            Context = context;
        }

        private void CreateSomeNodes()
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

            Context.SaveChanges();
        }
    
    }
}
