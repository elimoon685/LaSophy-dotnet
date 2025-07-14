using CommentsApi.Models;

namespace CommentsApi.DTO.Response
{
    public class GetCommentsResponseDto
    {
        public int CommentsId { get; set; }

        public string Content { get; set; }
        public string CreatedBy { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int BookId { get; set; }
        public int? ParentCommentId { get; set; }
        public int CommentLikesCount { get; set; }
        public List<GetCommentsResponseDto> Replies { get; set; }
    }
}
