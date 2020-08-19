namespace Be.Vlaanderen.Basisregisters.Api.LastObservedPosition
{
    using System;
    using System.Text.RegularExpressions;
    using Microsoft.AspNetCore.Http;
    using NodaTime;

    public class LastObservedPositionWithCrabDependency : LastObservedPosition
    {
        public Instant ExecutionTime { get; }

        public LastObservedPositionWithCrabDependency(long lastObservedPosition, Instant executionTime)
            : base(lastObservedPosition)
            => ExecutionTime = executionTime;

        public LastObservedPositionWithCrabDependency(HttpRequest request)
            : this(GetHeaderValuesFrom(request)) { }

        private LastObservedPositionWithCrabDependency(LastObservedPositionHeaderValues headerValues)
            : this (headerValues.Position, headerValues.ExecutionTime) { }

        public override string ToString()
            => $"{Position}|{ExecutionTime.ToUnixTimeMilliseconds()}";

        private static LastObservedPositionHeaderValues GetHeaderValuesFrom(HttpRequest request)
        {
            var match = new Regex(@"^(?<position>\d+)\|(?<execution_time>\d+)$").Match(GetHeaderValue(request));

            T MapCapture<T>(string groupName, Func<long, T> convert, T defaultValue)
                => match.Success && long.TryParse(match.Groups[groupName].Value, out var value)
                    ? convert(value)
                    : defaultValue;

            return new LastObservedPositionHeaderValues
            {
                Position = MapCapture("position", val => val, -1),
                ExecutionTime = MapCapture("execution_time", Instant.FromUnixTimeMilliseconds, Instant.MinValue)
            };
        }
        
        private class LastObservedPositionHeaderValues
        {
            public long Position { get; set; }
            public Instant ExecutionTime { get; set; }
        }
    }
}
