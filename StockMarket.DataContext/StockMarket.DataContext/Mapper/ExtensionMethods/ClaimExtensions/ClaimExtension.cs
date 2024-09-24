using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.DataContext.Mapper.ExtensionMethods.ClaimExtensions
{
    public static class ClaimExtension
    {
        private const string CLAIM_TYPE = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

        public static string GetUserNameExtension(this ClaimsPrincipal claimsPrincipalForUser)
        {
            return claimsPrincipalForUser.Claims.SingleOrDefault(item => item.Type.Equals(value: CLAIM_TYPE)).Value;
        }
    }
}
