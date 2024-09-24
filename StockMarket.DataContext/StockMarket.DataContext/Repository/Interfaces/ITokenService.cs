using StockMarket.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.DataContext.Repository.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(UsersModel users);
    }
}
