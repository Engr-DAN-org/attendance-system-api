using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace api.Utils
{
    public class PhilippineTimeConverter : IsoDateTimeConverter
    {
        public PhilippineTimeConverter()
        {
            DateTimeFormat = "yyyy-MM-dd'T'HH:mm"; // (without seconds)
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) return null;

            if (!DateTime.TryParse(reader.Value.ToString(), null, DateTimeStyles.RoundtripKind, out DateTime parsedTime))
                return null;
            //  Check if DateTime is UTC; only convert if needed
            if (parsedTime.Kind == DateTimeKind.Utc)
            {
                TimeZoneInfo phTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Manila");
                return TimeZoneInfo.ConvertTimeFromUtc(parsedTime, phTimeZone);
            }

            return parsedTime;
        }
    }
}