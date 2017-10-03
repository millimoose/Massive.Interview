using Autofac;
using Massive.Interview.Service.Support;

namespace Massive.Interview.Service
{
    class GraphServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LoaderService>().AsSelf();
            builder.RegisterType<GraphService>().AsSelf();
            builder.RegisterType<NodeSynchronizer>().AsImplementedInterfaces();
        }
    }
}