using StockMarket.DataContext.DTOs.StockDTOs;
using StockMarket.DataContext.Mapper.ExtensionMethods.CommentMappers;
using StockMarket.Models.StocksModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.DataContext.Mapper.ExtensionMethods.StockMappers
{
    /// <summary>
    /// This class represents the mapper between StockModel and StockDto
    /// </summary>
    public static class StockMapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stockModel"></param>
        /// <returns></returns>
        public static StockDto FromStockModelToStockDto(this StockModel stockModel)
        {
            return new()
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                Industry = stockModel.Industry,
                Purchase = stockModel.Purchase,
                MarketCap = stockModel.MarketCap,
                CompanyName = stockModel.CompanyName,
                LastDividend = stockModel.LastDividend,
                Comments = stockModel.Comments.Select(comment => comment.FromCommentModelToCommentDto()).ToList()
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createStockRequestDto"></param>
        /// <returns></returns>
        public static StockModel FromCreateStockRequestDtoToStockModel(this CreateStockRequestDto createStockRequestDto)
        {
            return new()
            {
                Symbol = createStockRequestDto.Symbol,
                Purchase = createStockRequestDto.Purchase,
                Industry = createStockRequestDto.Industry,
                MarketCap = createStockRequestDto.MarketCap,
                CompanyName = createStockRequestDto.CompanyName,
                LastDividend = createStockRequestDto.LastDividend,
            };
        }
    }
}
