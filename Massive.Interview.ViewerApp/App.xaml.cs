using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using Caliburn.Micro.Autofac;

namespace Massive.Interview.ViewerApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() => InitializeComponent();
    }

    public class AppBootstrapper : AutofacBootstrapper<ShellViewModel>
    {
        public AppBootstrapper() => Initialize();
        protected override void OnStartup(object sender, StartupEventArgs e) => DisplayRootViewFor<ShellViewModel>();
        protected override void ConfigureContainer(ContainerBuilder builder) {
            builder.RegisterModule<ViewerModule>();
        }

    }
}
