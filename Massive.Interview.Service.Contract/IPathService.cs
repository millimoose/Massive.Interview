using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Massive.Interview.Service
{
    /// <summary>
    /// A service to retrieve the shortest path between two nodes in a graph.
    /// </summary>
    [ServiceContract]
    interface IPathService
    {
        /// <summary>
        /// Return the shortest path between two nodes in the graph.
        /// </summary>
        /// <param name="fromId">ID of the start of the path</param>
        /// <param name="toId">ID of the end of the path</param>
        /// <returns>IDs of the nodes along the path</returns>
        [OperationContract]
        [FaultContract(typeof(NodeNotFoundFault))]
        [FaultContract(typeof(PathNotFoundFault))]
        IEnumerable<AdjacentNodeData> GetShortestPath(long fromId, long toId);
    }

    [DataContract]
    class NodeNotFoundFault
    {
        /// <summary>
        /// The ID of the node that could not be found.
        /// </summary>
        [DataMember]
        public long NodeId { get; set; }

        public NodeNotFoundFault(long nodeId)
        {
            NodeId = nodeId;
        }
    }

    [DataContract]
    class PathNotFoundFault
    {

    }
}
