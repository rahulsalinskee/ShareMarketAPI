using Microsoft.EntityFrameworkCore;
using StockMarket.DataContext.DataContext;
using StockMarket.DataContext.Repository.Interfaces;
using StockMarket.Models.StocksModel;
using StockMarket.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.DataContext.Repository.Implementations
{
    public class PortfolioRepository : IPortfolioRepositoryServices
    {
        #region Private Variables
        private readonly ApplicationDbContext _applicationDbContext;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for PortfolioRepository
        /// </summary>
        /// <param name="applicationDbContext"></param>
        public PortfolioRepository(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }
        #endregion

        #region Get Portfolio Async
        /// <summary>
        /// Get Portfolio by name
        /// </summary>
        /// <param name="usersModel"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<StockModel>> GetPortfolioAsync(UsersModel usersModel)
        {
            return await this._applicationDbContext.TblPortfolio.Where(user => user.UserId == usersModel.Id).Select(stock => new StockModel()
            {
                Id = stock.StockId,
                CompanyName = stock.StockModel.CompanyName,
                Symbol = stock.StockModel.Symbol,
                MarketCap = stock.StockModel.MarketCap,
                LastDividend = stock.StockModel.LastDividend,
                Purchase = stock.StockModel.Purchase,
                Industry = stock.StockModel.Industry,
            }).ToListAsync();
        }
        #endregion

        #region Create Portfolio Async
        /// <summary>
        /// Create Portfolio from PortfolioModel
        /// </summary>
        /// <param name="portfolioModel"></param>
        /// <returns></returns>
        public async Task<PortfolioModel> CreatePortfolioAsync(PortfolioModel portfolioModel)
        {
            await this._applicationDbContext.TblPortfolio.AddAsync(portfolioModel);
            await this._applicationDbContext.SaveChangesAsync();
            return portfolioModel;
        }
        #endregion

        #region Delete Portfolio Async
        /// <summary>
        /// Delete Portfolio
        /// </summary>
        /// <param name="stockSymbol"></param>
        /// <returns></returns>
        public async Task<PortfolioModel> DeletePortfolioAsync(UsersModel userModel, string stockSymbol)
        {
            var portfolioModel = await this._applicationDbContext.TblPortfolio.FirstOrDefaultAsync(user => user.UserId == userModel.Id && user.StockModel.Symbol.Equals(stockSymbol, StringComparison.CurrentCultureIgnoreCase));

            if (portfolioModel != null)
            {
                this._applicationDbContext.TblPortfolio.Remove(portfolioModel);
                await this._applicationDbContext.SaveChangesAsync();
                return portfolioModel;
            }
            return null;
        } 
        #endregion
    }
}
