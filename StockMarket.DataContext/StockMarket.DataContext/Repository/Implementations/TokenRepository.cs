using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using StockMarket.DataContext.Repository.Interfaces;
using StockMarket.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.DataContext.Repository.Implementations
{
    /// <summary>
    /// Token Service implementation
    /// </summary>
    public class TokenRepository : ITokenService
    {
        #region Private Fields
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _symmetricSecurityKey; 
        #endregion

        #region Constructor
        /// <summary>
        /// Token Service Constructor
        /// </summary>
        /// <param name="configuration"></param>
        public TokenRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
            this._symmetricSecurityKey = new(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]));
        } 
        #endregion

        #region Create Token
        /// <summary>
        /// Create Token
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public string CreateToken(UsersModel users)
        {
            var claims = new List<Claim>()
            {
                new (JwtRegisteredClaimNames.Email, users.Email),
                new (JwtRegisteredClaimNames.GivenName, users.UserName),
            };

            var credentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescrptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = credentials,
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"]
            };

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescrptor);
            return tokenHandler.WriteToken(token);
        } 
        #endregion
    }
}
