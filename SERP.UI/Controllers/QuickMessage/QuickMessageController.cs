using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SERP.UI.Controllers.QuickMessage
{
    public class QuickMessageController : Controller
    {

        public string Index()
        {
            //using (var httpClient = new HttpClient())
            //{
            //    using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.ultramsg.com/instance11520/messages/chat"))
            //    {
            //        var contentList = new List<string>();
            //        contentList.Add("token=216tq4ry5y9cbhek");
            //        contentList.Add("to=+919315775084");
            //        contentList.Add("body=WhatsApp API on UltraMsg.com works good");
            //        contentList.Add("priority=10");
            //        contentList.Add("referenceId=");
            //        request.Content = new StringContent(string.Join("&", contentList));
            //        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

            //        var response = await httpClient.SendAsync(request);
            //    }
            //}
            //oA777Gk2YFA-zL5QeUl4wIwDEZ9YwgXpQUzESTgll2

            //String result;
            //string apiKey = "oA777Gk2YFA-zL5QeUl4wIwDEZ9YwgXpQUzESTgll2";
            //string numbers = "919315775084"; // in a comma seperated list
            //string message = "Hi there, thank you for sending your first test message from Textlocal. See how you can send effective SMS campaigns here: https://tx.gl/r/2nGVj/";
            //string sender = "TXTLCL";

            //String url = "https://api.textlocal.in/send/?apikey=" + apiKey + "&numbers=" + numbers + "&message=" + message + "&sender=" + sender;
            ////refer to parameters to complete correct url string

            //StreamWriter myWriter = null;
            //HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);

            //objRequest.Method = "POST";
            //objRequest.ContentLength = Encoding.UTF8.GetByteCount(url);
            //objRequest.ContentType = "application/x-www-form-urlencoded";
            //try
            //{
            //    myWriter = new StreamWriter(objRequest.GetRequestStream());
            //    myWriter.Write(url);
            //}
            //catch (Exception e)
            //{
            //    return e.Message;
            //}
            //finally
            //{
            //    myWriter.Close();
            //}

            //HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            //using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            //{
            //    result = sr.ReadToEnd();
            //    // Close and clean up the StreamReader
            //    sr.Close();
            //}
            //return result;
            return "";
        }
    }
}