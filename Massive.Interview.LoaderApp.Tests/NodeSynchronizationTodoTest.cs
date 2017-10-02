using System;
using System.Collections.Generic;
using System.Linq;
using Massive.Interview.LoaderApp.Support;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Massive.Interview.LoaderApp.Tests
{
    [TestClass]
    public class NodeSynchronizationTodoTest
    {
        [TestMethod]
        public void Create_todo()
        {
            var previousIds = new long[] { 1, 2, 3, 4 };
            var newIds = new long[] { 3, 4, 5, 6 };
            var newNodes = from id in newIds select new NodeInput { Id = id };
        
            var todo = new NodeSynchronizationTodo(previousIds, newNodes);

            CollectionAssert.AreEquivalent(
                new long[] { 1, 2 },
                todo.NodeIdsToRemove.ToArray());

            CollectionAssert.AreEquivalent(
                new long[] { 3, 4 },
                (from node in todo.NodesToUpdate select node.Id).ToArray());

            CollectionAssert.AreEquivalent(
                new long[] { 5, 6 },
                (from node in todo.NodesToAdd select node.Id).ToArray());
        }
    }
}
