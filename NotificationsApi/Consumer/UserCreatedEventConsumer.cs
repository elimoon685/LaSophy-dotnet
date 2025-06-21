using MassTransit;
using SharedContract.Event;

namespace NotificationsApi.Consumer
{
    public class UserCreatedEventConsumer:IConsumer<CreateUserEvent>
    {


        public async Task Consume(ConsumeContext<CreateUserEvent> context)
        {
            try
            {
                var message = context.Message;
                // Your logic
                Console.WriteLine($"Please send email to this {message.UserEmail} with subject{message.UserName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                throw; // Re-throw so it goes to error queue
            }
        }
    }
}
