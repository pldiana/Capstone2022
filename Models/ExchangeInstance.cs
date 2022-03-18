using System;
using System.Collections.Generic;

namespace Models
{
    public class ExchangeInstance
    {
        public Exchange Exchange { get; set; }
        public string Key { get; set; }
        public string Signature { get; set; }
        public string Phrase { get; set; }
        public string AccountName { get; set; }
        public bool? AutoLiquidate { get; set; }
        public decimal? ExchangeLiquidationPercentage { get; set; }
        public List<StrategyInstance> StrategyList { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsSandbox { get; set; }

    }
}
