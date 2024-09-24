using StockMarket.Models.StocksModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.DataContext.Repository.Interfaces
{
    public interface ICommentRepositoryService
    {
        public Task<List<CommentModel>> GetAllCommentsAsync();

        public Task<CommentModel?> GetCommentByIdAsync(int id);

        public Task<CommentModel> CreateCommentAsync(CommentModel comment);

        public Task<CommentModel> DeleteCommentAsync(int id);

        public Task<CommentModel> UpdateCommentAsync(int id, CommentModel comment);
    }
}
