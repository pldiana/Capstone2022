using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoDevilAPI.DataAccess
{
    public interface ICandleDataDA
    {
        Task<List<Candle>> GetCandleDataAsync();
        Task InsertOneAsync(Candle candle);
    }
}
