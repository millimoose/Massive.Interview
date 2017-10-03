using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Massive.Interview.ViewerApp.Remote;

namespace Massive.Interview.ViewerApp
{
    class ViewerModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GraphServiceClient>().AsImplementedInterfaces();

            builder.RegisterType<App>();
            builder.RegisterType<ShellViewModel>();
        }
    }
}
