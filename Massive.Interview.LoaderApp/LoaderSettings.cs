using System.Runtime.CompilerServices;
using Massive.Interview.Entities.Module;

[assembly: InternalsVisibleTo("Massive.Interview.LoaderApp.Tests")]

namespace Massive.Interview.LoaderApp
{
    class LoaderSettings
    {
        public string InputDirectory { get; set; }
        public string Pattern { get; set; } = "*.xml";
        public GraphEntitiesSettings Entities { get; set; }
    }
}
