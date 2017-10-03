using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Xml;
using Massive.Interview.LoaderApp.Support;

namespace Massive.Interview.LoaderApp.Tests
{
    [TestClass]
    public class NodeDocumentReaderTest
    {
        INodeDocumentReader reader = new NodeXmlDocumentReader();

        private async Task WithStreamAsync(string xmlString, Func<Stream, Task> func)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
            {
                await func(stream);
            }
        }

        static readonly string _validXmlString =
@"<?xml version=""1.0"" encoding=""utf-8""?>
<node>
    <id>1</id>
    <label>Apple</label>
    <adjacentNodes>
        <id>3</id>
        <id>2</id>
    </adjacentNodes>
</node>";

        [TestMethod]
        public async Task Parse_valid_document()
        {
            await WithStreamAsync(_validXmlString, async stream => {
                var node = await reader.ParseNodeInputAsync(stream).ConfigureAwait(false);

                Assert.IsNotNull(node);
                Assert.AreEqual(1L, node.Id);
                Assert.AreEqual("Apple", node.Label);

                CollectionAssert.AreEquivalent(
                    new[] { 3L, 2L },
                    node.AdjacentNodeIds.ToArray());
            }).ConfigureAwait(false);
        }

        static readonly string _invalidRootNode =
@"<?xml version=""1.0"" encoding=""utf-8""?>
<done>
    <id>1</id>
    <label>Apple</label>
    <adjacentNodes>
        <id>3</id>
        <id>2</id>
    </adjacentNodes>
</done>";
        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public async Task Parse_invalid_root_node()
        {
            await WithStreamAsync(_invalidRootNode, async stream => {
                var node = await reader.ParseNodeInputAsync(stream).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        static readonly string _invalidRootDescendant =
@"<?xml version=""1.0"" encoding=""utf-8""?>
<node>
    <id>1</id>
    <label>Apple</label>
    <adjacentNodes>
        <id>3</id>
        <id>2</id>
    </adjacentNodes>
    <foo>bar</foo>
</node>";
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task Parse_invalid_root_descendant()
        {
            await WithStreamAsync(_invalidRootDescendant, async stream => {
                var node = await reader.ParseNodeInputAsync(stream).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        static readonly string _invalidAdjacentNodeId =
@"<?xml version=""1.0"" encoding=""utf-8""?>
<node>
    <id>1</id>
    <label>Apple</label>
    <adjacentNodes>
        <id>3</id>
        <id>2</id>
        <foo>bar</foo>
    </adjacentNodes>
</node>";
        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public async Task Parse_invalid_adjacent_node_id()
        {
            await WithStreamAsync(_invalidAdjacentNodeId, async stream => {
                var node = await reader.ParseNodeInputAsync(stream).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
        static readonly string _emptyAdjacentIds =
@"<?xml version=""1.0"" encoding=""utf-8""?>
<node>
    <id>1</id>
    <label>Apple</label>
    <adjacentNodes/>
</node>";
        [TestMethod]
        public async Task Parse_empty_adjacentIds()
        {
            await WithStreamAsync(_emptyAdjacentIds, async stream => {
                var node = await reader.ParseNodeInputAsync(stream).ConfigureAwait(false);
                CollectionAssert.AreEquivalent(new long[0], node.AdjacentNodeIds.ToArray());
            }).ConfigureAwait(false);
        }
    }
}
