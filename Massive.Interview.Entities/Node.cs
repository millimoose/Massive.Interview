using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Massive.Interview.Support;

namespace Massive.Interview.Entities
{
    /// <summary>
    /// A node of an undirected graph.
    /// </summary>
    public class Node : IFormattable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long? NodeId { get; set; }

        /// <summary>
        /// A text label for the node.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The nodes adjacent to this node to its "left" - with a lesser id.
        /// </summary>
        public ICollection<AdjacentNode> LeftAdjacentNodes { get; } = new List<AdjacentNode>(0);

        /// <summary>
        /// The nodes adjacent to this node to its "right" - with a greater id. 
        /// </summary>
        public ICollection<AdjacentNode> RightAdjacentNodes { get; } = new List<AdjacentNode>(0);

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
                    return base.ToString() + new { NodeId, Label };
                case "G":
                default:
                    return base.ToString() + new {
                        NodeId,
                        Label,
                        LeftAdjacentNodes = LeftAdjacentNodes.ToShortString(),
                        RightAdjacentNodes = RightAdjacentNodes.ToShortString()
                    };
            }
        }
    }
}