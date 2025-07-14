namespace UserApi.DTO
{
    public class UserInfoResponseDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        public DateTime CreatedAt { get; set; }
        public string Bio {  get; set; }
    }
}
