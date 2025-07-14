namespace UserApi.DTO
{
    public class ResetPasswordRequestDto
    {
        public string Email { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmNewPassword { get; set; }

        public string Token { get; set; }
    }
}
