using System;
using System.IO;
using System.Linq;
using System.Net;

namespace TestServices
{
    public class RestCaller
    {
        private readonly string _baseUrl;
        public RestCaller(string baseURL)
        {
            _baseUrl = baseURL;
        }

        public string PostService(string serviceName, string methodName, params string[] parameters)
        {
            return CallService(serviceName, methodName, "Post", parameters);
        }

        public string GetService(string serviceName, string methodName, params string[] parameters)
        {
            return CallService(serviceName, methodName, "Get", parameters);
        }

        private string CallService(string serviceName, string methodName, string methodType, params string[] parameters)
        {
            var url = String.Format("{0}/{1}/{2}", _baseUrl, serviceName, methodName);

            url = parameters.Aggregate(url, (current, parameter) => current + ("/" + parameter));

            var webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.KeepAlive = false;
            webrequest.ProtocolVersion = HttpVersion.Version10;
            webrequest.ServicePoint.ConnectionLimit = 1;
            webrequest.ContentType = "text/XML; charset=utf-8";
            webrequest.Method = methodType;
            webrequest.ContentLength = 0;
            string result;
            WebResponse response = webrequest.GetResponse();

            var reader = new StreamReader(response.GetResponseStream());
                {
                    result = reader.ReadToEnd();
                }
                return result;
            

           
        }
    }
}
