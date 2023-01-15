using CryptoDevilAPI.DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoDevilAPI.Repositories
{
    public class UserDataRepository : IUserDataRepository
    {
        private List<IUserDataDA> _userDataList;
        public UserDataRepository()
        {
        }
        public List<IUserDataDA> UserDataList { set => _userDataList = value; }

        public async Task<Dictionary<string, object>> GetPortfolioDataAsync()
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            decimal sum = 0.0m;

            foreach(var u in _userDataList)
            {
                sum += await u.TotalPortfolioValueAsync();

            }
            response.Add("PortfolioAmount", sum);

            return response;
            
        }
    }
}