namespace Be.Vlaanderen.Basisregisters.Api.Search.Filtering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentValidation;
    using FluentValidation.Results;
    using Helpers;

    public class EmbedValue
    {
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
                _value |= option;
            else
                _value &= ~option;
        }

        public EmbedValue()
            : this(EmbedOption.None) { }

        private EmbedValue(EmbedOption value)
            => _value = value;

        private static EmbedOption ParseOption(string value)
        {
            if (value.IsNullOrWhiteSpace())
                return EmbedOption.None;

            if (value.Contains(','))
                return value
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(EmbedOption.None, (embed, option) => embed | ParseOption(option));

            if (Enum.TryParse(typeof(EmbedOption), value, true, out var result))
                return (EmbedOption)result;

            throw new InvalidOptionException(value);
        }

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

        public static EmbedValue Parse(string value)
            => new EmbedValue(ParseOption(value));

        // Support deconstructing from string-value
        public static implicit operator EmbedValue(string value)
            => Parse(value);

        public class InvalidOptionException : ValidationException
        {
            public InvalidOptionException(string argumentValue)
                : base("Invalid embed option", GetFailures(argumentValue)) { }

            private static IEnumerable<ValidationFailure> GetFailures(string argumentValue)
                => new[]
                {
                    new ValidationFailure(
                        "embed",
                        $"The value '{argumentValue}' is not a valid embed option.",
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
