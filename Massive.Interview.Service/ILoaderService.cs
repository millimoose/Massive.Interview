using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Massive.Interview.Service
{
    [ServiceContract]
    public interface ILoaderService
    {
        [OperationContract]
        Task<long[]> GetExistingNodeIdsAsync();

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

        public ICollection<long> AdjacentNodeIds { get; set; } = new List<long>();
    }
}
