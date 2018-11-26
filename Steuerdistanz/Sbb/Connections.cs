
namespace Sbb.Connections 
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;


    public partial class RootNode
    {
        [JsonProperty("connections")]
        public List<Connection> Connections { get; set; }

        [JsonProperty("from")]
        public FromElement From { get; set; }

        [JsonProperty("to")]
        public FromElement To { get; set; }

        [JsonProperty("stations")]
        public Stations Stations { get; set; }
    }

    public partial class Connection
    {
        [JsonProperty("from")]
        public ConnectionFrom From { get; set; }

        [JsonProperty("to")]
        public To To { get; set; }

        // [JsonProperty("duration")]
        // public string Duration { get; set; }

        [JsonProperty("duration")]
        [JsonConverter(typeof(ParseTimespanConverter))]
        public System.TimeSpan? Duration { get; set; }

        [JsonProperty("transfers")]
        public long? Transfers { get; set; }

        [JsonProperty("service")]
        public object Service { get; set; }

        [JsonProperty("products")]
        public List<string> Products { get; set; }

        [JsonProperty("capacity1st")]
        public object Capacity1St { get; set; }

        [JsonProperty("capacity2nd")]
        public object Capacity2Nd { get; set; }

        [JsonProperty("sections")]
        public List<Section> Sections { get; set; }
    }

    public partial class ConnectionFrom
    {
        [JsonProperty("station")]
        public FromElement Station { get; set; }

        [JsonProperty("arrival")]
        public object Arrival { get; set; }

        [JsonProperty("arrivalTimestamp")]
        public object ArrivalTimestamp { get; set; }

        [JsonProperty("departure")]
        public string Departure { get; set; }

        [JsonProperty("departureTimestamp")]
        public long? DepartureTimestamp { get; set; }

        [JsonProperty("delay")]
        public object Delay { get; set; }

        [JsonProperty("platform")]
        // [JsonConverter(typeof(ParseStringConverter))]
        public string Platform { get; set; }

        [JsonProperty("prognosis")]
        public FromPrognosis Prognosis { get; set; }

        [JsonProperty("realtimeAvailability")]
        public object RealtimeAvailability { get; set; }

        [JsonProperty("location")]
        public FromElement Location { get; set; }
    }

    public partial class FromElement
    {
        [JsonProperty("id")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long? Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("score")]
        public object Score { get; set; }

        [JsonProperty("coordinate")]
        public Coordinate Coordinate { get; set; }

        [JsonProperty("distance")]
        public object Distance { get; set; }
    }

    public partial class Coordinate
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("x")]
        public double? X { get; set; }

        [JsonProperty("y")]
        public double? Y { get; set; }
    }

    public partial class FromPrognosis
    {
        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("arrival")]
        public object Arrival { get; set; }

        [JsonProperty("departure")]
        public object Departure { get; set; }

        [JsonProperty("capacity1st")]
        public object Capacity1St { get; set; }

        [JsonProperty("capacity2nd")]
        public object Capacity2Nd { get; set; }
    }

    public partial class Section
    {
        [JsonProperty("journey")]
        public Journey Journey { get; set; }

        [JsonProperty("walk")]
        public object Walk { get; set; }

        [JsonProperty("departure")]
        public Departure Departure { get; set; }

        [JsonProperty("arrival")]
        public Arrival Arrival { get; set; }
    }

    public partial class Arrival
    {
        [JsonProperty("station")]
        public FromElement Station { get; set; }

        [JsonProperty("arrival")]
        public string ArrivalArrival { get; set; }

        [JsonProperty("arrivalTimestamp")]
        public long? ArrivalTimestamp { get; set; }

        [JsonProperty("departure")]
        public object Departure { get; set; }

        [JsonProperty("departureTimestamp")]
        public object DepartureTimestamp { get; set; }

        [JsonProperty("delay")]
        public long? Delay { get; set; }

        [JsonProperty("platform")]
        // [JsonConverter(typeof(ParseStringConverter))]
        public string Platform { get; set; }

        [JsonProperty("prognosis")]
        public ArrivalPrognosis Prognosis { get; set; }

        [JsonProperty("realtimeAvailability")]
        public object RealtimeAvailability { get; set; }

        [JsonProperty("location")]
        public FromElement Location { get; set; }
    }

    public partial class ArrivalPrognosis
    {
        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("arrival")]
        public string Arrival { get; set; }

        [JsonProperty("departure")]
        public object Departure { get; set; }

        [JsonProperty("capacity1st")]
        public object Capacity1St { get; set; }

        [JsonProperty("capacity2nd")]
        public object Capacity2Nd { get; set; }
    }

    public partial class Departure
    {
        [JsonProperty("station")]
        public FromElement Station { get; set; }

        [JsonProperty("arrival")]
        public string Arrival { get; set; }

        [JsonProperty("arrivalTimestamp")]
        public long? ArrivalTimestamp { get; set; }

        [JsonProperty("departure")]
        public string DepartureDeparture { get; set; }

        [JsonProperty("departureTimestamp")]
        public long? DepartureTimestamp { get; set; }

        [JsonProperty("delay")]
        public long? Delay { get; set; }

        [JsonProperty("platform")]
        // [JsonConverter(typeof(ParseStringConverter))]
        public string Platform { get; set; }

        [JsonProperty("prognosis")]
        public DeparturePrognosis Prognosis { get; set; }

        [JsonProperty("realtimeAvailability")]
        public object RealtimeAvailability { get; set; }

        [JsonProperty("location")]
        public FromElement Location { get; set; }
    }

    public partial class DeparturePrognosis
    {
        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("arrival")]
        public object Arrival { get; set; }

        [JsonProperty("departure")]
        public string Departure { get; set; }

        [JsonProperty("capacity1st")]
        public object Capacity1St { get; set; }

        [JsonProperty("capacity2nd")]
        public object Capacity2Nd { get; set; }
    }

    public partial class Journey
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("subcategory")]
        public object Subcategory { get; set; }

        [JsonProperty("categoryCode")]
        public object CategoryCode { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("operator")]
        public string Operator { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("passList")]
        public List<PassList> PassList { get; set; }

        [JsonProperty("capacity1st")]
        public object Capacity1St { get; set; }

        [JsonProperty("capacity2nd")]
        public object Capacity2Nd { get; set; }
    }

    public partial class PassList
    {
        [JsonProperty("station")]
        public FromElement Station { get; set; }

        [JsonProperty("arrival")]
        public string Arrival { get; set; }

        [JsonProperty("arrivalTimestamp")]
        public long? ArrivalTimestamp { get; set; }

        [JsonProperty("departure")]
        public string Departure { get; set; }

        [JsonProperty("departureTimestamp")]
        public long? DepartureTimestamp { get; set; }

        [JsonProperty("delay")]
        public long? Delay { get; set; }

        [JsonProperty("platform")]
        // [JsonConverter(typeof(ParseStringConverter))]
        public string Platform { get; set; }

        [JsonProperty("prognosis")]
        public PassListPrognosis Prognosis { get; set; }

        [JsonProperty("realtimeAvailability")]
        public object RealtimeAvailability { get; set; }

        [JsonProperty("location")]
        public FromElement Location { get; set; }
    }

    public partial class PassListPrognosis
    {
        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("arrival")]
        public string Arrival { get; set; }

        [JsonProperty("departure")]
        public string Departure { get; set; }

        [JsonProperty("capacity1st")]
        public object Capacity1St { get; set; }

        [JsonProperty("capacity2nd")]
        public object Capacity2Nd { get; set; }
    }

    public partial class To
    {
        [JsonProperty("station")]
        public FromElement Station { get; set; }

        [JsonProperty("arrival")]
        public string Arrival { get; set; }

        [JsonProperty("arrivalTimestamp")]
        public long? ArrivalTimestamp { get; set; }

        [JsonProperty("departure")]
        public object Departure { get; set; }

        [JsonProperty("departureTimestamp")]
        public object DepartureTimestamp { get; set; }

        [JsonProperty("delay")]
        public object Delay { get; set; }

        [JsonProperty("platform")]
        // [JsonConverter(typeof(ParseStringConverter))]
        public string Platform { get; set; }

        [JsonProperty("prognosis")]
        public FromPrognosis Prognosis { get; set; }

        [JsonProperty("realtimeAvailability")]
        public object RealtimeAvailability { get; set; }

        [JsonProperty("location")]
        public FromElement Location { get; set; }
    }

    public partial class Stations
    {
        [JsonProperty("from")]
        public List<FromElement> From { get; set; }

        [JsonProperty("to")]
        public List<FromElement> To { get; set; }
    }

    public partial class RootNode
    {
        public static RootNode FromJson(string json)
        {
            return JsonConvert.DeserializeObject<RootNode>(json, Converter.Settings);
        }
    }

    public static class Serialize
    {
        public static string ToJson(this RootNode self)
        {
            return JsonConvert.SerializeObject(self, Converter.Settings);
        }
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }


    internal class ParseTimespanConverter 
        : JsonConverter
    {
        public override bool CanConvert(Type t)
        {
            return t == typeof(System.TimeSpan?) || t == typeof(System.TimeSpan?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            string value = serializer.Deserialize<string>(reader);
            System.TimeSpan t;

            // string value = "00d02:07:00";
            // if (System.TimeSpan.TryParse(value, out t))
            if (System.TimeSpan.TryParseExact(value, @"dd'd'hh':'mm':'ss", System.Globalization.CultureInfo.InvariantCulture, out t))
            {
                return t;
            }
            throw new Exception("Cannot unmarshal type System.TimeSpan?");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            System.TimeSpan? value = (System.TimeSpan?)untypedValue;

            serializer.Serialize(writer, value.Value.ToString(@"dd'd'hh':'mm':'ss", System.Globalization.CultureInfo.InvariantCulture));
        }
    }


    internal class ParseStringConverter 
        : JsonConverter
    {
        public override bool CanConvert(Type t)
        {
            return t == typeof(long?) || t == typeof(long?);
        }

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long?");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            long? value = (long?)untypedValue;
            serializer.Serialize(writer, value.ToString());
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}
