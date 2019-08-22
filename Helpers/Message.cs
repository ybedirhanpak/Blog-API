using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Helpers
{
    public class Message
    {

        public Message(string context)
        {
            Context = context;
        }

        public string Context { get; set; }
    }
}
