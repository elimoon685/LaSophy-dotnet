using System.ComponentModel.DataAnnotations;

namespace CommentsApi.Models
{
    public class BookInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Year { get; set; }

        public string PdfPath {  get; set; }

        public string ImgPath { get; set; }
        
        public List<BookLike> Likes { get; set; }

        public List<BookCollection> Collections { get; set; }

        public List<BookComments> Comments { get; set; }
        
    }
}
