using System;
using System.IO;
using System.Runtime.CompilerServices;
using Massive.Interview.Entities.Module;
using Microsoft.Extensions.Configuration;

[assembly: InternalsVisibleTo("Massive.Interview.LoaderApp.Tests")]

namespace Massive.Interview.LoaderApp
{
    class LoaderSettings
    {
        public string InputDirectory { get; set; }
        public GraphEntitiesSettings Entities { get; set; }
    }
    class Program
    {
        static LoaderSettings NewSettings(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("LoaderApp.json", true)
                .AddCommandLine(args    )
                .Build();

            LoaderSettings result = new LoaderSettings();
            config.Bind(result);
            return result;
        }
        public static void Main(string[] args)
        {
            var settings = NewSettings(args);
            Console.WriteLine(new DirectoryInfo(settings.InputDirectory));
        }
    }
}
