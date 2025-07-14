namespace NotificationsApi.Service
{
    public interface ISendNotificationService
    {
        Task SendWelcomeEmailAsync(string toEmail, string userName, string role);

        Task SendEmailVerifyEamilAsync(string email, string userName, string token);
    }
}
