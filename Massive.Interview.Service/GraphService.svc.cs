using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Massive.Interview.Entities;
using Massive.Interview.Service.Contract;
using Microsoft.EntityFrameworkCore;

namespace Massive.Interview.Service
{
    class GraphService : IGraphService
    {
        GraphEntities _db;

        public GraphService(GraphEntities db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<GraphData> GetGraphAsync()
        {
            var nodes = (from dbNode in _db.Nodes
                         select new NodeData
                         {
                             Id = dbNode.NodeId.Value,
                             Label = dbNode.Label
                         }).ToListAsync();

            var adjacents = (from dbAdjacent in _db.AdjacentNodes
                             select new AdjacentNodeData
                             {
                                 LeftId = dbAdjacent.LeftNodeId.Value,
                                 RightId = dbAdjacent.RightNodeId.Value
                             }).ToListAsync();

            await Task.WhenAll(nodes, adjacents).ConfigureAwait(false);

            return new GraphData
            {
                Nodes = nodes.Result,
                AdjacentNodes = adjacents.Result
            };
        }
    }
}
