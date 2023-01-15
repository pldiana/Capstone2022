using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Models.CryptoWatch
{

    /// <summary>
    /// Documentation can be found here https://cryptowat.ch/docs/api
    /// </summary>
    public class OHLCAPI
    {
        /// <summary>
        /// Allowance is updated with every call.
        /// </summary>
        public static Allowance allowance = null;

        ///<summary>
        ///You can always request this to query your allowance without any extra result - this request costs very little.
        /// </summary>

        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="JsonReaderException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ProtocolViolationException"></exception>
        /// <exception cref="WebException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public static SiteInformation GetSiteInformation()
        {
            try
            {
                return Deserialize<SiteInformation>(GetJObject("https://api.cryptowat.ch"));
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        ///Returns all assets(in no particular order)
        /// </summary>
        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="JsonReaderException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ProtocolViolationException"></exception>
        /// <exception cref="WebException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public static List<Assets> GetAssets()
        {
            try
            {
                return DeserializeToList<Assets>(GetJObject("https://api.cryptowat.ch/assets"));
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        ///Returns a single asset. Lists all markets which have this asset as a base or quote.
        /// </summary>
        /// <param name="route"> Asset specific url, e.g. https://api.cryptowat.ch/assets/btc </param>
        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="JsonReaderException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ProtocolViolationException"></exception>
        /// <exception cref="WebException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public static Asset GetAsset(string route)
        {
            try
            {
                return Deserialize<Asset>(GetJObject(route));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns all pairs (in no particular order).
        /// </summary>
        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="JsonReaderException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ProtocolViolationException"></exception>
        /// <exception cref="WebException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public static List<Pairs> GetPairs()
        {
            try
            {
                return DeserializeToList<Pairs>(GetJObject("https://api.cryptowat.ch/pairs"));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///	Returns a single pair. Lists all markets for this pair.
        /// </summary>
        /// <param name="route"> Pair specific url, e.g. https://api.cryptowat.ch/pairs/ethbtc </param>
        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="JsonReaderException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ProtocolViolationException"></exception>
        /// <exception cref="WebException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public static Pair GetPair(string route)
        {
            try
            {
                return Deserialize<Pair>(GetJObject(route));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns a list of all supported exchanges.
        /// </summary>
        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="JsonReaderException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ProtocolViolationException"></exception>
        /// <exception cref="WebException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public static List<Exchanges> GetExchanges()
        {
            try
            {
                return DeserializeToList<Exchanges>(GetJObject("https://api.cryptowat.ch/exchanges"));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///	Returns a single exchange, with associated routes.
        /// </summary>
        /// <param name="route"> Exchange specific url, e.g. https://api.cryptowat.ch/exchanges/kraken </param> 
        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="JsonReaderException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ProtocolViolationException"></exception>
        /// <exception cref="WebException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public static Exchange GetExchange(string route)
        {
            try
            {
                return Deserialize<Exchange>(GetJObject(route));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns a list of all supported markets.
        /// </summary>
        /// <param name="route">You can also get the supported markets for only a specific exchange. e.g. https://api.cryptowat.ch/markets/kraken </param>
        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="JsonReaderException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ProtocolViolationException"></exception>
        /// <exception cref="WebException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public static List<Markets> GetMarkets(string route = "https://api.cryptowat.ch/markets")
        {
            try
            {
                return DeserializeToList<Markets>(GetJObject(route));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///	Returns a single market, with associated routes.
        /// </summary>
        /// <param name="route"> Market specific url, e.g. https://api.cryptowat.ch/markets/gdax/btcusd </param> 
        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="JsonReaderException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ProtocolViolationException"></exception>
        /// <exception cref="WebException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public static Market GetMarket(string route)
        {
            try
            {
                return Deserialize<Market>(GetJObject(route));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns the current price for all supported markets. Some values may be out of date by a few seconds. 
        /// <para>
        /// key = exchangeName:pairName
        /// </para>
        /// </summary>
        /// <returns> dictionsry</returns>
        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="JsonReaderException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ProtocolViolationException"></exception>
        /// <exception cref="WebException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public static Dictionary<string, double> GetPrices()
        {
            try
            {
                return Deserialize<Dictionary<string, double>>(GetJObject("https://api.cryptowat.ch/markets/prices"));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///	Returns a market’s last price.
        /// </summary>
        /// <param name="route"> Price specific url, e.g. https://api.cryptowat.ch/markets/gdax/btcusd/price </param> 
        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="JsonReaderException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ProtocolViolationException"></exception>
        /// <exception cref="WebException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public static double GetPrice(string route, string apiKey = null)
        {
            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                route += "?apikey=YGMJLKIA4TLB42I3NEL5";
            }
            try
            {
                return Deserialize<Price>(GetJObject(route)).price;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns the market summary for all supported markets. Some values may be out of date by a few seconds.
        /// <para>
        /// key = exchangeName:pairName
        /// </para>
        /// </summary>
        /// <returns> dictionsry</returns>
        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="JsonReaderException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ProtocolViolationException"></exception>
        /// <exception cref="WebException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public static Dictionary<string, Summary> GetSummaries()
        {
            try
            {
                return Deserialize<Dictionary<string, Summary>>(GetJObject("https://api.cryptowat.ch/markets/summaries"));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns a market’s last price as well as other stats based on a 24-hour sliding window: High price, Low price, % change, Absolute change, Volume
        /// </summary>
        /// <param name="route"> Summary specific url, e.g. https://api.cryptowat.ch/markets/gdax/btcusd/summary </param> 
        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="JsonReaderException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ProtocolViolationException"></exception>
        /// <exception cref="WebException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public static Summary GetSummary(string route)
        {
            try
            {
                return Deserialize<Summary>(GetJObject(route));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns a market’s most recent trades, incrementing chronologically. Note some exchanges don’t provide IDs for public trades.
        /// </summary>
        /// <param name="route"> Trade specific url, e.g. https://api.cryptowat.ch/markets/gdax/btcusd/trades </param> 
        /// <param name="limit"> Limit amount of trades returned. If 0 returns all.</param> 
        /// <param name="since"> Only return trades at or after this time. </param> 
        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="JsonReaderException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ProtocolViolationException"></exception>
        /// <exception cref="WebException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public static List<Trade> GetTrades(string route, int limit = 50, long since = -1)
        {
            if (limit != 50 && since != -1)
            {
                route += "?limit=" + limit.ToString() + "&since=" + since.ToString();
            }
            else if (since != -1)
            {
                route += "?since=" + since.ToString();
            }
            else if (limit != 50)
            {
                route += "?limit=" + limit.ToString();
            }
            try
            {
                List<double[]> tradeList = DeserializeToList<double[]>(GetJObject(route));
                List<Trade> trades = new List<Trade>();
                foreach (var t in tradeList)
                {
                    trades.Add(new Trade((int)t[0], (long)t[1], t[2], t[3]));
                }
                return trades;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns a market’s order book.
        /// </summary>
        /// <param name="route"> OrderNook specific url, e.g. https://api.cryptowat.ch/markets/gdax/btcusd/orderbook </param> 
        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="JsonReaderException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ProtocolViolationException"></exception>
        /// <exception cref="WebException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public static OrderBook GetOrderBook(string route)
        {
            try
            {
                _OrderBook _orderBook = Deserialize<_OrderBook>(GetJObject(route));
                OrderBook orderBook = new OrderBook();
                foreach (var bid in _orderBook.bids)
                {
                    orderBook.bids.Add(new Order(bid[0], bid[1]));
                }
                foreach (var ask in _orderBook.asks)
                {
                    orderBook.asks.Add(new Order(ask[0], ask[1]));
                }
                return orderBook;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns a market’s OHLC candlestick data.
        /// </summary>
        /// <param name="route"> Candlestick specific url, e.g. https://api.cryptowat.ch/markets/gdax/btcusd/ohlc </param> 
        /// <param name="timeFrame"> Candlestick timeframe.</param> 
        /// <param name="after"> Only return candles opening after this time. If set to -1 max limit is 6000, otherwise it's 500.</param> 
        /// <param name="before"> Only return candles opening before this time. </param> 
        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="JsonReaderException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ProtocolViolationException"></exception>
        /// <exception cref="WebException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public static List<Candle> GetCandlesticks(string route, string symbol, TimeFrame timeFrame, long after = -2, long before = 0, string apiKey = null)
        {
            route += "?periods=" + ((int)timeFrame).ToString();
            if (after != -2)
            {
                route += "&after=" + after.ToString();
            }
            if (before != 0)
            {
                route += "&before=" + before.ToString();
            }
            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                route += "&apikey=YGMJLKIA4TLB42I3NEL5";
            }
            try
            {
                _Candlestick _candlestick = Deserialize<_Candlestick>(GetJObject(route));
                List<Candle> candles = new List<Candle>();
                foreach (var c in _candlestick.allCandlesticks)
                {
                    var tempCandle = new Candlestick((long)c[0], c[1], c[2], c[3], c[4], c[5]);

                    DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    DateTime date = start.AddSeconds(tempCandle.closeTime).ToLocalTime();

                    candles.Add(new Candle
                    {
                        High = (decimal)tempCandle.highPrice,
                        Low = (decimal)tempCandle.lowPrice,
                        Close = (decimal)tempCandle.closePrice,
                        Open = (decimal)tempCandle.openPrice,
                        Timestamp = date,
                        Volume = (decimal)tempCandle.volume,
                        Symbol = symbol,
                        DateTime = DateTime.UtcNow
                    });
                }
                return candles;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns a market’s OHLC candlestick data.
        /// </summary>
        /// <param name="route"> Candlestick specific url, e.g. https://api.cryptowat.ch/markets/gdax/btcusd/ohlc </param> 
        /// <param name="timeFrame"> Candlestick timeframe.</param> 
        /// <param name="after"> Only return candles opening after this time. If set to -1 max limit is 6000, otherwise it's 500.</param> 
        /// <param name="before"> Only return candles opening before this time. </param> 
        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="JsonReaderException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ProtocolViolationException"></exception>
        /// <exception cref="WebException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public static List<Candle> GetCandlesticks(string route, string pair)
        {
            
            try
            {
                decimal[][] _candlestick = Deserialize<decimal[][]>(GetJObject(route), pair);
                List<Candle> candles = new List<Candle>();
                foreach (var c in _candlestick)
                {
                    var tempCandle = new Candlestick((long)c[0], c[1], c[2], c[3], c[4], c[6]);

                    DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    DateTime date = start.AddSeconds(tempCandle.closeTime).ToLocalTime();

                    candles.Add(new Candle
                    {
                        High = (decimal)tempCandle.highPrice,
                        Low = (decimal)tempCandle.lowPrice,
                        Close = (decimal)tempCandle.closePrice,
                        Open = (decimal)tempCandle.openPrice,
                        Timestamp = date,
                        Volume = (decimal)tempCandle.volume,
                        Symbol = pair,
                        DateTime = DateTime.UtcNow
                    });
                }
                return candles;
            }
            catch(Exception e)
            {
                throw;
            }
        }

        //?pair={coin.ToUpper()}USD&interval=15&since={DateTime.UtcNow.AddDays(-2)}",

        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ProtocolViolationException"></exception>
        /// <exception cref="WebException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        private static Stream GetResponseStream(string url)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Accept = "*/*";
                httpWebRequest.Method = "GET";
                //httpWebRequest.Headers.Add("Authorization", "Basic reallylongstring");
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                return httpResponse.GetResponseStream();
            }
            catch
            {
                throw;
            }

        }


        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="JsonReaderException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ProtocolViolationException"></exception>
        /// <exception cref="WebException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        private static JObject GetJObject(string url)
        {
            try
            {
                StreamReader sr = new StreamReader(GetResponseStream(url));
                string response = sr.ReadToEnd();
                JObject jObject = JObject.Parse(response);
                sr.Close();
                return jObject;
            }
            catch
            {
                throw;
            }
        }


        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="JsonReaderException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ProtocolViolationException"></exception>
        /// <exception cref="WebException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        private static List<T> DeserializeToList<T>(JObject jObject)
        {
            try
            {
                // if we get any other error from cryptowatch
                foreach (var responseType in jObject)
                {
                    if (responseType.Key == "error")
                        throw new Exception("Cryptowatch error: " + responseType.Value.ToObject<string>());
                }
                // get JSON result objects into a list
                List<JToken> jTokens = jObject["result"].Children().ToList();
                allowance = jObject["allowance"].ToObject<Allowance>();

                // serialize JSON results into .NET objects
                List<T> objects = new List<T>();
                foreach (JToken jToken in jTokens)
                {
                    // JToken.ToObject is a helper method that uses JsonSerializer internally
                    objects.Add(jToken.ToObject<T>());
                }
                return objects;
            }
            catch
            {
                throw;
            }

        }


        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="JsonReaderException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ProtocolViolationException"></exception>
        /// <exception cref="WebException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        private static T Deserialize<T>(JObject jObject)
        {
            try
            {
                // if we get any other error from cryptowatch
                foreach (var responseType in jObject)
                {
                    if (responseType.Key == "error" && responseType.Value != null && (responseType.Value as JArray).HasValues)
                        throw new Exception("Cryptowatch error: " + responseType.Value.ToObject<string>());
                }
                //allowance = jObject["allowance"].ToObject<Allowance>();
                return jObject["result"].ToObject<T>();

               // jObject["result"]["LINKUSD"].Children().ToList()[0][0].ToObject<int>()
            }
            catch
            {
                throw;
            }
        }

        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="JsonReaderException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ProtocolViolationException"></exception>
        /// <exception cref="WebException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        private static T Deserialize<T>(JObject jObject, string pair)
        {
            try
            {
                // if we get any other error from cryptowatch
                foreach (var responseType in jObject)
                {
                    if (responseType.Key == "error" && responseType.Value != null && (responseType.Value as JArray).HasValues)
                        throw new Exception("Cryptowatch error: " + responseType.Value.ToObject<string>());
                }
                //allowance = jObject["allowance"].ToObject<Allowance>();
                return jObject["result"].First.First.ToObject<T>();

                // jObject["result"]["LINKUSD"].Children().ToList()[0][0].ToObject<int>()
            }
            catch
            {
                throw;
            }
        }

        private class _OrderBook
        {
            public double[][] asks { get; set; }
            public double[][] bids { get; set; }
        }

        private class _Candlestick
        {
            public decimal[][] allCandlesticks { get; set; }
            [JsonProperty("60")]
            private decimal[][] min { set { allCandlesticks = value; } }
            [JsonProperty("180")]
            private decimal[][] _180 { set { allCandlesticks = value; } }
            [JsonProperty("300")]
            private decimal[][] _300 { set { allCandlesticks = value; } }
            [JsonProperty("900")]
            private decimal[][] _900 { set { allCandlesticks = value; } }
            [JsonProperty("1800")]
            private decimal[][] _1800 { set { allCandlesticks = value; } }
            [JsonProperty("3600")]
            private decimal[][] _3600 { set { allCandlesticks = value; } }
            [JsonProperty("7200")]
            private decimal[][] _7200 { set { allCandlesticks = value; } }
            [JsonProperty("14400")]
            private decimal[][] _14400 { set { allCandlesticks = value; } }
            [JsonProperty("21600")]
            private decimal[][] _21600 { set { allCandlesticks = value; } }
            [JsonProperty("43200")]
            private decimal[][] _43200 { set { allCandlesticks = value; } }
            [JsonProperty("86400")]
            private decimal[][] _86400 { set { allCandlesticks = value; } }
            [JsonProperty("259200")]
            private decimal[][] _259200 { set { allCandlesticks = value; } }
            [JsonProperty("604800")]
            private decimal[][] _604800 { set { allCandlesticks = value; } }
        }

        private class Price
        {
            public double price { get; set; }
        }

    }
    public class SiteInformation
    {
        public string revision { get; set; }
        public string uptime { get; set; }
        public string documentation { get; set; }
        public string[] indexes { get; set; }
    }

    /// <summary>
    /// An asset can be a crypto or fiat currency. 
    /// </summary>
    public class Assets
    {
        public int id { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public bool fiat { get; set; }
        public string route { get; set; }
    }

    /// <summary>
    /// An asset can be a crypto or fiat currency. 
    /// </summary>
    public class Asset
    {
        public int id { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public bool fiat { get; set; }
        public Markets1 markets { get; set; }
    }

    /// <summary>
    /// A pair of assets. Each pair has a base and a quote. For example, btceur has base btc and quote eur.
    /// </summary>
    public class Pairs
    {
        public string symbol { get; set; }
        public int id { get; set; }
        [JsonProperty("base")]
        public Base basePair { get; set; }
        [JsonProperty("quote")]
        public Quote quotePair { get; set; }
        public string route { get; set; }
        /// <summary>
        /// Not always set.
        /// </summary>
        public string futuresContractPeriod { get; set; }
    }

    public class Pair
    {
        public string symbol { get; set; }
        public int id { get; set; }
        [JsonProperty("base")]
        public Base basePair { get; set; }
        [JsonProperty("quote")]
        public Quote quotePair { get; set; }
        public string route { get; set; }
        public Markets[] markets { get; set; }
    }

    /// <summary>
    /// Exchanges are where all the action happens!
    /// </summary>
    public class Exchanges
    {
        public string symbol { get; set; }
        public string name { get; set; }
        public string route { get; set; }
        public bool active { get; set; }
    }

    /// <summary>
    /// Exchanges are where all the action happens!
    /// </summary>
    public class Exchange
    {
        public string symbol { get; set; }
        public string name { get; set; }
        public bool active { get; set; }
        public Route routes { get; set; }
    }

    /// <summary>
    /// A market is a pair listed on an exchange. For example, pair btceur on exchange kraken is a market.
    /// </summary>
    public class Markets
    {
        public int id { get; set; }
        public string exchange { get; set; }
        public string pair { get; set; }
        public bool active { get; set; }
        public string route { get; set; }
    }

    /// <summary>
    /// A market is a pair listed on an exchange. For example, pair btceur on exchange kraken is a market.
    /// </summary>
    public class Market
    {
        public string exchange { get; set; }
        public string pair { get; set; }
        public bool active { get; set; }
        public Routes routes { get; set; }
    }

    public class Markets1
    {
        [JsonProperty("base")]
        public Bases[] baseMarket { get; set; }
        [JsonProperty("quote")]
        public Quotes[] quoteMarket { get; set; }
    }

    public class Bases
    {
        public int id { get; set; }
        public string exchange { get; set; }
        public string pair { get; set; }
        public bool active { get; set; }
        public string route { get; set; }
    }

    public class Base
    {
        public int id { get; set; }
        public string route { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public bool fiat { get; set; }
    }

    public class Quotes
    {
        public int id { get; set; }
        public string exchange { get; set; }
        public string pair { get; set; }
        public bool active { get; set; }
        public string route { get; set; }
    }

    public class Quote
    {
        public int id { get; set; }
        public string route { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public bool fiat { get; set; }
    }

    public class Routes
    {
        public string price { get; set; }
        public string summary { get; set; }
        public string orderbook { get; set; }
        public string trades { get; set; }
        public string ohlc { get; set; }
    }

    public class Route
    {
        public string markets { get; set; }
    }

    public class Summary
    {
        public Price price { get; set; }
        public double volume { get; set; }
    }

    public class Price
    {
        public double last { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public Change change { get; set; }
    }

    public class Change
    {
        public double percentage { get; set; }
        public double absolute { get; set; }
    }

    public class Trade
    {
        public int id { get; set; }
        public long timestamp { get; set; }
        public double price { get; set; }
        public double amount { get; set; }

        public Trade(int id, long timestamp, double price, double amount)
        {
            this.id = id;
            this.timestamp = timestamp;
            this.price = price;
            this.amount = amount;
        }
    }

    public class OrderBook
    {
        public List<Order> bids { get; set; }
        public List<Order> asks { get; set; }

        public OrderBook()
        {
            bids = new List<Order>();
            asks = new List<Order>();
        }
    }

    public class Order
    {
        public double price;
        public double amount;

        public Order(double price, double amount)
        {
            this.price = price;
            this.amount = amount;
        }
    }
    public class Candlestick
    {
        public long closeTime;
        public decimal openPrice;
        public decimal highPrice;
        public decimal lowPrice;
        public decimal closePrice;
        public decimal volume;

        public Candlestick(long closeTime, decimal openPrice, decimal highPrice, decimal lowPrice, decimal closePrice, decimal volume)
        {
            this.closeTime = closeTime;
            this.openPrice = openPrice;
            this.highPrice = highPrice;
            this.lowPrice = lowPrice;
            this.closePrice = closePrice;
            this.volume = volume;
        }
    }

    public class Allowance
    {
        public long cost { get; set; }
        public long remaining { get; set; }
    }
}
