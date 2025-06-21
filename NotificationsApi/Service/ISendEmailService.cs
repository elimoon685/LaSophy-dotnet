namespace NotificationsApi.Service
{
    public interface ISendEmailService
    {
        Task SendWelcomeEmailAsync(string toEmail, string userName, string role);
    }
}
