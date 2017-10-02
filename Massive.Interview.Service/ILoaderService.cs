﻿using System;
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
    }
}
