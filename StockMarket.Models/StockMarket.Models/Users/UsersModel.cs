using Microsoft.AspNetCore.Identity;
using StockMarket.Models.StocksModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Models.Users
{
    public class UsersModel : IdentityUser
    {
        public List<PortfolioModel> Portfolios { get; set; } = [];
    }
}
