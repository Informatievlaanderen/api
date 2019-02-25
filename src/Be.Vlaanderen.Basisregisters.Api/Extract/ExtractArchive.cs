namespace Be.Vlaanderen.Basisregisters.Api.Extract
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Net.Http.Headers;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Net.Mime;
    using System.Threading;
    using System.Threading.Tasks;

    public class ExtractArchive : IEnumerable
    {
        private readonly string _fileName;
        private readonly List<ExtractFile> _files = new List<ExtractFile>();

        public ExtractArchive(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            _fileName = name.EndsWith(".zip")
                ? name
                : name.TrimEnd('.') + ".zip";
        }

        public void Add(ExtractFile fileWriter)
        {
            if (fileWriter != null)
                _files.Add(fileWriter);
        }

        public void Add(IEnumerable<ExtractFile> files)
        {
            if (files != null)
                _files.AddRange(files);
        }

        public FileResult CreateCallbackFileStreamResult(
            string fileName,
            IEnumerable<ExtractFile> files,
            CancellationToken token)
            => new FileCallbackResult(
                    new MediaTypeHeaderValue(MediaTypeNames.Application.Octet),
                    (stream, _) => Task.Run(() => WriteArchiveContent(stream, token), token)) { FileDownloadName = fileName };

        private void WriteArchiveContent(Stream archiveStream, CancellationToken token)
        {
            using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create))
            {
                foreach (var file in _files.Where(file => null != file))
                {
                    if (token.IsCancellationRequested)
                        break;

                    using (var fileStream = archive.CreateEntry(file.Name).Open())
                        file.WriteTo(fileStream, token);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => _files.GetEnumerator();
    }
}
