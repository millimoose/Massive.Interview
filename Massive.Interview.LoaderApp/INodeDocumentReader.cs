using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Massive.Interview.LoaderApp
{

    /// <summary>
    /// Converts a document describing a node into an instance of <see cref="NodeInput"/>.
    /// </summary>
    interface INodeDocumentReader
    {
        Task<NodeInput> ParseNodeInputAsync(Stream inputStream);
    }
}
