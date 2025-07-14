using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedContract.Event
{
    public class CreateNewReplyEvent
    {

        public int BookId { get; set; }

        public string Content { get; set; }

        public string ParentCommentContent { get; set; }

        public DateTime CreatedAt   { get; set; }
        public string ReplyUserId    { get; set; }
        public string SendUserId { get; set; }
        public string ReplyUserName { get; set; }    

    }
}
