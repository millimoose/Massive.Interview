using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using Massive.Interview.LoaderApp.Services;
using Massive.Interview.LoaderApp.Support;

namespace Massive.Interview.LoaderApp.Components
{
    /// <summary>
    /// An implementation of <see cref="INodeDocumentReader"/> that parses a
    /// XML description of a node.
    /// </summary>
    internal class NodeXmlDocumentReader : INodeDocumentReader
    {
        public async Task<NodeInput> ParseNodeInputAsync(Stream inputStream)
        {
            XmlReaderSettings settings = new XmlReaderSettings {
                Async = true,
                IgnoreWhitespace = true
            };

            using (var reader = XmlReader.Create(inputStream, settings))
            {
                var result = new NodeInput();

                reader.ReadStartElement("node");
                while (reader.IsStartElement())
                {
                    switch(reader.Name)
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

        private void ParseAdjacentNodes(XmlReader reader, NodeInput result)
        {
            var empty = reader.IsEmptyElement;
            reader.ReadStartElement("adjacentNodes");
            if (empty) return;

            while (reader.IsStartElement("id"))
            {
                result.AdjacentNodeIds.Add(reader.ReadElementContentAsLong());
            }
            reader.ReadEndElement();

        }
    }
}
