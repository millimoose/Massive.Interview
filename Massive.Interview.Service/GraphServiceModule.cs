using Autofac;

namespace Massive.Interview.Service
{
    class GraphServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LoaderService>().AsSelf();
        }
    }
}