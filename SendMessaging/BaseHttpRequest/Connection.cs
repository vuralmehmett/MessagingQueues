using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using SendMessaging.Model;


namespace SendMessaging.BaseHttpRequest
{
    public class Connection
    {
        public string Post(MessagingQueue message)
        {
            string Url = "http://localhost/MessagingQueues.WebApi/api/messagingqueues/senddata";
            string result = "";
            Random rnd = new Random();

            var webrequest = WebRequest.Create(Url);
            var enc = new UTF8Encoding(false);
            var serializedJson = JsonConvert.SerializeObject(message);
            var data = enc.GetBytes(serializedJson);

            webrequest.Method = "POST";
            webrequest.ContentType = "application/json";
            webrequest.ContentLength = data.Length;
            webrequest.Timeout = 500000000;

            using (var sr = webrequest.GetRequestStream())
            {
                sr.Write(data, 0, data.Length);
            }
            var res = webrequest.GetResponse();

            // ReSharper disable once AssignNullToNotNullAttribute
            result = new StreamReader(res.GetResponseStream()).ReadToEnd();

            return result;
        }
    }
}
