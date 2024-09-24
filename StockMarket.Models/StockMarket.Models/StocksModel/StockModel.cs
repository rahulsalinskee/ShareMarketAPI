using System.ComponentModel.DataAnnotations.Schema;

namespace StockMarket.Models.StocksModel
{
    [Table("TblStocks")]
    public class StockModel
    {
        public int Id { get; set; }

        public string Symbol { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Purchase { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal LastDividend { get; set; }

        public string Industry { get; set; } = string.Empty;

        public long MarketCap { get; set; }

        public List<CommentModel> Comments { get; set; } = [];

        public List<PortfolioModel> Portfolios { get; set; } = [];

    }
}
