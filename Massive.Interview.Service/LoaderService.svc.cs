using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Massive.Interview.Entities;
using Microsoft.EntityFrameworkCore;

namespace Massive.Interview.Service
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class LoaderService : ILoaderService
    {
        GraphEntities _db;

        public LoaderService(GraphEntities db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<long[]> GetExistingNodeIdsAsync()
        {
            return await (from dbNode in _db.Nodes select dbNode.NodeId.Value).ToArrayAsync();
        }
    }
}
