using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Massive.Interview.LoaderApp.Remote;

namespace Massive.Interview.LoaderApp.Support
{
    /// <summary>
    /// A document batch that loads all files in a directory.
    /// </summary>
    class NodeDocumentDirectoryBatch : INodeDocumentBatch
    {
        private DirectoryInfo _directory;
        private INodeDocumentReader _reader;
        private string _searchPattern;

        public NodeDocumentDirectoryBatch(INodeDocumentReader reader, DirectoryInfo directory, string searchPattern)
        {
            _directory = directory ?? throw new ArgumentNullException(nameof(directory));
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
            _searchPattern = searchPattern;
        }

        public Task<NodeInputData[]> LoadDocumentsAsync()
        {
            var files = _searchPattern == null 
                ? _directory.EnumerateFiles()
                : _directory.EnumerateFiles(_searchPattern);

            return Task.WhenAll(from file in files select LoadDocumentAsync(file));

        }

        /// <summary>
        /// Load a single file
        /// </summary>
        private async Task<NodeInputData> LoadDocumentAsync(FileInfo file)
        {
            Trace.TraceInformation($"NodeDocumentDirectoryBatch.LoadDocumentAsync(file: {file.Name})");
            using (var stream = file.OpenRead())
            {
                NodeInputData task = await _reader.ParseNodeInputAsync(stream).ConfigureAwait(false);
                return task;
            }
        }
    }
}
