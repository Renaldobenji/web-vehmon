using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Logic;

namespace TestLogic
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadKey();
            CreateUser();
        }

        static private void CreateUser()
        {

            var restult = PostData("http://localhost:51761/Authentication.svc/CreateUser/PhilipSc/MyPasswordHere/Mr/Philip/Schoeman/6546/8911035116084/98496645/philip.programmer@gmail.com/0745899420");
        }

        static private string PostData(string url)
        {
            WebClient serviceRequest = new WebClient();
            string response = serviceRequest.UploadString(new Uri("http://localhost:51761/Authentication.svc"), "POST", "/CreateUser/PhilipSc/MyPasswordHere/Mr/Philip/Schoeman/6546/8911035116084/98496645/philip.programmer@gmail.com/0745899420");
            return response;
        }
    }
}
