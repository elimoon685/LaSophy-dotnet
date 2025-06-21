using CommentsApi.Models;
using System.ComponentModel.DataAnnotations;

namespace CommentsApi.DTO
{
    public class CreateCommentsRequestDto
    {


        public string Content { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int BookId { get; set; }
        public int? ParentCommentId { get; set; }

    }
}
