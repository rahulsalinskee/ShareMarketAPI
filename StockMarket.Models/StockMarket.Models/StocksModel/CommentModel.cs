using StockMarket.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Models.StocksModel
{
    [Table("TblComments")]
    public class CommentModel
    {
        public int ID { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public int? StockId { get; set; }

        public StockModel? Stock { get; set; }

        public string UserId { get; set; }

        public UsersModel UserModel { get; set; }
    }
}
