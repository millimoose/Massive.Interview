using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using Massive.Interview.Entities;
using Massive.Interview.Service.Support;
using Microsoft.EntityFrameworkCore;

namespace Massive.Interview.Service
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    class LoaderService : ILoaderService
    {
        readonly GraphEntities _db;
        readonly INodeSynchronizer _synchronizer;

        public LoaderService(GraphEntities db, INodeSynchronizer synchronizer)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _synchronizer = synchronizer;
        }

        public async Task<IEnumerable<long>> GetExistingNodeIdsAsync()
        {
            var nodesQuery = from dbNode in _db.Nodes.AsNoTracking() select dbNode.NodeId.Value;
            return await nodesQuery.ToListAsync().ConfigureAwait(false);
        }

        public async Task LoadNodesAsync(IEnumerable<NodeInputData> nodeinputs)
        {
            var todo = _synchronizer.NewTodo(nodeinputs);
            await _synchronizer.SynchronizeAsync(todo).ConfigureAwait(false);
            await _db.SaveChangesAsync().ConfigureAwait(false);

        }
    }
}
