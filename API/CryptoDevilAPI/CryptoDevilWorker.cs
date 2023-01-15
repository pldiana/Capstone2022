using CoinbasePro;
using CoinbasePro.Network.Authentication;
using CoinbasePro.Services.Accounts.Models;
using CoinbasePro.Services.Products.Models;
using CryptoDevilAPI.DataAccess;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models;
using Models.CryptoWatch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Trady.Analysis.Extension;

namespace TradeProcessor
{
    public class CryptoDevilWorker : BackgroundService
    {
        private readonly ILogger<CryptoDevilWorker> _logger;
        private readonly IUserExchangeDA _userExchangeDA;
        private readonly ICandleDataDA _candleDataDA;
        private List<UserExchange> _userExchanges;
        public static event TradeAdviceEventHandler TradeEventHandler;

        public CryptoDevilWorker(ILogger<CryptoDevilWorker> logger, IUserExchangeDA userExchangeDA, ICandleDataDA candleDataDA)
        {
            _logger = logger;
            _userExchangeDA = userExchangeDA;
            _candleDataDA = candleDataDA;

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{DateTime.Now} Starting Worker Processing Run");


                try
                {
                    _userExchanges = await _userExchangeDA.GetActiveAsync();
                    await Run();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unhandled Exception");
                }
                _logger.LogInformation($"{DateTime.Now} End Worker Processing Run");
                _logger.LogInformation($"{DateTime.Now} Next Run: {DateTime.Now.AddMilliseconds(300000)}");
                await Task.Delay(300000, stoppingToken);

            }
        }
        private async Task Run()
        {
            List<StrategyInstance> strategies = new List<StrategyInstance>()
            {
                new StrategyInstance()
                {
                    Tokens = new List<CryptoToken>()
                    {
                         new CryptoToken()
                         {
                           Symbol = "btc",
                           Base = "usd",
                           TradeAmount = 1000m,
                           Period = Models.TimeFrame.h1,
                           Properties = new { RsiLow = 55m, CciLow = -10m, RsiHigh = 60m, CciHigh = 10m }
                         }
                    },
                    StrategyDetail = new Strategy()
                    {
                        IsActive = true,
                        Name = "rsicii"
                    },
                    IsActive = true,
                    AutoLiquidate = false
                },
            };

            var handlers = _userExchanges.Select(s => new UserExchangeHandler(_logger, s, new ExchangeFactory())).ToList();
            List<StrategyInstance> strategyInstanceList = new List<StrategyInstance>();

            _userExchanges.ForEach(u =>
            {
                u.ExchangeList.ForEach(e =>
                {
                    e.StrategyList.ForEach(si =>
                    {
                        if (!strategyInstanceList.Any(sil => sil.StrategyDetail.Name == si.StrategyDetail.Name))
                            strategyInstanceList.Add(si);
                    });
                });
            });

            foreach (var si in strategyInstanceList)
            {
                foreach (var token in si.Tokens)
                {
                    var strategySettings = strategies.Where(t => t.StrategyDetail.Name.ToLower() == si.StrategyDetail.Name.ToLower()).SingleOrDefault();
                    if (strategySettings != null)
                    {
                        CryptoToken tokenSettings = strategySettings.Tokens.SingleOrDefault(t => t.Symbol.ToLower() == token.Symbol.ToLower());
                        if (tokenSettings != null)
                        {
                            IStrategy strategy = StrategyFactory.GetStrategy(si.StrategyDetail.Name, tokenSettings.Properties);
                            if (strategy != null)
                            {
                                List<Models.Candle> candleDetail = null;

                                string uriTemplate = "https://api.cryptowat.ch/markets/{0}/{1}usd/ohlc";
                                var exchange = "coinbase-pro";
                                candleDetail = OHLCAPI.GetCandlesticks(string.Format(uriTemplate, exchange, token.Symbol), token.Symbol, tokenSettings.Period.HasValue ? tokenSettings.Period.Value : TimeFrame.h1, DateTime.UtcNow.AddDays(-60).ToUnixTimestamp(), 0);//, "YGMJLKIA4TLB42I3NEL5");
                                var strategyResult = strategy.PrepareWithDetail(candleDetail, token.Symbol);
                                strategyResult.Last().CurrentPrice = candleDetail.Last().Close;

                                if (TradeEventHandler != null && strategyResult != null)
                                {
                                    _logger.LogInformation($"Symbol: {strategyResult.Last().Symbol} - TradeAdvice: {strategyResult.Last().TradeAdvice.ToString()}");
                                    TradeEventHandler.Invoke(this,
                                        new TradeEventArgs
                                        {
                                            TradeAdviceCandle = strategyResult.Last()
                                        });
                                    await _candleDataDA.InsertOneAsync(strategyResult.Last());
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public class StrategyFactory
    {
        public static IStrategy GetStrategy(string name, dynamic properties)
        {
            IStrategy strategy = null;

            switch (name.ToLower())
            {
                case "rsicci":
                case "rsicii":
                    strategy = new RsiCci(properties);
                    break;
                default:
                    break;
            }
            return strategy;
        }
    }
    public enum Period
    {
        Minute,
        FiveMinutes,
        QuarterOfAnHour,
        HalfAnHour,
        Hour,
        Day,
        TwoHours,
        FourHours
    }
    public class CciResult
    {
        public int Index { get; set; }
        public DateTime Date { get; set; }
        internal decimal? Tp { get; set; }
        public decimal? Cci { get; set; }
    }
    public enum CandleVariable
    {
        High,
        Low,
        Close,
        Open
    }
    public static partial class Extensions
    {
        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            return Convert.ToInt64((TimeZoneInfo.ConvertTimeToUtc(dateTime) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds);
        }

        private static decimal Factor = 0.015M;

        public static List<decimal?> Sma(this List<Models.Candle> source, int period = 30, CandleVariable type = CandleVariable.Close)
        {
            double[] smaValues = new double[source.Count];
            List<double?> outValues = new List<double?>();
            double[] valuesToCheck;

            switch (type)
            {
                case CandleVariable.Open:
                    valuesToCheck = source.Select(x => Convert.ToDouble(x.Open)).ToArray();
                    break;
                case CandleVariable.Low:
                    valuesToCheck = source.Select(x => Convert.ToDouble(x.Low)).ToArray();
                    break;
                case CandleVariable.High:
                    valuesToCheck = source.Select(x => Convert.ToDouble(x.High)).ToArray();
                    break;
                default:
                    valuesToCheck = source.Select(x => Convert.ToDouble(x.Close)).ToArray();
                    break;
            }

            return source.AsEnumerable().Sma(period).Select(x => x.Tick).ToList();

            throw new Exception("Could not calculate SMA!");
        }

        private static List<decimal?> FixIndicatorOrdering(List<double> items, int outBegIdx, int outNbElement)
        {
            var outValues = new List<decimal?>();
            var validItems = items.Take(outNbElement);

            for (int i = 0; i < outBegIdx; i++)
                outValues.Add(null);

            foreach (var value in validItems)
                outValues.Add((decimal?)value);

            return outValues;
        }

        public static List<decimal?> Sma(this List<decimal> source, int period = 30)
        {
            int outBegIdx, outNbElement;
            double[] smaValues = new double[source.Count];
            List<double?> outValues = new List<double?>();

            var sourceFix = source.Select(x => Convert.ToDouble(x)).ToArray();

            //TODO: Wire this up for Trady
            var sma = TicTacTec.TA.Library.Core.Sma(0, source.Count - 1, sourceFix, period, out outBegIdx, out outNbElement, smaValues);

            if (sma == TicTacTec.TA.Library.Core.RetCode.Success)
            {
                return FixIndicatorOrdering(smaValues.ToList(), outBegIdx, outNbElement);
            }

            throw new Exception("Could not calculate SMA!");
        }
        public static List<decimal?> Cci(this List<Models.Candle> source, int period = 22, int? startIndex = 0, int? endIndex = null)
        {
            //double[] outReal = null;
            //endIndex = source.Count - 1;
            //int outBegIdx;
            // int outNBElement;
            //var cci = source.AsEnumerable().Cci(period).Select(x => x.Tick).ToList();
            //TicTacTec.TA.Library.Core.Cci(startIndex.Value, endIndex.Value, source.Select(s => ((double)s.High)).ToArray(), source.Select(s => ((double)s.Low)).ToArray(), source.Select(s => ((double)s.Close)).ToArray(), period,
            //    out outBegIdx, out outNBElement, outReal);

            return GetCci(source, period);
            //return outReal.Select(s=>(decimal?)s).ToList();
            //return cci;
            throw new Exception("Could not calculate CCI!");
        }

        public static List<decimal?> Calculate(List<Models.Candle> OhlcList, int period)
        {
            List<decimal?> cciSeries = new List<decimal?>();
            List<decimal?> tpList = new List<decimal?>();

            for (int i = 0; i < OhlcList.Count; i++)
            {
                tpList.Add((OhlcList[i].High + OhlcList[i].Low + OhlcList[i].Close) / 3);
            }

            List<decimal?> smaList = OhlcList.Sma(period);

            List<decimal?> meanDeviationList = new List<decimal?>();
            for (int i = 0; i < OhlcList.Count; i++)
            {
                if (i >= period - 1)
                {
                    decimal total = 0.0M;
                    for (int j = i; j >= i - (period - 1); j--)
                    {
                        total += Math.Abs(tpList[i].Value - smaList[i].Value);
                    }
                    meanDeviationList.Add(total / period);

                    decimal? cci = (tpList[i] - smaList[i].Value) / (Factor * meanDeviationList[i].Value);
                    cciSeries.Add(cci);
                }
                else
                {
                    meanDeviationList.Add(null);
                    cciSeries.Add(null);
                }
            }

            return cciSeries;
        }

        public static List<decimal?> GetCci(List<Models.Candle> history, int lookbackPeriod = 20)
        {

            // clean quotes
            //history = Cleaners.PrepareHistory(history);

            // initialize results
            List<CciResult> results = new List<CciResult>();

            int index = 0;
            // roll through history to get Typical Price
            foreach (Models.Candle h in history)
            {
                CciResult result = new CciResult
                {
                    Index = index,
                    Date = h.Timestamp,
                    Tp = (h.High + h.Low + h.Close) / 3
                };
                results.Add(result);
                index++;
            }


            // roll through interim results to calculate CCI
            foreach (CciResult result in results.Where(x => x.Index >= lookbackPeriod))
            {
                IEnumerable<CciResult> period = results.Where(x => x.Index <= result.Index && x.Index > (result.Index - lookbackPeriod));

                decimal smaTp = (decimal)period.Select(x => x.Tp).Average();
                decimal meanDv = 0;

                foreach (CciResult p in period)
                {
                    meanDv += Math.Abs(smaTp - (decimal)p.Tp);
                }
                meanDv /= lookbackPeriod;
                result.Cci = (result.Tp - smaTp) / ((decimal)0.015 * meanDv);
            }

            return results.Select(s => s.Cci).ToList();
        }

        public static List<decimal?> Rsi(this List<Models.Candle> source, int period = 14)
        {
            var rsi = source.AsEnumerable().Rsi(period).Select(x => x.Tick).ToList();

            return rsi;

            throw new Exception("Could not calculate RSI!");
        }
    }
    public class RsiCci : IStrategy
    {
        private decimal _rsiLower;
        private decimal _cciLower;
        private decimal _rsiHigher;
        private decimal _cciHigher;
        private Period _idealPeriod;
        public RsiCci()
        {
            _rsiLower = 35.0M;
            _rsiHigher = 65.0M;
            _cciHigher = 100.0M;
            _cciLower = -100M;
            _idealPeriod = Period.Hour;
        }

        public RsiCci(Dictionary<string, object> properties)
        {
            _rsiLower = decimal.Parse(properties["RsiLow"].ToString());
            _rsiHigher = decimal.Parse(properties["RsiHigh"].ToString());
            _cciHigher = decimal.Parse(properties["CciHigh"].ToString());
            _cciLower = decimal.Parse(properties["CciLow"].ToString());
            _idealPeriod = Period.Hour;
        }

        public RsiCci(dynamic properties)
        {
            _rsiLower = properties.RsiLow;
            _rsiHigher = properties.RsiHigh;
            _cciHigher = properties.CciHigh;
            _cciLower = properties.CciLow;
            _idealPeriod = Period.Hour;
        }

        public RsiCci(string symbol, ILogger logger)
        {
            _rsiLower = 35.0M;
            _rsiHigher = 65.0M;
            _cciHigher = 100.0M;
            _cciLower = -100M;
            _idealPeriod = Period.Hour;
        }

        public string Name => "rsicii";
        public int MinimumAmountOfCandles => 15;
        public Period IdealPeriod
        {
            get
            {
                return _idealPeriod;
            }
        }
        public List<TradeAdvice> Prepare(List<Models.Candle> candles)
        {
            var result = new List<TradeAdvice>();

            var cci = candles.Cci();
            var rsi = candles.Rsi();
            //var mfi = candles.Mfi();

            for (int i = 0; i < candles.Count; i++)
            {
                bool isBigDrop = false;
                bool isBigGain = false;

                candles[i].ExtendedProperties = new Dictionary<string, object>();
                candles[i].ExtendedProperties.Add("cci", cci[i]);
                candles[i].ExtendedProperties.Add("rsi", rsi[i]);
                //candles[i].ExtendedProperties.Add("mfi", mfi);

                if (i > 1)
                {
                    if (cci[i - 1].HasValue && cci[i - 2].HasValue && cci[i - 2].Value > cci[i - 1].Value)
                    {
                        var difference = Math.Abs((cci[i - 1].HasValue ? cci[i - 1].Value : 0.0M) - (cci[i - 2].HasValue ? cci[i - 2].Value : cci[i - 1].HasValue ? cci[i - 1].Value : 0.0M));
                        if (difference != 0.0M && difference > 45M)
                        {
                            isBigDrop = true;
                        }
                    }
                    if (cci[i].HasValue && cci[i - 1].HasValue && cci[i].Value > cci[i - 1].Value)
                    {

                        if ((cci[i].Value - cci[i - 1].Value) > 50.0m)
                        {
                            isBigGain = true;
                        }
                    }
                }
                candles[i].ExtendedProperties.Add("bigdrop", isBigDrop);
                candles[i].ExtendedProperties.Add("biggain", isBigGain);
                if (i == 0)
                    result.Add(TradeAdvice.Hold);
                else if (rsi[i] < _rsiLower && cci[i] < _cciLower && !isBigDrop)// || (cci[i] < -125M && (mfi.HasValue && mfi.Value < 20.0M)))
                {
                    result.Add(TradeAdvice.Buy);
                }
                else if ((rsi[i] > _rsiHigher && cci[i] > _cciHigher && !isBigGain))//|| (rsi[i] > 60.0M && cci[i] > 275.0M))//&& mfi[i].HasValue && mfi[i].Value > 80.0M)
                    result.Add(TradeAdvice.Sell);
                else
                    result.Add(TradeAdvice.Hold);
            }

            return result;
        }
        public List<Models.Candle> PrepareWithDetail(List<Models.Candle> candles, string symbol)
        {
            var result = new List<Models.Candle>();
            var cci = candles.Cci();
            var rsi = candles.Rsi();
            //var mfi = candles.Mfi();

            for (int i = 0; i < candles.Count; i++)
            {
                bool isBigDrop = false;
                bool isBigGain = false;
                if (i > 1)
                {
                    if (cci[i - 1].HasValue && cci[i - 2].HasValue && cci[i - 2].Value > cci[i - 1].Value)
                    {
                        var difference = Math.Abs((cci[i - 1].HasValue ? cci[i - 1].Value : 0.0M) - (cci[i - 2].HasValue ? cci[i - 2].Value : cci[i - 1].HasValue ? cci[i - 1].Value : 0.0M));
                        if (difference != 0.0M && difference > 45m)
                        {
                            isBigDrop = true;

                        }
                    }
                    if (cci[i].HasValue && cci[i - 1].HasValue && cci[i].Value > cci[i - 1].Value)
                    {
                        if ((cci[i].Value - cci[i - 1].Value) > 55.0m)
                        {
                            isBigGain = true;
                        }
                    }
                }

                Models.Candle strategyDetail = new Models.Candle()
                {
                    ExtendedProperties = new Dictionary<string, object>(),
                    StrategyType = Name,
                    DateTime = DateTime.UtcNow
                };
                strategyDetail.ExtendedProperties.Add("cci", cci[i]);
                strategyDetail.ExtendedProperties.Add("rsi", rsi[i]);
                //strategyDetail.ExtendedProperties.Add("mfi", mfi);
                strategyDetail.ExtendedProperties.Add("bigdrop", isBigDrop);
                strategyDetail.ExtendedProperties.Add("biggain", isBigGain);
                if (i == 0)
                {
                    strategyDetail.Timestamp = candles[i].Timestamp;
                    strategyDetail.Close = candles[i].Close;
                    strategyDetail.High = candles[i].High;
                    strategyDetail.Low = candles[i].Low;
                    strategyDetail.Open = candles[i].Open;
                    strategyDetail.Volume = candles[i].Volume;
                    strategyDetail.TradeAdvice = TradeAdvice.Hold;
                    strategyDetail.Symbol = symbol;
                    result.Add(strategyDetail);
                }
                else if (rsi[i] < _rsiLower && cci[i] < _cciLower && !isBigDrop)// || (cci[i] < -125M && (mfi.HasValue && mfi.Value < 20.0M)))
                {
                    strategyDetail.Timestamp = candles[i].Timestamp;
                    strategyDetail.Close = candles[i].Close;
                    strategyDetail.High = candles[i].High;
                    strategyDetail.Low = candles[i].Low;
                    strategyDetail.Open = candles[i].Open;
                    strategyDetail.Volume = candles[i].Volume;
                    strategyDetail.TradeAdvice = TradeAdvice.Buy;
                    strategyDetail.Symbol = symbol;
                    result.Add(strategyDetail);
                }
                else if ((rsi[i] > _rsiHigher && cci[i] > _cciHigher && !isBigGain))// || (rsi[i] > 60.0M && cci[i] > 275.0M))//&& mfi[i].HasValue && mfi[i].Value > 80.0M)
                {
                    strategyDetail.Timestamp = candles[i].Timestamp;
                    strategyDetail.Close = candles[i].Close;
                    strategyDetail.High = candles[i].High;
                    strategyDetail.Low = candles[i].Low;
                    strategyDetail.Open = candles[i].Open;
                    strategyDetail.Volume = candles[i].Volume;
                    strategyDetail.TradeAdvice = TradeAdvice.Sell;
                    strategyDetail.Symbol = symbol;
                    result.Add(strategyDetail);
                }

                else
                {
                    strategyDetail.Timestamp = candles[i].Timestamp;
                    strategyDetail.Close = candles[i].Close;
                    strategyDetail.High = candles[i].High;
                    strategyDetail.Low = candles[i].Low;
                    strategyDetail.Open = candles[i].Open;
                    strategyDetail.Volume = candles[i].Volume;
                    strategyDetail.TradeAdvice = TradeAdvice.Hold;
                    strategyDetail.Symbol = symbol;
                    result.Add(strategyDetail);
                }
            }

            return result;
        }
        public Models.Candle GetSignalCandle(List<Models.Candle> candles)
        {
            return candles.Last();
        }
        public TradeAdvice Forecast(List<Models.Candle> candles)
        {
            return Prepare(candles).LastOrDefault();
        }
    }
    public interface IStrategy
    {
        string Name { get; }
        List<Models.Candle> PrepareWithDetail(List<Models.Candle> candles, string symbol);
    }
    public class TradeEventArgs : EventArgs
    {
        public Models.Candle TradeAdviceCandle { get; set; }
    }

    public delegate void TradeAdviceEventHandler(object sender, TradeEventArgs e);
    public class UserExchangeHandler
    {
        private readonly ILogger _logger;
        public event TradeAdviceEventHandler _tradeEventHandler;
        private readonly UserExchange _userExchange;
        private readonly IExchangeFactory _exchangeFactory;


        public UserExchangeHandler(ILogger logger, UserExchange userExchange, IExchangeFactory exchangeFactory)
        {
            _logger = logger;
            CryptoDevilWorker.TradeEventHandler += TradeEventOccured;
            _userExchange = userExchange;
            _exchangeFactory = exchangeFactory;
        }

        void TradeEventOccured(object sender, TradeEventArgs e)
        {
            var exchangeInstance = _userExchange.ExchangeList
                .Where(x => x.StrategyList
                    .Where(sl => sl.Tokens
                        .Select(t => t.Symbol.ToLower())
                            .Contains(e.TradeAdviceCandle.Symbol.ToLower()) && sl.StrategyDetail.Name.ToLower() == e.TradeAdviceCandle.StrategyType.ToLower())
                    .Any())
                .ToList();

            exchangeInstance.ForEach(x =>
            {
                ITradingExchange exchange = _exchangeFactory.GetInstanceAsync(_userExchange, x, _logger).GetAwaiter().GetResult();
                StrategyInstance strategyInstance = x.StrategyList.SingleOrDefault(sl => sl.Tokens.Select(t => t.Symbol.ToLower()).Contains(e.TradeAdviceCandle.Symbol.ToLower()));
                CryptoToken token = strategyInstance.Tokens.SingleOrDefault(t => t.Symbol.ToLower() == e.TradeAdviceCandle.Symbol.ToLower());
                strategyInstance.ActiveCandle = e.TradeAdviceCandle;
                switch (e.TradeAdviceCandle.TradeAdvice.Value)
                {
                    case TradeAdvice.Buy:
                    case TradeAdvice.StrongBuy:
                        exchange.BuyAsync(strategyInstance, token).GetAwaiter().GetResult();
                        break;
                    case TradeAdvice.Sell:
                    case TradeAdvice.StrongSell:
                        exchange.SellAsync(strategyInstance, token).GetAwaiter().GetResult();
                        break;
                    default:
                        var result = x.StrategyList.SingleOrDefault(sl => sl.Tokens.Select(t => t.Symbol.ToLower()).Contains(e.TradeAdviceCandle.Symbol.ToLower()));
                        break;
                }
            });
        }
    }
    public interface IExchangeFactory
    {
        Task<ITradingExchange> GetInstanceAsync(UserExchange userExchange, ExchangeInstance exchangeInstance, ILogger logger);
    }
    public class ExchangeFactory : IExchangeFactory
    {
        public async Task<ITradingExchange> GetInstanceAsync(UserExchange userExchange, ExchangeInstance exchangeInstance, ILogger logger)
        {
            CoinbaseTradingExchange result = null;
            await Task.Run(() =>
            {
                switch (exchangeInstance.Exchange.Name)
                {
                    default:
                        result = new CoinbaseTradingExchange(logger, userExchange, exchangeInstance, new MockNotificationManager());
                        break;
                }
            });
            return result;
        }
    }
    public interface ITradingExchange
    {
        Task BuyAsync(StrategyInstance strategy, CryptoToken token);
        Task SellAsync(StrategyInstance strategy, CryptoToken token);
    }
    public class CoinbaseTradingExchange : ITradingExchange
    {
        private readonly UserExchange _userExchange;
        private readonly ExchangeInstance _exchangeInstance;
        private readonly CoinbaseProClient _exchangeClient;
        private readonly ILogger _logger;
        private readonly INotificationManager _notification;
        public CoinbaseTradingExchange(ILogger logger, UserExchange userExchange, ExchangeInstance exchangeInstance, INotificationManager notification)
        {
            _logger = logger;
            _notification = notification;
            _userExchange = userExchange;
            _exchangeInstance = exchangeInstance;
            var authenticator = new Authenticator(exchangeInstance.Key, exchangeInstance.Signature, exchangeInstance.Phrase);
            _exchangeClient = new CoinbaseProClient(authenticator, exchangeInstance.IsSandbox.HasValue ? exchangeInstance.IsSandbox.Value : true);
        }
        public async Task BuyAsync(StrategyInstance strategy, CryptoToken token)
        {
            try
            {
                var accounts = (await _exchangeClient.AccountsService.GetAllAccountsAsync()).ToList();
                var usdAccount = accounts.ToList().GetAccount("USD");
                var strategyAccount = accounts.GetAccount(token.Symbol.ToUpper());

                if (strategyAccount != null)
                {

                    var precision = _exchangeClient.ProductsService.GetAllProductsAsync().GetAwaiter().GetResult();

                    var product = precision.Where(w => w.BaseCurrency == token.Symbol.ToUpper() && w.QuoteCurrency == "USD").SingleOrDefault();
                    decimal amountToBuy;
                    if (product != null)
                    {
                        int priceprecision = BitConverter.GetBytes(decimal.GetBits(product.QuoteIncrement)[3])[2];
                        int quanityprecision = BitConverter.GetBytes(decimal.GetBits(product.BaseIncrement)[3])[2];
                        if (usdAccount.Available > 1.001m && (strategyAccount.Balance * strategy.ActiveCandle.CurrentPrice.Value) <= token.TradeAmount.Value * 10.0m)
                        {
                            var price = decimal.Round(strategy.ActiveCandle.CurrentPrice.Value * .997M, priceprecision, MidpointRounding.ToZero);
                            price = ((price == 0.0m && priceprecision == 2) ? 0.01m : price);
                            amountToBuy = (usdAccount.Available / price);//* .995M;
                            if (token.TradeAmount < usdAccount.Available)
                            {
                                amountToBuy = (token.TradeAmount.HasValue ? token.TradeAmount.Value : 0.0M) / price;
                            }
                            else
                            {
                                amountToBuy = (usdAccount.Available / price);
                            }


                            var quanity = decimal.Round(amountToBuy * .998m, quanityprecision, MidpointRounding.ToZero);
                            if (product.BaseIncrement <= quanity && quanity > product.BaseMinSize)
                            {
                                _logger.LogInformation($"{DateTime.Now} - {_exchangeInstance.AccountName}\nCreating Buy Limit Order: {token.Symbol}\nPrice: {price}");
                                var result = await _exchangeClient.OrdersService.PlaceLimitOrderAsync(CoinbasePro.Services.Orders.Types.OrderSide.Buy, token.Symbol.ConvertSymbolToProductTypeUSD(), quanity, price);
                                if (result.Status != CoinbasePro.Services.Orders.Types.OrderStatus.Rejected)
                                {
                                    string message = $"\n{DateTime.Now} - {_exchangeInstance.AccountName}:  Symbol - {token.Symbol.ToUpper()}\nLimit order buy price is {price.ToString("C8")}";
                                    await _notification.SendNotification(message);
                                }
                                else
                                    await _notification.SendNotification($"{DateTime.Now} - {_exchangeInstance.AccountName}\nSymbol: {token.Symbol} - ORDER REJECTED\nPrice: {price.ToString("C8")}\nSold {quanity}");
                            }
                        }
                    }
                    else
                    {
                        _logger.LogInformation($"{DateTime.Now} - Pair does not exist:  {token.Symbol.ToUpper()}/USD");
                    }
                }
                else
                {
                    await _notification.SendNotification($"{DateTime.Now} - {_exchangeInstance.AccountName}\nSymbol: {token.Symbol} - Could not find in the account list.\nForcast is {TradeAdvice.Buy.ToString()}\nPrice: {strategy.ActiveCandle.CurrentPrice.Value.ToString("C8")}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }

        public async Task SellAsync(StrategyInstance strategy, CryptoToken token)
        {
            try
            {

                var accounts = (await _exchangeClient.AccountsService.GetAllAccountsAsync()).ToList();
                var strategyAccount = accounts.GetAccount(token.Symbol.ToUpper());

                if (strategyAccount != null)
                {

                    var precision = _exchangeClient.ProductsService.GetAllProductsAsync().GetAwaiter().GetResult();

                    var product = precision.Where(w => w.BaseCurrency == token.Symbol.ToUpper() && w.QuoteCurrency == "USD").SingleOrDefault();
                    int priceprecision = BitConverter.GetBytes(decimal.GetBits(product.QuoteIncrement)[3])[2];
                    int quanityprecision = BitConverter.GetBytes(decimal.GetBits(product.BaseIncrement)[3])[2];

                    var price = decimal.Round(strategy.ActiveCandle.CurrentPrice.HasValue ? strategy.ActiveCandle.CurrentPrice.Value : 0.0m * 1.0015M, priceprecision, MidpointRounding.ToZero);
                    var quanity = decimal.Round(strategyAccount.Available, quanityprecision, MidpointRounding.ToZero);
                    if (product.BaseIncrement <= quanity && quanity > product.BaseMinSize)
                    {
                        _logger.LogInformation($"{DateTime.Now} - {_exchangeInstance.AccountName}\nCreating Limit Order: {token.Symbol}\nPrice: {price}");
                        var result = _exchangeClient.OrdersService.PlaceLimitOrderAsync(CoinbasePro.Services.Orders.Types.OrderSide.Sell, token.Symbol.ConvertSymbolToProductTypeUSD(), quanity, price).GetAwaiter().GetResult();
                        if (result.Status != CoinbasePro.Services.Orders.Types.OrderStatus.Rejected)
                        {
                            string message = $"\n{DateTime.Now} - {_exchangeInstance.AccountName}:  Symbol - {token.Symbol.ToUpper()}\nLimit order sell price is {price.ToString("C8")}";
                            await _notification.SendNotification(message);
                        }
                        else
                            await _notification.SendNotification($"{DateTime.Now} - {_exchangeInstance.AccountName}\nSymbol: {token.Symbol} - ORDER REJECTED\nPrice: {price.ToString("C8")}\nSold {quanity}");
                    }
                }
                else
                {
                    await _notification.SendNotification($"{DateTime.Now} - {_exchangeInstance.AccountName}\nSymbol: {token.Symbol} - Could not find in the account list.\nForcast is {strategy.ActiveCandle.TradeAdvice}\nPrice: {strategy.ActiveCandle.CurrentPrice.Value.ToString("C8")}");
                }
            }
            catch (Exception ex)
            {
                await _notification.SendNotification($"{DateTime.Now} - {_exchangeInstance.AccountName}\nSymbol: {token.Symbol} - Error creating sell limit - {ex.ToString()}");
                _logger.LogError(ex.Message, ex);
            }
        }
    }
    public interface INotificationManager
    {
        Task<bool> SendNotification(string message);
        Task<bool> SendTemplatedNotification(string template, params object[] parameters);
    }
    public class MockNotificationManager : INotificationManager
    {
        public async Task<bool> SendNotification(string message)
        {
            return true;
        }

        public async Task<bool> SendTemplatedNotification(string template, params object[] parameters)
        {
            return true;
        }
    }
    public static class AccountExtensions
    {
        public static Account GetAccount(this List<Account> accounts, string symbol)
        {
            var account = accounts.Where(s => s.Currency.ToString().ToUpper() == symbol.ToUpper()).SingleOrDefault();
            return account;
        }

        public static Product GetProduct(this List<Product> products, string symbol)
        {
            var product = products.Where(w => w.BaseCurrency == "usd" && w.QuoteCurrency == symbol).SingleOrDefault();
            return product;
        }

        public static string ConvertSymbolToProductTypeUSD(this string symbol)
        {
            var result = symbol + "-usd";
            return result.ToUpper();
        }
    }
}
