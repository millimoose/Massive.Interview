using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Massive.Interview.LoaderApp.Support;

namespace Massive.Interview.LoaderApp.Services
{
    /// <summary>
    /// Executes a set of synchronization operations.
    /// </summary>
    interface INodeSynchronizer
    {
        /// <summary>
        /// Create a set of operations that will synchronize the current 
        /// database to the loaded input.
        NodeSynchronizationTodo NewTodo(IEnumerable<NodeInput> inputs);

        /// <summary>
        /// Synchronize the database according to the given operations
        /// </summary>
        Task SynchronizeAsync(NodeSynchronizationTodo todo);


    }

}
