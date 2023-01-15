using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CryptoToken
    {
        public string Symbol { get; set; }
        public string Base { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? TradeAmount { get; set; }
        public TimeFrame? Period { get; set; }
       
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public dynamic Properties { get; set; }
    }
}
