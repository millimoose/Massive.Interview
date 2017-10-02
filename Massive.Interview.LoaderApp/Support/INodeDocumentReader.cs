using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Massive.Interview.LoaderApp.Remote;

namespace Massive.Interview.LoaderApp.Support
{

    /// <summary>
    /// Converts a document describing a node into an instance of <see cref="NodeInput"/>.
    /// </summary>
    interface INodeDocumentReader
    {
        Task<NodeInputData> ParseNodeInputAsync(Stream inputStream);
    }
}
