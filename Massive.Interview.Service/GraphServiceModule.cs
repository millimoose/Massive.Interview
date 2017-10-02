using Autofac;
using Massive.Interview.Interview.Service.Support;

namespace Massive.Interview.Service
{
    class GraphServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LoaderService>().AsSelf();
            builder.RegisterType<GraphService>().AsSelf();
            builder.RegisterType<NodeSynchronizer>().AsImplementedInterfaces();
        }
    }
}