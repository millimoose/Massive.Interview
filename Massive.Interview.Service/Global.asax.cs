using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Autofac;
using Autofac.Integration.Wcf;
using Massive.Interview.Entities.Module;

namespace Massive.Interview.Service
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            var builder = new ContainerBuilder();
            var connectionString = ConfigurationManager.ConnectionStrings["Massive.Interview.Entities"];
            var settings = new GraphEntitiesSettings
            {
                ConnectionString = connectionString.ConnectionString
            };
            builder.RegisterModule(new GraphEntitiesModule(settings));
            builder.RegisterModule<GraphServiceModule>();
            AutofacHostFactory.Container = builder.Build();
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