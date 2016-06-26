using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Runtime.Serialization;

namespace LoveMsg
{
    [DataContract] public class Message
    {
        [DataMember] public string id;
        [DataMember] public string name;
        [DataMember] public string content;
        [DataMember] public string createtime;
    }
    public class Messages
    {
        private Queue<Message> msgqueue = new Queue<Message>();
        public bool HasMore()
        {
            return msgqueue.Count > 0;
        }
        public Message Get()
        {
            return msgqueue.Dequeue();
        }
        public void Add(Message msg)
        {
            msgqueue.Enqueue(msg);
        }

    }
}
