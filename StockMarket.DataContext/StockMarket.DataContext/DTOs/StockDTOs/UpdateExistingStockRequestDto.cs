using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.DataContext.DTOs.StockDTOs
{
    public class UpdateExistingStockRequestDto
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Symbol must not be over 10 characters long")]
        public string Symbol { get; set; } = string.Empty;

        [Required]
        [MaxLength(10, ErrorMessage = "Symbol must not be over 10 characters long")]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [Range(1, 100000000)]
        public decimal Purchase { get; set; }

        [Required]
        [Range(0.01, 100)]
        public decimal LastDividend { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "Industry must not be over 10 characters long")]
        public string Industry { get; set; } = string.Empty;

        [Range(1, 5000000000)]
        public long MarketCap { get; set; }
    }
}
