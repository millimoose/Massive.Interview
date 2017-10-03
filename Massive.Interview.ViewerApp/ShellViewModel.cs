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

            var aglGraph = new Graph();
            var aglNodes = from nodeData in graphData.Nodes
                           let idString = Convert.ToString(nodeData.Id, CultureInfo.InvariantCulture)
                           select new Node(idString) { LabelText = nodeData.Label };

            foreach (var aglNode in aglNodes) {
                aglGraph.AddNode(aglNode);
            }

            var aglEdges = from adjacentData in graphData.AdjacentNodes
                           let left = Convert.ToString(adjacentData.LeftId, CultureInfo.InvariantCulture)
                           let right = Convert.ToString(adjacentData.RightId, CultureInfo.InvariantCulture)
                           select (left, right);

            foreach (var (left, right) in aglEdges)
            {
                aglGraph.AddEdge(left, right);
            }

            AglGraph = aglGraph;
        }

        public Graph AglGraph { get; set; }
    }

    
}
