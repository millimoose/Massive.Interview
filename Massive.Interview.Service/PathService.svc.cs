using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Massive.Interview.Entities;
using QuickGraph;
using QuickGraph.Algorithms;

namespace Massive.Interview.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class PathService : IPathService
    {
        readonly GraphEntities _db;

        public PathService(GraphEntities db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public IEnumerable<AdjacentNodeData> GetShortestPath(long fromId, long toId)
        {
            var graph = new UndirectedGraph<Node, UndirectedEdge<Node>>();

            graph.AddVertexRange(_db.Nodes);
            graph.AddEdgeRange(from adjacent in _db.AdjacentNodes select new UndirectedEdge<Node>(adjacent.LeftNode, adjacent.RightNode));

            var fromNode = _db.Nodes.Find(fromId) ?? throw new FaultException<NodeNotFoundFault>(new NodeNotFoundFault(fromId));
            var toNode = _db.Nodes.Find(toId) ?? throw new FaultException<NodeNotFoundFault>(new NodeNotFoundFault(fromId));

            var tryFunc = graph.ShortestPathsDijkstra(_ => 0, fromNode);
            if (tryFunc(toNode, out var path))
            {
                return from step in path
                       select new AdjacentNodeData {
                           LeftId = step.Source.NodeId.Value,
                           RightId = step.Source.NodeId.Value
                       };
            }
            else
            {
                throw new FaultException<PathNotFoundFault>(new PathNotFoundFault());
            }
        }


    }
}
