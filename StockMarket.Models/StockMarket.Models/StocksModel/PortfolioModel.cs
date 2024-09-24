using StockMarket.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Models.StocksModel
{
    [Table("TblPortfolio")]
    public class PortfolioModel
    {
        public string UserId { get; set; }

        public int StockId { get; set; }

        public UsersModel UsersModel { get; set; }

        public StockModel StockModel { get; set; }
    }
}
