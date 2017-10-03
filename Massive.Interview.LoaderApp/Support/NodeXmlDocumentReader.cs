using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Massive.Interview.Service;

namespace Massive.Interview.LoaderApp.Support
{
    /// <summary>
    /// An implementation of <see cref="INodeDocumentReader"/> that parses a
    /// XML description of a node.
    /// </summary>
    internal class NodeXmlDocumentReader : INodeDocumentReader
    {
        public async Task<NodeInputData> ParseNodeInputAsync(Stream inputStream)
        {
            var settings = new XmlReaderSettings {
                Async = true,
                IgnoreWhitespace = true
            };

            using (var reader = XmlReader.Create(inputStream, settings))
            {
                var result = new NodeInputData();

                reader.ReadStartElement("node");
                while (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "id":
                            result.Id = reader.ReadElementContentAsLong();
                            break;
                        case "label":
                            result.Label = await reader.ReadElementContentAsStringAsync().ConfigureAwait(false);
                            break;
                        case "adjacentNodes":
                            ParseAdjacentNodes(reader, result);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("reader.Name", reader.Name, "Unexpected element name");

                    }
                }
                reader.ReadEndElement();
                return result;
            }
        }

        private void ParseAdjacentNodes(XmlReader reader, NodeInputData result)
        {
            var empty = reader.IsEmptyElement;
            reader.ReadStartElement("adjacentNodes");

            if (empty)
            {
                return;
            }

            result.AdjacentNodeIds = ReadIdList(reader).ToList();
            reader.ReadEndElement();

        }

        private IEnumerable<long> ReadIdList(XmlReader reader)
        {
            while (reader.IsStartElement("id"))
            {
                yield return reader.ReadElementContentAsLong();
            }
        }
    }
}
