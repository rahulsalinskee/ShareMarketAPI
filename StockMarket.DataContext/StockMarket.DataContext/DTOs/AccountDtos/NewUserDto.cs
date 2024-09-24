using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.DataContext.DTOs.AccountDtos
{
    public class NewUserDto
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }
    }
}
