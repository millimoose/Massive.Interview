using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Massive.Interview.Entities;
using Massive.Interview.Interview.Service.Support;
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

        public async Task<long[]> GetExistingNodeIdsAsync()
        {
            return await (from dbNode in _db.Nodes select dbNode.NodeId.Value).ToArrayAsync();
        }

        public async Task LoadNodesAsync(IEnumerable<NodeInputData> nodeinputs)
        {
            var todo = _synchronizer.NewTodo(nodeinputs);
            await _synchronizer.SynchronizeAsync(todo).ConfigureAwait(false);
            await _db.SaveChangesAsync().ConfigureAwait(false);

        }
    }
}
