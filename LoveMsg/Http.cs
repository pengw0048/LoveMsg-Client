using System;
using System.IO;
using System.Net;
using System.Text;

namespace LoveMsg
{
    class Http
    {
        public static string server = "http://localhost/lovemsg.php?";
        public static string GetResponse(ref HttpWebRequest req, out CookieCollection cookies, bool GetLocation = false, bool GetRange = false, bool NeedResponse = true)
        {
            HttpWebResponse res = null;
            cookies = null;
            try
            {
                res = (HttpWebResponse)req.GetResponse();
                cookies = res.Cookies;
            }
            catch (WebException e)
            {
                StreamReader ereader = new StreamReader(e.Response.GetResponseStream(), Encoding.UTF8);
                string erespHTML = ereader.ReadToEnd();
                Console.WriteLine(erespHTML);
                throw new Exception(erespHTML);
            }
            if (GetLocation)
            {
                string ts = res.Headers["Location"];
                res.Close();
                Console.WriteLine("Location: " + ts);
                return ts;
            }
            if (GetRange && res.ContentLength == 0)
            {
                string ts = res.Headers["Range"];
                Console.WriteLine("Range: " + ts);
                return ts;
            }
            if (NeedResponse)
            {
                StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
                string respHTML = reader.ReadToEnd();
                res.Close();
                //Console.WriteLine(respHTML);
                return respHTML;
            }
            else
            {
                res.Close();
                return "";
            }
        }
        public static string GetResponse(ref HttpWebRequest req, bool GetLocation = false, bool GetRange = false, bool NeedResponse = true)
        {
            CookieCollection c;
            return GetResponse(ref req, out c, GetLocation, GetRange, NeedResponse);
        }
        public static HttpWebRequest GenerateRequest(string URL, string Method, string token, bool KeepAlive = false, string ContentType = null, byte[] data = null, int offset = 0, int length = 0, string ContentRange = null, bool PreferAsync = false, int Timeout = 20 * 1000, string host = null, string Referer = null, string Accept = null, CookieCollection cookies = null)
        {
            Uri httpUrl = new Uri(URL);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(httpUrl);
            req.ProtocolVersion = new System.Version("1.0");
            req.Timeout = Timeout;
            req.ReadWriteTimeout = Timeout;
            req.Method = Method;
            if (token != null) req.Headers.Add("Authorization", "Bearer " + token);
            req.KeepAlive = KeepAlive;
            if (ContentType != null) req.ContentType = ContentType;
            if (ContentRange != null) req.Headers.Add("Content-Range", ContentRange);
            if (PreferAsync == true) req.Headers.Add("Prefer", "respond-async");
            if (Referer != null) req.Referer = Referer;
            if (Accept != null) req.Accept = Accept;
            if (cookies != null)
            {
                req.CookieContainer = new CookieContainer();
                req.CookieContainer.Add(cookies);
            }
            if (data != null)
            {
                req.ContentLength = length;
                Stream stream = req.GetRequestStream();
                stream.Write(data, offset, length);
                stream.Close();
            }
            return req;
        }
        public static string HttpGet(string URL, string token = null, bool GetLocation = false, bool AllowAutoRedirect = true, bool NeedResponse = true, int Timeout = 5 * 1000, string host = null)
        {
            HttpWebRequest req = GenerateRequest(URL, "GET", token, false, null, null, 0, 0, null, false, Timeout, host);
            if (AllowAutoRedirect == false) req.AllowAutoRedirect = false;
            return GetResponse(ref req, GetLocation, false, NeedResponse);
        }
        public static string HttpGet(string URL, out CookieCollection cookies, string token = null, bool GetLocation = false, bool AllowAutoRedirect = true, bool NeedResponse = true, int Timeout = 5 * 1000, string host = null)
        {
            HttpWebRequest req = GenerateRequest(URL, "GET", token, false, null, null, 0, 0, null, false, Timeout, host);
            if (AllowAutoRedirect == false) req.AllowAutoRedirect = false;
            return GetResponse(ref req, out cookies, GetLocation, false, NeedResponse);
        }
        public static string HttpPost(string URL, string token, byte[] data, int offset = 0, int length = -1, bool NeedResponse = true, int Timeout = 20 * 1000, string host = null, string ContentType = null, string Referer = null, string Accept = null, CookieCollection cookiesin = null)
        {
            if (length == -1) length = data.Length;
            HttpWebRequest req = GenerateRequest(URL, "POST", token, false, ContentType, data, 0, data.Length, null, false, Timeout, host, Referer, Accept, cookiesin);
            return GetResponse(ref req, false, false, NeedResponse);
        }
        public static string HttpPost(string URL, string token, byte[] data, out CookieCollection cookies, int offset = 0, int length = -1, bool NeedResponse = true, int Timeout = 20 * 1000, string host = null, string ContentType = null, string Referer = null, string Accept = null, CookieCollection cookiesin = null)
        {
            if (length == -1) length = data.Length;
            HttpWebRequest req = GenerateRequest(URL, "POST", token, false, ContentType, data, 0, data.Length, null, false, Timeout, host, Referer, Accept, cookiesin);
            return GetResponse(ref req, out cookies, false, false, NeedResponse);
        }
    }
}
