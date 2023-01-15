using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trady.Core.Infrastructure;

namespace Models
{
    public class Candle : IOhlcv
    {
        public DateTime Timestamp { get; set; }
        public string Symbol { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> ExtendedProperties { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public TradeAdvice? TradeAdvice { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string StrategyType { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? CurrentPrice { get; set; }

        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public Object Id { get; set; }

    }
}
