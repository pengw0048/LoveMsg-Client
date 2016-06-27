using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;

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
        private bool running = true;
        private Form1 instance;
        private Thread updmsg;
        public Messages(Form1 instance)
        {
            this.instance = instance;
            updmsg = new Thread(new ThreadStart(UpdateMsg));
            updmsg.Start();
        }
        ~Messages()
        {
            running = false;
            updmsg.Abort();
        }
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
        private void UpdateMsg()
        {
            var inteval = new TimeSpan(0, 0, 4);
            var ser = new DataContractJsonSerializer(typeof(Message[]));
            var addr = Http.server + "action=getmsg&group=" + instance.settings.Get("group", "") + "&member=" + instance.settings.Get("member", "");
            while (running)
            {
                var start = DateTime.Now;
                try
                {
                    var res = Http.HttpGet(addr);
                    using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(res)))
                    {
                        var obj = (Message[])ser.ReadObject(ms);
                        foreach (var msg in obj)
                        {
                            msgqueue.Enqueue(msg);
                        }
                    }
                }
                catch (Exception) { }
                var end = DateTime.Now;
                if (end - start < inteval)
                    try
                    {
                        Thread.Sleep(inteval - (end - start));
                    }
                    catch (Exception) { }
            }
        }
    }
}
