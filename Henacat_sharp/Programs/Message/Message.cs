using System;
using System.Collections.Generic;

namespace Message
{
    public class Message
    {
        public static List<Message> messageList = new List<Message>();

        public String title;
        public String handle;
        public String message;
        public DateTime date;

        public Message( String title, String handle, String message )
        {
            this.title = title;
            this.handle = handle;
            this.message = message;
            this.date = new DateTime();
        }
    }
}
