﻿using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Massive.Interview.LoaderApp.Remote;
using Massive.Interview.LoaderApp.Support;
using Massive.Interview.Service.Contract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Massive.Interview.LoaderApp.Tests
{
    [TestClass]
    public class NodeDocumentBatchTest
    {
        class MockNodeDocumentReader : INodeDocumentReader
        {
            [SuppressMessage("", "CS1998")]
            public async Task<NodeInputData> ParseNodeInputAsync(Stream inputStream)
            {
                return new NodeInputData();
            }
        }

        [TestMethod]
        public async Task Load_XML_batch()
        {
            var testDataDirectory = 
                new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "TestData"));
            var batch = new NodeDocumentDirectoryBatch(new MockNodeDocumentReader(), testDataDirectory, "*.xml");
            NodeInputData[] nodes = await batch.LoadDocumentsAsync().ConfigureAwait(false);

            Assert.AreEqual(2, nodes.Length);
            Assert.IsTrue(nodes.All(_ => !string.IsNullOrEmpty(_.Source)));
            Assert.IsTrue(nodes.All(_ => _.Source.EndsWith(".xml")));
            Assert.IsFalse(nodes.Any(_ => _.Source.EndsWith(".txt")));
        }
    }

}