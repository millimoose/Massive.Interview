using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Massive.Interview.Service.Contract
{
    /// <summary>
    /// A service for loading the graph in a front-end.
    /// </summary>
    [ServiceContract]
    internal interface IGraphService
    {
        /// <summary>
        /// Gets the entire graph.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Task<GraphData> GetGraphAsync();
    }

    /// <summary>
    /// An undirected graph.
    /// </summary>
    [DataContract]
    public class GraphData
    {
        /// <summary>
        /// All the nodes in the graph.
        /// </summary>
        [DataMember]
        public IEnumerable<NodeData> Nodes { get; set; } = Enumerable.Empty<NodeData>();

        /// <summary>
        /// All the edges between the nodes in the graph.
        /// </summary>
        [DataMember]
        public IEnumerable<AdjacentNodeData> AdjacentNodes { get; set; } = Enumerable.Empty<AdjacentNodeData>();
    }

    /// <summary>
    /// A node in the graph.
    /// </summary>
    [DataContract]
    public class NodeData
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string Label { get; set; }
    }

    /// <summary>
    /// An edge between two nodes.
    /// </summary>
    [DataContract]
    public class AdjacentNodeData
    {
        /// <summary>
        /// The "left" side of the edge - with the node with the lesser ID.
        /// </summary>
        [DataMember]
        public long LeftId { get; set; }

        /// <summary>
        /// The "left" side of the edge - with the node with the greater ID.
        /// </summary>
        [DataMember]
        public long RightId { get; set; }
    }
}
