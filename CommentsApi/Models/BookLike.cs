namespace CommentsApi.Models
{
    public class BookLike
    {
        public int Id { get; set; }

        public int BookId { get; set; }
        public BookInfo Book { get; set; }
        public string UserId { get; set; }

        public DateTime LikedAt { get; set; } = DateTime.Now;

    }
}
