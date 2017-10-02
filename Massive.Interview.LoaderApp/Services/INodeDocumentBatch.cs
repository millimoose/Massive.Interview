using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Massive.Interview.LoaderApp.Support;

namespace Massive.Interview.LoaderApp.Services
{
    /// <summary>
    /// A batch of multiple node documents being loaded.
    /// </summary>
    interface INodeDocumentBatch
    {
        /// <summary>
        /// Load a batch of documents into <see cref="NodeInput"/> instances.
        /// </summary>
        /// 
        Task<NodeInput[]> LoadDocumentsAsync();
    }
}
