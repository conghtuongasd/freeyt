using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeGetLink.LIB
{
    public class HttpRequest
    {
        public string Cookie { get; set; }

        public string Send(string url, bool kt = false, string postData = "", bool isJson = false, bool onlyheader = false)
        {
            string html = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                if (postData == "")
                    request.Method = "GET";
                else
                    request.Method = "POST";
                if (kt == false)
                    request.AllowAutoRedirect = false;
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36 Edg/86.0.622.69";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                //Thêm cookies
                if (Cookie != "") request.Headers.Add("Cookie: " + Cookie);
                Stream dataStream;
                if (postData != "")
                {
                    request.ContentType = isJson ? "application/json;charset=UTF-8" : "application/x-www-form-urlencoded";
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    request.ContentLength = byteArray.Length;
                    dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Flush();
                    dataStream.Close();
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream, Encoding.UTF8);
                string header = response.Headers.ToString();
                html = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
                if (onlyheader) return header;
            }
            catch (Exception e) { html = "Lỗi: " + e.Message; }
            return html;
        }

        public static Stream Download(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.AllowAutoRedirect = false;
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) coc_coc_browser/53.2.131 Chrome/47.2.2526.131 Safari/537.36";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                return dataStream;
            }
            catch (Exception e) { return null; }
        }
    }
}
