using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockMarket.DataContext.DTOs.AccountDtos;
using StockMarket.DataContext.Repository.Interfaces;
using StockMarket.Models.Users;

namespace StockMarket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region Private Fields
        private readonly UserManager<UsersModel> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<UsersModel> _signInManager;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the AccountController
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="tokenService"></param>
        public AccountController(UserManager<UsersModel> userManager, ITokenService tokenService, SignInManager<UsersModel> signInManager)
        {
            this._userManager = userManager;
            this._tokenService = tokenService;
            this._signInManager = signInManager;
        }
        #endregion

        #region Register Action Method For New User
        /// <summary>
        /// Register New User
        /// </summary>
        /// <param name="registerRequestDto"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequestDto registerRequestDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var users = new UsersModel()
                    {
                        UserName = registerRequestDto.UserName,
                        Email = registerRequestDto.Email
                    };
                    var createdUser = await this._userManager.CreateAsync(users, registerRequestDto.Password);
                    if (createdUser.Succeeded)
                    {
                        var roleResult = await this._userManager.AddToRoleAsync(users, "User");
                        if (roleResult.Succeeded)
                        {
                            return Ok(new NewUserDto() { UserName = users.UserName, Email = users.Email, Token = this._tokenService.CreateToken(users) });
                        }
                        else
                        {
                            return StatusCode(500, createdUser.Errors);
                        }
                    }
                    else
                    {
                        return StatusCode(500, createdUser.Errors);
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
        }
        #endregion

        #region Login Existing User
        /// <summary>
        /// Login the specified user using username and password
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var user = await this._userManager.Users.FirstOrDefaultAsync(user => user.UserName == loginDto.UserName.ToLower());
                if (user != null)
                {
                    var result = await this._signInManager.CheckPasswordSignInAsync(user: user, password: loginDto.Password, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        return Ok(new NewUserDto() { UserName = user.UserName, Email = user.Email, Token = this._tokenService.CreateToken(user) });
                    }
                    return Unauthorized("User Name Not Found And/Or Password Is Invalid!");
                }
                return Unauthorized("Invalid User Name");
            }
            return BadRequest(ModelState);
        }
        #endregion
    }
}
