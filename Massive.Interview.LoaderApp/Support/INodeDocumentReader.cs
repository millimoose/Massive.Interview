using System.IO;
using System.Threading.Tasks;
using Massive.Interview.Service.Contract;

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
