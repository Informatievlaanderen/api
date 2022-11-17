namespace Be.Vlaanderen.Basisregisters.Api.Extract
{
    using System;

    public class ExtractFileName
    {
        private readonly string _name;
        private readonly string _extension;

        public ExtractFileName(string name, string extension)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(extension))
            {
                throw new ArgumentNullException(nameof(extension));
            }

            _name = name;
            _extension = extension;
        }

        public override string ToString()
        {
            var extension = _extension.Trim();
            if (!extension.StartsWith('.'))
            {
                extension = "." + extension;
            }

            var name = _name
                .Trim()
                .TrimEnd('.');

            var nameIncludesExtension = name.ToLowerInvariant().EndsWith(extension.ToLowerInvariant());

            return nameIncludesExtension ? name : name + extension;
        }

        public static implicit operator string(ExtractFileName name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return name.ToString();
        }
    }
}
