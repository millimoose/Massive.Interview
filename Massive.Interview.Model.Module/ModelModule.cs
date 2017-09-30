using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Massive.Interview.Model.Module
{
    public class ModelModule : Autofac.Module
    {
        public string ConnectionString { get; set; }

        private GraphModel NewModel()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                return new GraphModel();
            } else
            {
                return new GraphModel(ConnectionString);
            }
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(ctx => NewModel()).AsSelf();
        }
    }
