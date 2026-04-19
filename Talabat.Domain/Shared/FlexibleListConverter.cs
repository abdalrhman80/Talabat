using System.Text.Json;
using System.Text.Json.Serialization;

namespace Talabat.Domain.Shared
{
    public class FlexibleListConverter<T> : JsonConverter<List<T>>
    {
        public override List<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Already an array — deserialize normally
            if (reader.TokenType == JsonTokenType.StartArray)
                return JsonSerializer.Deserialize<List<T>>(ref reader, options) ?? [];

            // Object with numeric keys — extract values
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                var result = new List<T>();
                var dict = JsonSerializer.Deserialize<Dictionary<string, T>>(ref reader, options);
                if (dict != null)
                    result.AddRange(dict.Values);
                return result;
            }

            return [];
        }

        public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options) => JsonSerializer.Serialize(writer, value, options);
    }
}
