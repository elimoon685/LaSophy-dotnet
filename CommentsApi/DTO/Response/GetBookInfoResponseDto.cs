namespace CommentsApi.DTO
{
    public class GetBookInfoResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Year { get; set; }

        public bool CurrentUserLike { get; set; }

        public bool CurrentUserCollect {  get; set; }

        public int LikeCount { get; set; }

        public int CollectCount { get; set; }

        public int CommentCount { get; set; }
    }
}
