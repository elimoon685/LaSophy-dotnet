namespace CommentsApi.Models
{
    public class CommentLike
    {

        public int Id { get; set; }

        public int CommentId { get; set; }
        public BookComments Comment { get; set; }
        public string UserId { get; set; }

        public DateTime LikedAt { get; set; } = DateTime.Now;
    }
}
