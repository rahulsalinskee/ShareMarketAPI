using StockMarket.DataContext.DTOs.CommentDtos;
using StockMarket.Models.StocksModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.DataContext.Mapper.ExtensionMethods.CommentMappers
{
    public static class CommentMapper
    {
        /// <summary>
        /// Extension Method For Comments
        /// This method maps CommentModel to CommentDto
        /// </summary>
        /// <param name="commentModel"></param>
        /// <returns></returns>
        public static CommentDto FromCommentModelToCommentDto(this CommentModel commentModel)
        {
            return new()
            {
                ID = commentModel.ID,
                Title = commentModel.Title,
                StockId = commentModel.StockId,
                Content = commentModel.Content,
                CreatedOn = commentModel.CreatedOn,
                CreatedBy = commentModel.UserModel.UserName
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentDto"></param>
        /// <returns></returns>
        public static CommentModel FromCreateCommentDtoToCommentModel(this CreateCommentRequestDto commentDto, int stockId)
        {
            return new CommentModel()
            {
                StockId = stockId,
                Title = commentDto.Title,
                Content = commentDto.Content,
            };
        }

        /// <summary>
        /// This is for Update
        /// </summary>
        /// <param name="commentDto"></param>
        /// <param name="stockId"></param>
        /// <returns></returns>
        public static CommentModel FromCommentDtoToCommentModel(this UpdateCommentRequestDto commentDto, int stockId)
        {
            return new CommentModel()
            {
                StockId = stockId,
                Title = commentDto.Title,
                Content = commentDto.Content,
            };
        }
    }
}
