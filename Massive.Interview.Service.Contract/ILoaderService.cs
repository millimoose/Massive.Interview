using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Massive.Interview.Service
{
    [ServiceContract]
    internal interface ILoaderService
    {
        [OperationContract]
        Task<IEnumerable<long>> GetExistingNodeIdsAsync();

        [OperationContract]
        Task LoadNodesAsync(IEnumerable<NodeInputData> nodeinputs);
    }

    /// <summary>
    /// A DTO describing a node submitted to the loader.
    /// </summary>
    [DataContract]
    public class NodeInputData
    {
        /// <summary>
        /// The node ID.
        /// </summary>
        [DataMember]
        public long Id { get; set; }

        /// <summary>
        /// The node label.
        /// </summary>
        [DataMember]

        public string Label { get; set; }

        /// <summary>
        /// The IDs of the adjacent nodes.
        /// </summary>
        [DataMember]
        public IEnumerable<long> AdjacentNodeIds { get; set; } = Enumerable.Empty<long>();

        /// <summary>
        /// A description of the source where the node input was read from.
        /// </summary>
        [DataMember]
        public string Source { get; set; }
    }
}
