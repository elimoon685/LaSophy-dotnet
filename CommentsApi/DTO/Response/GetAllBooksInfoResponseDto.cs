namespace CommentsApi.DTO.Response
{
    public class GetAllBooksInfoResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Year { get; set; }

        public string PdfPath { get; set; }

        public string ImgPath { get; set; }

        public int LikeCount { get; set; }

        public int CollectCount { get; set; }

        public int CommentCount { get; set; }
    }
}
