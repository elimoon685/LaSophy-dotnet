namespace CommentsApi.DTO
{
    public class CreateBookInfoRequestDto
    {

        public string Title { get; set; }
        public string Author { get; set; }
        public string Year { get; set; }

        public string PdfPath { get; set; }

        public string ImgPath { get; set; }
    }
}
