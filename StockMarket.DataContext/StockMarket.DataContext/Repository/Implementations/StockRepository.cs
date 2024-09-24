using Microsoft.EntityFrameworkCore;
using StockMarket.DataContext.DataContext;
using StockMarket.DataContext.DTOs.StockDTOs;
using StockMarket.DataContext.Helpers.Enum;
using StockMarket.DataContext.Helpers.QueryObject;
using StockMarket.DataContext.Repository.Interfaces;
using StockMarket.Models.StocksModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.DataContext.Repository.Implementations
{
    public class StockRepository : IStockRepositoryServices
    {
        #region Private Data Members
        private readonly ApplicationDbContext _applicationDbContext;
        #endregion

        #region Constructor
        public StockRepository(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }
        #endregion

        #region Create Stock Async
        public async Task<StockModel> CreateStockAsync(StockModel stockModel)
        {
            await this._applicationDbContext.TblStocks.AddAsync(stockModel);
            await this._applicationDbContext.SaveChangesAsync();
            return stockModel;
        }
        #endregion

        #region Delete Stock Async
        public async Task<StockModel> DeleteStockAsync(int id)
        {
            var stockModel = await this._applicationDbContext.TblStocks.FirstOrDefaultAsync(stock => stock.Id == id);
            if (stockModel != null)
            {
                this._applicationDbContext.TblStocks.Remove(stockModel);
                await this._applicationDbContext.SaveChangesAsync();
                return stockModel;
            }
            return null;
        }
        #endregion

        #region Get All Stocks Async
        /// <summary>
        /// Get All Stocks
        /// </summary>
        /// <param name="queryObject"></param>
        /// <returns></returns>
        public async Task<List<StockModel>> GetAllStocksAsync(QueryObject queryObject)
        {
            var stocks = this._applicationDbContext.TblStocks.Include(stock => stock.Comments).ThenInclude(user => user.UserId).AsQueryable();

            /* Sort By Company Name */
            if (!string.IsNullOrWhiteSpace(queryObject.CompanyName))
            {
                stocks = FetchFilteredStock(queryObject: queryObject, stocks: stocks, key: QueryObjectEnumerations.CompanyName);
            }

            /* Sort By Symbol */
            if (!string.IsNullOrWhiteSpace(queryObject.Symbol))
            {
                stocks = FetchFilteredStock(queryObject: queryObject, stocks: stocks, key: QueryObjectEnumerations.Symbol);
            }

            /* Adding Filter To Sort The Stocks By Descending Through Symbol Name */
            if (!string.IsNullOrWhiteSpace(queryObject.SortBy) && (queryObject.SortBy.Equals(value: "Symbol", comparisonType: StringComparison.OrdinalIgnoreCase)))
            {
                stocks = FetchFilteredStockInDescendingOrder(queryObject: queryObject, stocks: stocks, key: QueryObjectEnumerations.Symbol);
            }

            /* To get the skip number in order to calculate the Page size to display data in view */
            var skipNumber = (queryObject.PageNumber - 1) * queryObject.PageSize;

            return await stocks.Skip(skipNumber).Take(queryObject.PageSize).AsNoTracking().ToListAsync();
        }
        #endregion

        #region Get Stock By ID
        /// <summary>
        /// Get Stock By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<StockModel?> GetStockByIdAsync(int id)
        {
            return await this._applicationDbContext.TblStocks.Include(stock => stock.Comments).FirstOrDefaultAsync(stock => stock.Id == id);
        }
        #endregion

        #region Get Stocks By Symbol Async
        /// <summary>
        /// Get Stock By Symbol Name
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public async Task<StockModel?> GetStockBySymbolAsync(string symbol)
        {
            return await this._applicationDbContext.TblStocks.FirstOrDefaultAsync(stock => stock.Symbol == symbol);
        } 
        #endregion

        #region Update Stock Async
        /// <summary>
        /// Update Stock
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateStockRequestDto"></param>
        /// <returns></returns>
        public async Task<StockModel> UpdateStockAsync(int id, UpdateExistingStockRequestDto updateStockRequestDto)
        {
            var existingStock = await this._applicationDbContext.TblStocks.FirstOrDefaultAsync(stock => stock.Id == id);
            if (existingStock != null)
            {
                existingStock.Industry = updateStockRequestDto.Industry;
                existingStock.Purchase = updateStockRequestDto.Purchase;
                existingStock.Symbol = updateStockRequestDto.Symbol;
                existingStock.MarketCap = updateStockRequestDto.MarketCap;
                existingStock.CompanyName = updateStockRequestDto.CompanyName;
                existingStock.LastDividend = updateStockRequestDto.LastDividend;

                await this._applicationDbContext.SaveChangesAsync();
                return existingStock;
            }
            return null;
        }
        #endregion

        #region Is Stock Exist
        /// <summary>
        /// Is Stock Exist
        /// </summary>
        /// <param name="stockId"></param>
        /// <returns></returns>
        public async Task<bool> IsStockExist(int stockId)
        {
            return await this._applicationDbContext.TblStocks.AnyAsync(stock => stock.Id == stockId);
        }
        #endregion

        #region Private Static Method
        /// <summary>
        /// Fetch Stock To Filter
        /// </summary>
        /// <param name="queryObject"></param>
        /// <param name="stocks"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static IQueryable<StockModel> FetchFilteredStock(QueryObject queryObject, IQueryable<StockModel> stocks, QueryObjectEnumerations key)
        {
            switch (key)
            {
                case QueryObjectEnumerations.CompanyName:
                    stocks = stocks.Where(stock => stock.CompanyName.Contains(queryObject.CompanyName));
                    break;
                case QueryObjectEnumerations.Symbol:
                    stocks = stocks.Where(stock => stock.Symbol.Contains(queryObject.Symbol));
                    break;
                default:
                    break;
            }
            return stocks;
        }

        /// <summary>
        /// Fetch Stock To Filter In Descending Order
        /// </summary>
        /// <param name="queryObject"></param>
        /// <param name="stocks"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static IQueryable<StockModel> FetchFilteredStockInDescendingOrder(QueryObject queryObject, IQueryable<StockModel> stocks, QueryObjectEnumerations key)
        {
            switch (key)
            {
                case QueryObjectEnumerations.Symbol:
                    stocks = queryObject.IsDescending ? stocks.OrderByDescending(stock => stock.Symbol) : stocks.OrderBy(stock => stock.Symbol);
                    break;
                default:
                    break;
            }
            return stocks;
        }
        #endregion
    }
}
