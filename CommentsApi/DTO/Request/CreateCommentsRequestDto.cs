using CommentsApi.Models;
using System.ComponentModel.DataAnnotations;

namespace CommentsApi.DTO.Request
{
    public class CreateCommentsRequestDto
    {
        public string Content { get; set; }
        public int BookId { get; set; }
        public int? ParentCommentId { get; set; }

    }
}
