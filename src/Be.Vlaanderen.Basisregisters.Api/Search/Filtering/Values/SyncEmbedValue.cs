namespace Be.Vlaanderen.Basisregisters.Api.Search.Filtering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;
    using FluentValidation;
    using FluentValidation.Results;
    using Helpers;

    public class SyncEmbedValue
    {
        private static readonly Regex AllowParseRegex = new Regex("[,a-z]+", RegexOptions.IgnoreCase);

        private EmbedOption _value;

        public bool Event
        {
            get => _value.HasFlag(EmbedOption.Event);
            set => Set(EmbedOption.Event, value);
        }

        public bool Object
        {
            get => _value.HasFlag(EmbedOption.Object);
            set => Set(EmbedOption.Object, value);
        }

        private void Set(EmbedOption option, bool value)
        {
            if (value)
            {
                _value |= option;
            }
            else
            {
                _value &= ~option;
            }
        }

        public SyncEmbedValue()
            : this(EmbedOption.None) { }

        private SyncEmbedValue(EmbedOption value)
            => _value = value;

        public override string ToString()
        {
            var options = Enum
                .GetValues(typeof(EmbedOption))
                .Cast<EmbedOption>()
                .Where(option => option != EmbedOption.None && option != EmbedOption.All);

            return string
                .Join(',', options.Where(option => _value.HasFlag(option)))
                .ToLowerInvariant();
        }

        public static SyncEmbedValue Parse(string value)
        {
            if (value.IsNullOrWhiteSpace())
            {
                return new SyncEmbedValue(EmbedOption.None);
            }

            if (AllowParseRegex.IsMatch(value)
                && Enum.TryParse(typeof(EmbedOption), value, true, out var result))
            {
                return new SyncEmbedValue((EmbedOption)result);
            }

            throw new InvalidOptionException(value);
        }

        // Support deconstructing from string-value
        public static implicit operator SyncEmbedValue(string value)
            => Parse(value);

        [Serializable]
        public sealed class InvalidOptionException : ValidationException
        {
            public InvalidOptionException(string argumentValue)
                : base("Invalid embed option", GetFailures(argumentValue))
            { }

            private InvalidOptionException(SerializationInfo info, StreamingContext context)
                : base(info, context)
            { }

            private static IEnumerable<ValidationFailure> GetFailures(string argumentValue)
                => new[]
                {
                    new ValidationFailure(
                        "embed",
                        $"De waarde '{argumentValue}' is ongeldig. U kan enkel 'event', 'object' of 'event,object' meegeven.",
                        argumentValue)
                };
        }

        [Flags]
        private enum EmbedOption
        {
            None = 0,
            Event = 1,
            Object = 2,
            All = Event | Object
        }
    }
}
