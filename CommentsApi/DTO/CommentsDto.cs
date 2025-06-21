using CommentsApi.Models;

namespace CommentsApi.DTO
{
    public class CommentsDto
    {
        public int CommentsId { get; set; }

        public string Content { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int BookId { get; set; }
        public int? ParentCommentId { get; set; }
        public List<CommentsDto> Replies { get; set; }
    }
}
