namespace CommentsApi.Models
{
    public class BookCollection
    {
      
        public int Id { get; set; }

        public int BookId { get; set; }
        public BookInfo Book { get; set; }
        public string UserId { get; set; }

        public DateTime CollectedAt { get; set; } = DateTime.Now;
    }
}
