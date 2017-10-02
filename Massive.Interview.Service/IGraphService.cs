using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Massive.Interview.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IGraphService" in both code and config file together.
    [ServiceContract]
    public interface IGraphService
    {
        [OperationContract]
        Task<GraphData> GetGraphAsync();
    }

    [DataContract]
    public class GraphData
    {
        [DataMember]
        public NodeData[] Nodes { get; set; }

        [DataMember]
        public AdjacentNodeData[] AdjacentNodes { get; set; }
    }

    [DataContract]
    public class NodeData
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string Label { get; set; }
    }

    [DataContract]
    public class AdjacentNodeData
    {
        [DataMember]
        public long LeftId { get; set; }
        [DataMember]
        public long RightId { get; set; }
    }
}
