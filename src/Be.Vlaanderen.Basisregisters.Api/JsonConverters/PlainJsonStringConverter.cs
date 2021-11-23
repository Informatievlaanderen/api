namespace Be.Vlaanderen.Basisregisters.Api.JsonConverters
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Converts a string to a JSON object (without quotes)
    /// </summary>
    public class PlainStringJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Read is not supported.");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue((string)value);
        }
    }
}
