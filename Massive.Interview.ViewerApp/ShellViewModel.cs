using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Massive.Interview.ViewerApp.Remote;
using Microsoft.Msagl.Drawing;

namespace Massive.Interview.ViewerApp
{
    public class ShellViewModel : PropertyChangedBase
    {
        string _hello = "Hello";

        public string Hello {
            get => _hello;
            set {
                _hello = value;
                NotifyOfPropertyChange(() => Hello);
            }
        }

        IGraphService _graphService;

        public ShellViewModel(IGraphService graphService)
        {
            _graphService = graphService ?? throw new ArgumentNullException(nameof(graphService));

            var graphData = _graphService.GetGraph();

            var aglGraph = new Graph() {
                Directed = false
            };
            // add edges
            var aglEdges = from adjacentData in graphData.AdjacentNodes
                           let left = Convert.ToString(adjacentData.LeftId, CultureInfo.InvariantCulture)
                           let right = Convert.ToString(adjacentData.RightId, CultureInfo.InvariantCulture)
                           select (left, right);

            foreach (var (left, right) in aglEdges)
            {
                var edge = aglGraph.AddEdge(left, right);
                edge.Attr.ArrowheadAtSource = edge.Attr.ArrowheadAtTarget = ArrowStyle.None;
            }

            // set node labels
            var aglNodes = from nodeData in graphData.Nodes
                           let id = Convert.ToString(nodeData.Id, CultureInfo.InvariantCulture)
                           select (id, label: nodeData.Label);

            foreach (var (id, label) in aglNodes)
            {
                var node = aglGraph.FindNode(id) ?? aglGraph.AddNode(id);
                node.LabelText = label;
            }
            AglGraph = aglGraph;
        }

        public Graph AglGraph { get; set; }
    }

    
}
