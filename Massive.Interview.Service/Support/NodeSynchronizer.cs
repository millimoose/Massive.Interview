﻿using System;
using System.Linq;
using System.Collections.Generic;
using Massive.Interview.Entities;
using Z.EntityFramework.Plus;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Massive.Interview.Service.Support
{
    class NodeSynchronizer : INodeSynchronizer
    {
        readonly GraphEntities _db;

        public NodeSynchronizer(GraphEntities db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public NodeSynchronizationTodo NewTodo(IEnumerable<NodeInputData> newNodes)
        {
            var oldNodeIds = (from dbNode in _db.Nodes.AsNoTracking() select dbNode.NodeId.Value);
            return new NodeSynchronizationTodo(oldNodeIds, newNodes);
        }

        public async Task SynchronizeAsync(NodeSynchronizationTodo todo)
        {
            // break adjacency for all nodes to be removed or updated
            var adjacentIdsToRemove = todo.NodesToUpdate.Select(_ => _.Id).Concat(todo.NodeIdsToRemove);
            var adjacentsToRemove = from dbAdjacent in _db.AdjacentNodes
                                    where adjacentIdsToRemove.Contains(dbAdjacent.LeftNodeId.Value)
                                       || adjacentIdsToRemove.Contains(dbAdjacent.RightNodeId.Value)
                                    select dbAdjacent;
            await adjacentsToRemove.DeleteAsync().ConfigureAwait(false);

            // delete nodes
            var nodesToRemove = from dbNode in _db.Nodes
                                where todo.NodeIdsToRemove.Contains(dbNode.NodeId.Value)
                                select dbNode;
            await nodesToRemove.DeleteAsync().ConfigureAwait(false);

            await _db.Nodes.AddRangeAsync(NewNodesFromInputs(todo.NodesToAdd)).ConfigureAwait(false);
            _db.Nodes.UpdateRange(NewNodesFromInputs(todo.NodesToUpdate));

            // create new adjacencies
            var adjacentsToAdd = from input in todo.NodesToAdd.Concat(todo.NodesToUpdate)
                                 from adjacent in input.AdjacentNodeIds
                                 select (leftId: input.Id, rightId: adjacent);

            foreach (var (leftId, rightId) in adjacentsToAdd)
            {
                await _db.AdjacentNodes.MakeAdjacentAsync(leftId, rightId).ConfigureAwait(false);
            }
        }


        /// <summary>
        /// Convert <see cref="NodeInputData"/>s to a <see cref="Node"/>s.
        /// </summary>
        private static IEnumerable<Node> NewNodesFromInputs(IEnumerable<NodeInputData> inputs) =>
             from input in inputs
             select new Node {
                 NodeId = input.Id,
                 Label = input.Label
             };
    }

}
