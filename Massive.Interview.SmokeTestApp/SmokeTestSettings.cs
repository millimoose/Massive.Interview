using System.Diagnostics.CodeAnalysis;
using Massive.Interview.Entities.Autofac;

namespace Massive.Interview.SmokeTestApp
{
    
    class SmokeTestSettings
    {
        public GraphEntitiesSettings Entities {
            get;

            [SuppressMessage("Microsoft.Performance", "CA1811")]
            set;
        }
    }
}
