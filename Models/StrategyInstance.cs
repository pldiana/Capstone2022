using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class StrategyInstance
    {
        public bool? AutoLiquidate { get; set; }
        public decimal? LiquidationPercentage { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public dynamic Settings { get; set; }
        public List<CryptoToken> Tokens { get; set; }
        public Strategy StrategyDetail { get; set; }
        public bool? IsActive { get; set; } 
        public Candle ActiveCandle { get; set; }
    }
}
