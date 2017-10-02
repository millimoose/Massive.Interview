using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Massive.Interview.Entities
{
    /// <summary>
    /// Join entity representing vertices between nodes.
    /// </summary>
    public class AdjacentNode : IFormattable
    {
        /// <summary>
        /// ID of the node on the left side of the vertex. (The node with the lesser ID.)
        /// </summary>
        public long? LeftNodeId { get; set; }

        /// <summary>
        /// The node on the left side of the vertex. (The node with the lesser ID.)
        /// </summary>
        public Node LeftNode { get; set; }

        /// <summary>
        /// ID of the node on the right side of the vertex. (The node with the greater ID.)
        /// </summary>
        public long? RightNodeId { get; set; }

        /// <summary>
        /// The node on the right side of the vertex. (The node with the greater ID.)
        /// </summary>
        public Node RightNode { get; set; }

        public override string ToString()
        {
            return ToString("G");
        }

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }
        public string ToString(string format, IFormatProvider formatProvider)
        {
            format = format ?? "G";
            formatProvider = formatProvider ?? CultureInfo.CurrentCulture;

            switch (format)
            {
                // short format
                case "g":
                    return base.ToString() + new { LeftNodeId, RightNodeId };
                case "G":
                default:
                    return base.ToString() + new
                    {
                        LeftNode = LeftNode.ToString("g"),
                        RightNode = RightNode.ToString("g")
                    };
            }
        }
    }

    public static class AdjacentNodeExtensions
    {
        /// <summary>
        /// Correctly set up an adjacency in the graph.
        /// </summary>
        /// Ensures sure that the node to the left of the adjacency has a 
        /// lesser ID than the node to the right.
        public static async Task MakeAdjacentAsync(this DbSet<AdjacentNode> dbNodes, long leftId, long rightId)
        {
            // swap nodes if neccessary
            if (leftId > rightId)
            {
                (leftId, rightId) = (rightId, leftId);
            }

            var exists = await
                (from n in dbNodes
                 where n.LeftNodeId == leftId && n.RightNodeId == rightId
                 select n).AnyAsync().ConfigureAwait(false);

            if (!exists)
            {
                await dbNodes.AddAsync(new AdjacentNode { LeftNodeId = leftId, RightNodeId = rightId }).ConfigureAwait(false);
            }
            return;
        }
    }
}