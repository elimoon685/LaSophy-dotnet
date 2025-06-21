using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IdentityModel.Tokens.Jwt;

namespace UserApi.Models
{
    public class UserEvent
    {

        public string UserEventId { get; set; }

        public string UserId { get; set; }

        public string EventType { get; set; }

        public string EventStatus { get; set; }

        public string Payload { get; set; }

        public DateTime CreatedAt { get; set; }= DateTime.Now;

        public DateTime UpdatedAt { get; set;} = DateTime.Now;
    }
}
