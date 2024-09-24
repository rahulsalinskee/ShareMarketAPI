using StockMarket.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.DataContext.DTOs.CommentDtos
{
    public class CreateCommentRequestDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title Must be of 5 Character")]
        [MaxLength(280, ErrorMessage = "Title Must Not be Over to 280 Character")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(5, ErrorMessage = "Content Must be of 5 Character")]
        [MaxLength(280, ErrorMessage = "Content Must Not be Over to 280 Character")]
        public string Content { get; set; } = string.Empty;
    }
}
