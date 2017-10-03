using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Massive.Interview.LoaderApp.Support;
using Massive.Interview.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Massive.Interview.LoaderApp.Tests
{
    [TestClass]
    public class NodeDocumentBatchTest
    {
        class MockNodeDocumentReader : INodeDocumentReader
        {
            public async Task<NodeInputData> ParseNodeInputAsync(Stream inputStream)
            {
                return await Task.FromResult(new NodeInputData()).ConfigureAwait(false);
            }
        }

        [TestMethod]
        public async Task LoadXmlBatch()
        {
            var testDataDirectory =
                new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "TestData"));
            var batch = new NodeDocumentDirectoryBatch(new MockNodeDocumentReader(), testDataDirectory, "*.xml");
            var nodes = await batch.LoadDocumentsAsync().ConfigureAwait(false);

            Assert.AreEqual(2, nodes.Length);
            Assert.IsTrue(nodes.All(_ => !string.IsNullOrEmpty(_.Source)));
            Assert.IsTrue(nodes.All(_ => _.Source.EndsWith(".xml")));
            Assert.IsFalse(nodes.Any(_ => _.Source.EndsWith(".txt")));
        }
    }

}
