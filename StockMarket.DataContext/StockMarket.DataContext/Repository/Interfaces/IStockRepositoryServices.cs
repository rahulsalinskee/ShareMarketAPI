using StockMarket.DataContext.DTOs.StockDTOs;
using StockMarket.DataContext.Helpers.QueryObject;
using StockMarket.Models.StocksModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.DataContext.Repository.Interfaces
{
    public interface IStockRepositoryServices
    {
        public Task<List<StockModel>> GetAllStocksAsync(QueryObject queryObject);

        public Task<StockModel?> GetStockByIdAsync(int id);

        public Task<StockModel> GetStockBySymbolAsync(string stockSymbol);

        public Task<StockModel> CreateStockAsync(StockModel stockModel);

        public Task<StockModel> UpdateStockAsync(int id, UpdateExistingStockRequestDto stockModel);

        public Task<StockModel> DeleteStockAsync(int id);

        public Task<bool> IsStockExist(int stockId);
    }
}
