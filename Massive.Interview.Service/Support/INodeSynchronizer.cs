using System.Collections.Generic;
using System.Threading.Tasks;

namespace Massive.Interview.Service.Support
{
    /// <summary>
    /// Executes a set of synchronization operations.
    /// </summary>
    interface INodeSynchronizer
    {
        /// <summary>
        /// Create a set of operations that will synchronize the current 
        /// database to the loaded input.
        NodeSynchronizationTodo NewTodo(IEnumerable<NodeInputData> inputs);

        /// <summary>
        /// Synchronize the database according to the given operations
        /// </summary>
        Task SynchronizeAsync(NodeSynchronizationTodo todo);


    }

}
