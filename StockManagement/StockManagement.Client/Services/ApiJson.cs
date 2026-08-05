using System.Text.Json;
using System.Text.Json.Serialization;

namespace StockManagement.Client.Services
{
    /// <summary>
    /// JSON settings for every call to the API.
    /// </summary>
    public static class ApiJson
    {
        public static readonly JsonSerializerOptions Options = CreateOptions();

        private static JsonSerializerOptions CreateOptions()
        {
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            options.Converters.Add(new UnspecifiedKindDateTimeConverter());
            return options;
        }

        /// <summary>
        /// Writes dates with no timezone offset, and reads them back as
        /// <see cref="DateTimeKind.Unspecified"/>.
        ///
        /// Blazor WebAssembly runs in the browser's local time, so a date picked
        /// as midnight on 31 March serialises as "2026-03-31T00:00:00+01:00"
        /// during British Summer Time. The server container runs in UTC, so
        /// System.Text.Json converted that back to 2026-03-30T23:00:00 and the
        /// transaction was stored a day early. In winter the offset is zero and
        /// nothing appeared to be wrong, which is why it went unnoticed - 42
        /// sale receipts were affected before it was found.
        ///
        /// These are calendar dates, not instants, so they must travel without
        /// an offset and never be shifted.
        /// </summary>
        private sealed class UnspecifiedKindDateTimeConverter : JsonConverter<DateTime>
        {
            private const string Format = "yyyy-MM-ddTHH:mm:ss";

            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var value = reader.GetString();

                if (string.IsNullOrEmpty(value))
                {
                    return default;
                }

                // Round-trip whatever the server sent, then drop any offset so
                // the value is treated as a plain calendar date.
                var parsed = DateTime.Parse(value, System.Globalization.CultureInfo.InvariantCulture,
                                            System.Globalization.DateTimeStyles.RoundtripKind);

                return DateTime.SpecifyKind(parsed, DateTimeKind.Unspecified);
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString(Format, System.Globalization.CultureInfo.InvariantCulture));
            }
        }
    }
}
