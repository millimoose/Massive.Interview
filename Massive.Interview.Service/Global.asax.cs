using System;
using System.Configuration;
using Autofac;
using Autofac.Integration.Wcf;
using Massive.Interview.Entities;
using Massive.Interview.Entities.Autofac;

namespace Massive.Interview.Service
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            var builder = new ContainerBuilder();
            var connectionString = ConfigurationManager.ConnectionStrings["Massive.Interview.Entities"];
            var settings = new GraphEntitiesSettings {
                ConnectionString = connectionString.ConnectionString
            };
            builder.RegisterModule(new GraphEntitiesModule(settings));
            builder.RegisterModule<GraphServiceModule>();
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                scope.Resolve<GraphEntities>().Database.EnsureCreated();
            }
            AutofacHostFactory.Container = container;
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}