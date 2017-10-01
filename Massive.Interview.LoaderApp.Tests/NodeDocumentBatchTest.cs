using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Massive.Interview.LoaderApp.Tests
{
    [TestClass]
    public class NodeDocumentBatchTest
    {
        class MockNodeDocumentReader : INodeDocumentReader
        {
            [SuppressMessage("", "CS1998")]
            public async Task<NodeInput> ParseNodeInputAsync(Stream inputStream)
            {
                return new NodeInput();
            }
        }

        [TestMethod]
        public async Task Load_XML_batch()
        {
            var testDataDirectory = 
                new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "TestData"));
            var batch = new NodeDocumentDirectoryBatch(testDataDirectory, new MockNodeDocumentReader(), "*.xml");
            NodeInput[] nodes = await batch.LoadDocumentsAsync().ConfigureAwait(false);

            Assert.AreEqual(2, nodes.Length);
            Assert.IsTrue(nodes.All(_ => !string.IsNullOrEmpty(_.Source)));
            Assert.IsTrue(nodes.All(_ => _.Source.EndsWith(".xml")));
            Assert.IsFalse(nodes.Any(_ => _.Source.EndsWith(".txt")));
        }
    }

}
