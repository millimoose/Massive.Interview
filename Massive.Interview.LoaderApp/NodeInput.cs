using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Massive.Interview.Support;

namespace Massive.Interview.LoaderApp
{
    /// <summary>
    /// A data class representing a parsed input document describing a node.
    /// </summary>
    class NodeInput
    {
        /// <summary>
        /// The node ID.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The node label.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The IDs of the adjacent nodes.
        /// </summary>
        public ICollection<long> AdjacentNodeIds { get; } = new List<long>();

        public override string ToString()
        {
            return base.ToString() + new
            {
                Id,
                Label,
                AdjacentNodes = AdjacentNodeIds.ToShortString()
            };
        }
    }
}
