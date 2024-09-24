using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.DataContext.Helpers.QueryObject
{
    /// <summary>
    /// This is class is used for filtering by Name
    /// </summary>
    public class QueryObject
    {
        public string? Symbol { get; set; } = null;

        public string? CompanyName { get; set; } = null;

        public string? SortBy { get; set; } = null;

        public bool IsDescending { get; set; } = default;

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 20;
    }
}
