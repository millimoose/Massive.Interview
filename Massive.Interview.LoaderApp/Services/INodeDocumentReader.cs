using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Massive.Interview.LoaderApp.Support;

namespace Massive.Interview.LoaderApp.Services
{

    /// <summary>
    /// Converts a document describing a node into an instance of <see cref="NodeInput"/>.
    /// </summary>
    interface INodeDocumentReader
    {
        Task<NodeInput> ParseNodeInputAsync(Stream inputStream);
    }
}
