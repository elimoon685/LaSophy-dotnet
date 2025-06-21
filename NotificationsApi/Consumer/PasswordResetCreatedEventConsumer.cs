using MassTransit;
using SharedContract.Event;

namespace NotificationsApi.Consumer
{
    public class PasswordResetCreatedEventConsumer:IConsumer<PasswordResetTokenGeneratedEvent>
    {
        public async Task Consume(ConsumeContext<PasswordResetTokenGeneratedEvent> context)
        {
            try
            {
                var message = context.Message;
                // Your logic
                Console.WriteLine($"Check your eamil, To create a new password, follow the link we sent to {message.Email}. Be sure to check your spam folder.Still can't find it? Resend email");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                throw; // Re-throw so it goes to error queue
            }
        }

    }
}
