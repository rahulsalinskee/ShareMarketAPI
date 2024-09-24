using Microsoft.EntityFrameworkCore;
using StockMarket.DataContext.DataContext;
using StockMarket.DataContext.Mapper;
using StockMarket.DataContext.Repository.Interfaces;
using StockMarket.Models.StocksModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.DataContext.Repository.Implementations
{
    public class CommentRepository : ICommentRepositoryService
    {
        #region Private Data Members
        private readonly ApplicationDbContext _applicationDbContext;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="applicationDbContext"></param>
        public CommentRepository(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }
        #endregion

        #region Create Comment Async
        /// <summary>
        /// Create Comment
        /// </summary>
        /// <param name="commentModel"></param>
        /// <returns></returns>
        public async Task<CommentModel> CreateCommentAsync(CommentModel commentModel)
        {
            await this._applicationDbContext.TblComments.AddAsync(commentModel);
            await this._applicationDbContext.SaveChangesAsync();
            return commentModel;
        }
        #endregion

        #region Delete Comment Async
        /// <summary>
        /// Delete Comment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CommentModel> DeleteCommentAsync(int id)
        {
            var commentModel = await this._applicationDbContext.TblComments.FirstOrDefaultAsync(comment => comment.ID == id);
            if (commentModel != null)
            {
                this._applicationDbContext.TblComments.Remove(commentModel);
                await this._applicationDbContext.SaveChangesAsync();
            }
            return commentModel;
        }
        #endregion

        #region Get All Comment Async
        /// <summary>
        /// Get All Comments
        /// </summary>
        /// <returns></returns>
        public async Task<List<CommentModel>> GetAllCommentsAsync()
        {
            return await this._applicationDbContext.TblComments.Include(user => user.UserModel).AsNoTracking().ToListAsync();
        }
        #endregion

        #region Get Comment By ID Async
        /// <summary>
        /// Get Comment By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CommentModel?> GetCommentByIdAsync(int id)
        {
            var fetchedCommentById = await this._applicationDbContext.TblComments.Include(user => user.UserModel).FirstOrDefaultAsync(comment => comment.ID == id);
            if (fetchedCommentById != null)
            {
                return fetchedCommentById;
            }
            return null;
        }
        #endregion

        #region Update Comment Async
        /// <summary>
        /// Update Comment Async
        /// </summary>
        /// <param name="id"></param>
        /// <param name="commentModel"></param>
        /// <returns></returns>
        public async Task<CommentModel> UpdateCommentAsync(int id, CommentModel commentModel)
        {
            var existingComment = await this._applicationDbContext.TblComments.FindAsync(id);
            if (existingComment is not null)
            {
                existingComment.Title = commentModel.Title;
                existingComment.Content = commentModel.Content;
                await this._applicationDbContext.SaveChangesAsync();
                return existingComment;
            }
            return null;
        }
        #endregion
    }
}
