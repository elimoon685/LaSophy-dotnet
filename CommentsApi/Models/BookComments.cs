using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommentsApi.Models
{
    public class BookComments
    {
        [Key]
        public int CommentsId { get; set; }

        public string Content  { get; set; }
        public string CreatedBy  { get; set; }
        public DateTime CreatedAt { get; set; }=DateTime.Now;
        public int BookId { get; set; }
        public BookInfo Book { get; set; }
        public int? ParentCommentId { get; set; }
        public BookComments ParentComment { get; set; }
        public List<BookComments> Replies { get; set; }


    }
}
