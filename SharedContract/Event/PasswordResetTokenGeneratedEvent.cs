using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedContract.Event
{
    public  class PasswordResetTokenGeneratedEvent
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public string Token { get; set; }
    }
}
