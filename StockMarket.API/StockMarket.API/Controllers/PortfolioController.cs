using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockMarket.DataContext.Mapper.ExtensionMethods.ClaimExtensions;
using StockMarket.DataContext.Repository.Interfaces;
using StockMarket.Models.StocksModel;
using StockMarket.Models.Users;

namespace StockMarket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        #region Private Fields
        private readonly UserManager<UsersModel> _userManager;
        private readonly IStockRepositoryServices _stockRepositoryServices;
        private readonly IPortfolioRepositoryServices _portfolioRepositoryServices; 
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="stockRepositoryServices"></param>
        /// <param name="portfolioRepositoryServices"></param>
        public PortfolioController(UserManager<UsersModel> userManager, IStockRepositoryServices stockRepositoryServices, IPortfolioRepositoryServices portfolioRepositoryServices)
        {
            this._userManager = userManager;
            this._stockRepositoryServices = stockRepositoryServices;
            this._portfolioRepositoryServices = portfolioRepositoryServices;
        }
        #endregion

        #region Get User Portfolio
        /// <summary>
        /// Get User Portfolio
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var userName = GetUserName();
            var applicationUserModel = await this._userManager.FindByNameAsync(userName);
            var portfolio = await this._portfolioRepositoryServices.GetPortfolioAsync(usersModel: applicationUserModel);
            return Ok(portfolio);
        }
        #endregion

        #region Add Portfolio Async
        /// <summary>
        /// Add Portfolio to the User
        /// </summary>
        /// <param name="stockSymbol"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolioAsync(string stockSymbol)
        {
            var userName = GetUserName();
            var user = await this._userManager.FindByNameAsync(userName);
            var stock = await this._stockRepositoryServices.GetStockBySymbolAsync(stockSymbol: stockSymbol);
            if (stock is null)
            {
                return BadRequest("Stock Not Found!");
            }
            else
            {
                /* Get the user portfolio using user model */
                var userPortfolio = await this._portfolioRepositoryServices.GetPortfolioAsync(usersModel: user);

                /* Check if the Portfolio already exists using stock symbol */
                if (userPortfolio.Any(stock => stock.Symbol.Equals(stockSymbol, StringComparison.CurrentCultureIgnoreCase)))
                {
                    return BadRequest("Stock Already Exists!");
                }

                /* Create New Portfolio using user ID and stock symbol */
                var portfolioModel = new PortfolioModel()
                {
                    StockId = stock.Id,
                    UserId = user.Id,
                };

                /* Doing Null Check for Portfolio Model */
                if (portfolioModel is not null)
                {
                    var newlyCreatedPortfolio = await this._portfolioRepositoryServices.CreatePortfolioAsync(portfolioModel);
                    return Ok(newlyCreatedPortfolio);
                }
                return StatusCode(500, "Portfolio Could Not Created!");
            }
        }
        #endregion

        #region Delete Portfolio Async
        /// <summary>
        /// Delete the portfolio
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolioAsync(string stockSymbol)
        {
            var userName = GetUserName();
            var userModel = await this._userManager.FindByNameAsync(userName);
            if (userModel is not null)
            {
                var userPortfolio = await this._portfolioRepositoryServices.GetPortfolioAsync(usersModel: userModel);

                var filteredStock = userPortfolio.Where(stock => stock.Symbol.Equals(stockSymbol, StringComparison.CurrentCultureIgnoreCase)).ToList();

                if (filteredStock.Count == 1)
                {
                    await this._portfolioRepositoryServices.DeletePortfolioAsync(userModel: userModel, stockSymbol: stockSymbol);
                    return Ok("Portfolio Deleted Successfully!");
                }
                else
                {
                    return BadRequest("Stock Not Found!");
                }
            }
            return BadRequest("User Does Not Exist!");
        } 
        #endregion

        #region Private Methods
        /// <summary>
        /// Get User Name
        /// </summary>
        /// <returns></returns>
        private string GetUserName()
        {
            return User.GetUserNameExtension();
        } 
        #endregion
    }
}
