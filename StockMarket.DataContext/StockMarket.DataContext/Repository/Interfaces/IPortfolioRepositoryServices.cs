using StockMarket.Models.StocksModel;
using StockMarket.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.DataContext.Repository.Interfaces
{
    public interface IPortfolioRepositoryServices
    {
        public Task<List<StockModel>> GetPortfolioAsync(UsersModel usersModel);
        public Task<PortfolioModel> CreatePortfolioAsync(PortfolioModel portfolioModel);
        public Task<PortfolioModel> DeletePortfolioAsync(UsersModel userModel, string stockSymbol);
    }
}
