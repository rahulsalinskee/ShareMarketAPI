using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockMarket.DataContext.DTOs.CommentDtos;
using StockMarket.DataContext.Mapper.ExtensionMethods.ClaimExtensions;
using StockMarket.DataContext.Mapper.ExtensionMethods.CommentMappers;
using StockMarket.DataContext.Repository.Interfaces;
using StockMarket.Models.Users;

namespace StockMarket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        #region Private Data Members
        private readonly ICommentRepositoryService _commentRepositoryService;
        private readonly IStockRepositoryServices _stockRepositoryServices;
        private readonly UserManager<UsersModel> _userManager;
        private const string ID_AS_INTEGER = "{id:int}";
        private const string STOCK_ID_AS_INTEGER = "{stockId:int}";
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commentRepositoryService"></param>
        /// <param name="stockRepositoryServices"></param>
        /// <param name="userManager"></param>
        public CommentController(ICommentRepositoryService commentRepositoryService, IStockRepositoryServices stockRepositoryServices, UserManager<UsersModel> userManager)
        {
            this._commentRepositoryService = commentRepositoryService;
            this._stockRepositoryServices = stockRepositoryServices;
            this._userManager = userManager;
        }
        #endregion

        #region Get All Comments
        /// <summary>
        /// Get All Comments
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            if (ModelState.IsValid)
            {
                var comments = await this._commentRepositoryService.GetAllCommentsAsync();
                var commentDto = comments.Select(comment => comment.FromCommentModelToCommentDto());
                return Ok(commentDto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        #endregion

        #region Get Comment By ID
        /// <summary>
        /// Get Comment By ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet(ID_AS_INTEGER)]
        public async Task<IActionResult> GetCommentById(int id)
        {
            if (ModelState.IsValid)
            {
                var comment = await this._commentRepositoryService.GetCommentByIdAsync(id);
                if (comment is not null)
                {
                    var commentDto = comment.FromCommentModelToCommentDto();
                    return Ok(commentDto);
                }
                return NotFound("Comment Not Found!");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        #endregion

        #region Create New Comment
        /// <summary>
        /// Create New Comment
        /// </summary>
        /// <param name="stockId"></param>
        /// <param name="createCommentRequestDto"></param>
        /// <returns></returns>
        [HttpPost(STOCK_ID_AS_INTEGER)]
        public async Task<IActionResult> CreateCommentAsync([FromRoute] int stockId, [FromBody] CreateCommentRequestDto createCommentRequestDto)
        {
            if (ModelState.IsValid)
            {
                if (!await this._stockRepositoryServices.IsStockExist(stockId: stockId))
                {
                    return BadRequest("Stock Not Found!");
                }
                var userName = User.GetUserNameExtension();

                /* Get The User From Database */
                var user = await this._userManager.FindByNameAsync(userName);
                var commentModel = createCommentRequestDto.FromCreateCommentDtoToCommentModel(stockId: stockId);
                commentModel.UserId = user.Id;
                var comment = await this._commentRepositoryService.CreateCommentAsync(comment: commentModel);
                return CreatedAtAction(actionName: nameof(GetCommentById), routeValues: new { id = comment }, value: commentModel.FromCommentModelToCommentDto());
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        #endregion

        #region Delete Comment
        /// <summary>
        /// Delete Comment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete(ID_AS_INTEGER)]
        public async Task<IActionResult> DeleteCommentAsync([FromRoute] int id)
        {
            if (ModelState.IsValid)
            {
                var commentModel = await this._commentRepositoryService.DeleteCommentAsync(id);
                if (commentModel is not null)
                {
                    return Ok("Comment Is Deleted Successfully");
                }
                return NotFound("Comment Not Found!");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        #endregion

        #region Update Comment Async
        /// <summary>
        /// Update Comment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateCommentRequestDto"></param>
        /// <returns></returns>
        [HttpPut(ID_AS_INTEGER)]
        public async Task<IActionResult> UpdateCommentAsync([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateCommentRequestDto)
        {
            if (ModelState.IsValid)
            {
                var commentModel = await this._commentRepositoryService.UpdateCommentAsync(id, updateCommentRequestDto.FromCommentDtoToCommentModel(id));
                if (commentModel is not null)
                {
                    return Ok(commentModel.FromCommentModelToCommentDto());
                }
                return NotFound("Comment Not Found!");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        #endregion
    }
}
