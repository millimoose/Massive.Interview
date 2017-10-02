using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Massive.Interview.Service;

namespace Massive.Interview.Interview.Service.Support
{
    /// <summary>
    /// The set of operations that will synchronize the database with loaded input data.
    /// </summary>
    class NodeSynchronizationTodo
    {
        /// <summary>
        /// New nodes that will be added to the database.
        /// </summary>
        public ReadOnlyCollection<NodeInputData> NodesToAdd { get; }
        /// <summary>
        /// Nodes in the database that need to be updated.
        /// </summary>
        public ReadOnlyCollection<NodeInputData> NodesToUpdate { get; }

        /// <summary>
        /// Nodes that will be removed from the database.
        /// </summary>
        public ReadOnlyCollection<long> NodeIdsToRemove { get; }

        /// <summary>
        /// Create a set of synmchronisation operations based on node IDs 
        /// currently in the database, and loaded nodes.
        /// </summary>
        /// <param name="oldNodeIds">previous node IDs</param>
        /// <param name="newNodes">new nodes</param>
        public NodeSynchronizationTodo(IEnumerable<long> oldNodeIds, IEnumerable<NodeInputData> newNodes)
        {
            var toAdd = new List<NodeInputData>();
            var toUpdate = new List<NodeInputData>();

            var oldSet = new HashSet<long>(oldNodeIds);
            foreach (var node in newNodes)
            {
                if (oldSet.Remove(node.Id))
                {
                    // 
                    toUpdate.Add(node);
                }
                else
                {
                    toAdd.Add(node);
                }
            }

            NodesToAdd = toAdd.AsReadOnly();
            NodesToUpdate = toUpdate.AsReadOnly();
            NodeIdsToRemove = oldSet.ToList().AsReadOnly();
        }
    }

}
