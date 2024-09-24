using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockMarket.DataContext.DTOs.StockDTOs;
using StockMarket.DataContext.Helpers.QueryObject;
using StockMarket.DataContext.Mapper.ExtensionMethods.StockMappers;
using StockMarket.DataContext.Repository.Interfaces;

namespace StockMarket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        #region Private Data Members
        private readonly IStockRepositoryServices _stockRepositoryServices;
        private const string ID_AS_INTEGER = "{id:int}";
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the StockController
        /// </summary>
        /// <param name="stockRepositoryServices"></param>
        public StockController(IStockRepositoryServices stockRepositoryServices)
        {
            this._stockRepositoryServices = stockRepositoryServices;
        }
        #endregion

        #region Get All Stock Async
        /// <summary>
        /// Get All Stocks
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllStocksAsync([FromQuery] QueryObject query)
        {
            if (ModelState.IsValid)
            {
                var stocks = await this._stockRepositoryServices.GetAllStocksAsync(queryObject: query);
                var stockDto = stocks.Select(item => item.FromStockModelToStockDto()).ToList();
                return Ok(stockDto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        #endregion

        #region Get Stock By ID Async
        /// <summary>
        /// Get Stock By ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet(ID_AS_INTEGER)]
        public async Task<IActionResult> GetStockByIdAsync([FromRoute] int id)
        {
            if (ModelState.IsValid)
            {
                var stockModel = await this._stockRepositoryServices.GetStockByIdAsync(id);
                if (stockModel is null)
                {
                    return NotFound();
                }
                return Ok(stockModel.FromStockModelToStockDto());
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        #endregion

        #region Add Stock Async
        /// <summary>
        /// Add New Stock
        /// </summary>
        /// <param name="createStockRequestDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddStockAsync([FromBody] CreateStockRequestDto createStockRequestDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var stockModel = createStockRequestDto.FromCreateStockRequestDtoToStockModel();
                    if (stockModel is not null)
                    {
                        var stock = await this._stockRepositoryServices.CreateStockAsync(stockModel);
                        return CreatedAtAction(actionName: nameof(GetStockByIdAsync), routeValues: new { id = stockModel.Id }, value: stock.FromStockModelToStockDto());
                    }
                    return BadRequest("New Stock Record Is Empty!");
                }
                catch (Exception exception)
                {
                    await Console.Out.WriteLineAsync($"Error Message: {exception.Message}");
                    return BadRequest(exception.Message);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        #endregion

        #region Update Stock ASync
        /// <summary>
        /// From Route - Update Existing Stock
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateStockRequestDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route(ID_AS_INTEGER)]
        public async Task<IActionResult> UpdateStockAsync([FromRoute] int id, [FromBody] UpdateExistingStockRequestDto updateStockRequestDto)
        {
            if (ModelState.IsValid)
            {
                var stockModel = await this._stockRepositoryServices.UpdateStockAsync(id, updateStockRequestDto);

                if (stockModel is not null)
                {

                    return Ok(stockModel.FromStockModelToStockDto());
                }
                return NotFound("Stock Record Not Found!"); 
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        #endregion

        #region Delete Stock Async
        /// <summary>
        /// Delete Stock
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(ID_AS_INTEGER)]
        public async Task<IActionResult> DeleteStockAsync([FromRoute] int id)
        {
            if (ModelState.IsValid)
            {
                var stockModel = await this._stockRepositoryServices.DeleteStockAsync(id);
                if (stockModel is not null)
                {
                    return NoContent();
                }
                return NotFound("Stock Record Not Found!"); 
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        #endregion
    }
}
