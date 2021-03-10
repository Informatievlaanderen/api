namespace Be.Vlaanderen.Basisregisters.Api.Extract
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Net.Http.Headers;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Net.Mime;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public class IsolationExtractArchive : IEnumerable
    {
        private readonly DbContext _context;
        private readonly string _fileName;
        private readonly List<ExtractFile> _files = new List<ExtractFile>();

        public IsolationExtractArchive(string name, DbContext context)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            _context = context ?? throw new ArgumentNullException(nameof(context));

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

        public FileResult CreateFileCallbackResult(
            CancellationToken token)
            => new FileCallbackResult(
                    new MediaTypeHeaderValue(MediaTypeNames.Application.Octet),
                    (stream, _) => Task.Run(async () => await WriteArchiveContent(stream, token), token)) { FileDownloadName = _fileName };

        private async Task WriteArchiveContent(Stream archiveStream, CancellationToken token)
        {
            using (await _context.Database.BeginTransactionAsync(IsolationLevel.Snapshot, token))
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
        }

        IEnumerator IEnumerable.GetEnumerator() => _files.GetEnumerator();
    }
}
