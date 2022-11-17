namespace Be.Vlaanderen.Basisregisters.Api.Extract
{
    using System;
    using System.IO;
    using System.Text;

    public abstract class ExtractFileWriter : IDisposable
    {
        protected readonly BinaryWriter Writer;

        protected ExtractFileWriter(Encoding encoding, Stream contentStream)
        {
            if (null == encoding)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            if (contentStream == null)
            {
                throw new ArgumentNullException(nameof(contentStream));
            }

            Writer = new BinaryWriter(contentStream, encoding, leaveOpen: true);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Writer.Dispose();
            }
        }
    }
}
