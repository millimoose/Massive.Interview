using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Massive.Interview.Model.Module
{
    public class GraphEntitiesModule : Autofac.Module
    {
        public string ConnectionString { get; set; }

        private GraphEntities NewDbContext()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                return new GraphEntities();
            }
            else
            {
                return new GraphEntities(ConnectionString);
            }
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(ctx => NewDbContext()).AsSelf();
        }
    }
}