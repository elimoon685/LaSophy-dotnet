using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedContract.Event
{
    public class CreateUserEvent
    {
        /*
        public CreateUserEvent(string userId, string name, string email, DateTime createdAt)
        {
            UserID=Guid.Parse(userId);
            UserName=name;
            UserEmail=email;
            CreatedAt=createdAt;
        }
        */
        public Guid UserID { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public DateTime CreatedAt {get; set; }= DateTime.Now;

        public string Role {  get; set; }
    }
}
