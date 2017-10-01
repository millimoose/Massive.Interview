using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Massive.Interview.LoaderApp
{
    /// <summary>
    /// A document batch that loads all files in a directory.
    /// </summary>
    class NodeDocumentDirectoryBatch : INodeDocumentBatch
    {
        private DirectoryInfo _directory;
        private INodeDocumentReader _reader;
        private string _searchPattern;

        public NodeDocumentDirectoryBatch(DirectoryInfo directory, INodeDocumentReader reader, string searchPattern)
        {
            _directory = directory ?? throw new ArgumentNullException(nameof(directory));
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
            _searchPattern = searchPattern;
        }

        public Task<NodeInput[]> LoadDocumentsAsync()
        {
            var files = _searchPattern == null 
                ? _directory.EnumerateFiles()
                : _directory.EnumerateFiles(_searchPattern);

            return Task.WhenAll(from file in files select LoadDocumentAsync(file));

        }

        /// <summary>
        /// Load a single file
        /// </summary>
        private async Task<NodeInput> LoadDocumentAsync(FileInfo file)
        {
            using (var stream = file.OpenRead())
            {
                NodeInput task = await _reader.ParseNodeInputAsync(stream).ConfigureAwait(false);
                task.Source = file.FullName;
                return task;
            }
        }
    }
}
