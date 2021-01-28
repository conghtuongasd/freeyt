using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;

namespace YouYou
{
    public class Connect
    {
        public static string URL_YOUTUBE = "https://www.youtube.com";
        public static string URL_YOUTUBE_WATCH = "https://www.youtube.com/watch?v=";
        public static string URL_YOUTUBE_INFO = "https://www.youtube.com/get_video_info?video_id=";
        public static string URL_YOUTUBE_SEARCH = "https://www.youtube.com/results?search_query=";
        public static string URL_YOUTUBE_SEARCH_KEYWORD = "http://suggestqueries.google.com/complete/search?client=youtube&ds=yt&q=";

        public static string GetDataFromUrl(string url)
        {
            string data = "";
            try
            {
                WebClient web = new WebClient();
                web.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0";
                web.Encoding = Encoding.UTF8;
                data = Encoding.UTF8.GetString(web.DownloadData(url));
            }
            catch { }
            return data;
        }

        public static ArrayList GetQuality(string video_id)
        {
            ArrayList quality = new ArrayList();
            Dictionary<string, string> typeMap = BuildTypeMap();
            Dictionary<string, string> arr_itags = new Dictionary<string, string>();
            try
            {
                string html = Connect.GetDataFromUrl(URL_YOUTUBE_WATCH + video_id);
                string match = "url_encoded_fmt_stream_map";
                int start = 0;

                if (html.IndexOf(match, 0) > 0)
                {
                    start = html.IndexOf(match, 0) + match.Length + 3;
                    string str = html.Substring(start, html.IndexOf("\",", start));

                    string[] fmts = str.Split(',');
                    foreach (string f in fmts)
                    {
                        if (!f.Equals(""))
                        {
                            string a = f.Replace("u0026", "");
                            string itag = "";
                            long size = 0;
                            string url = "";
                            foreach (string attr in a.Split('\\'))
                            {
                                if (attr.StartsWith("itag")) { itag = attr.Substring(5); }
                                if (attr.StartsWith("clen")) { size = long.Parse(attr.Substring(5)); }
                                if (attr.StartsWith("url")) { url = HttpUtility.UrlDecode(attr.Substring(4)); }
                            }

                            if (!arr_itags.ContainsKey(itag))
                            {
                                if (typeMap.ContainsKey(itag))
                                {
                                    string type = typeMap[itag];
                                    string[] type_ = type.Split(':');
                                    //quality.Add(new FileDownload("", "", size, url, type_[0], type_[1]));
                                    arr_itags.Add(itag, "");
                                }
                            }
                        }
                    }
                }

                html = Connect.GetDataFromUrl(URL_YOUTUBE_INFO + video_id);

                start = html.IndexOf(match, 0) + match.Length + 1;
                html = html.Substring(start, html.IndexOf("&", start) - start);
                html = HttpUtility.UrlDecode(html);

                string[] items = html.Split(',');
                foreach (string item in items)
                {

                    string itag = "";
                    string url = "";
                    long size = 0;
                    foreach (string attr in item.Split('&'))
                    {
                        if (attr.StartsWith("itag")) { itag = attr.Substring(5); }
                        if (attr.StartsWith("url")) { url = attr.Substring(4); }
                    }
                    if (!arr_itags.ContainsKey(itag))
                    {
                        if (typeMap.ContainsKey(itag))
                        {
                            string type = typeMap[itag];
                            string[] type_ = type.Split(':');
                            //quality.Add(new FileDownload("", "", size, url, type_[0], type_[1]));
                            arr_itags.Add(itag, "");
                        }
                    }
                }
            }
            catch { }
            return quality;
        }

        public static long GetSizeFromUrl(string url)
        {
            long len = 0;
            try
            {
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Timeout = 5000;
                request.Method = "HEAD";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                len = response.ContentLength;
                response.Close();
            }
            catch { }
            return len;
        }

        public static string FormatSize(long size)
        {
            string f = "B";
            double rs = 0;
            if (size >= 1024)
            {
                f = "KB";
                rs = size / 1024;
                if (rs >= 1024)
                {
                    f = "MB";
                    rs /= 1024;
                }
            }
            return rs.ToString("N2") + f;
        }

        public static string OptimString(string str)
        {
            str = str.Replace(":", " ").Replace("!", "").Replace("#", "").Replace("\"", "");
            str.Replace("  ", " ");
            return str;
        }

        public static Dictionary<string, string> BuildTypeMap()
        {
            Dictionary<string, string> typeMap = new Dictionary<string, string>();
            typeMap.Add("5", "FLV:Low Quality - 240p");
            typeMap.Add("6", "FLV:Medium Quality - 360p");
            typeMap.Add("13", "3GP:Low Quality - 144p");
            typeMap.Add("17", "3GP:Low Quality - 144p");
            typeMap.Add("18", "MP4:Medium Quality - 360p");
            typeMap.Add("22", "MP4:High Quality - 720p");
            typeMap.Add("33", "MP4:High Quality - 240p");
            typeMap.Add("34", "FLV:Medium Quality - 360p");
            typeMap.Add("35", "FLV:Standard Definition - 480p");
            typeMap.Add("36", "3GP:Low Quality - 240p");
            typeMap.Add("37", "MP4:Full High Quality - 1080p");
            typeMap.Add("38", "MP4:Original Definition - 3072p");
            //typeMap.Add("43", "WebM:Medium Quality - 360p");
            //typeMap.Add("44", "WebM:Standard Definition - 480p");
            //typeMap.Add("45", "WebM:High Quality - 720p");
            //typeMap.Add("46", "WebM:Full High Quality - 1080p");
            typeMap.Add("82", "MP4:Medium Quality - 360p 3D");
            typeMap.Add("83", "MP4:Standard Definition - 480p 3D");
            typeMap.Add("84", "MP4:High Quality - 720p 3D");
            typeMap.Add("85", "MP4:Full High Quality - 1080p 3D");
            //typeMap.Add("100", "WebM:Medium Quality - 360p 3D");
            //typeMap.Add("101", "WebM:Standard Definition - 480p 3D");
            //typeMap.Add("102", "WebM:High Quality - 720p 3D");
            typeMap.Add("133", "MP4:Low Quality - 240p VO");
            typeMap.Add("134", "MP4:Medium Quality - 360p VO");
            typeMap.Add("135", "MP4:Standard Definition - 480p VO");
            typeMap.Add("136", "MP4:High Quality - 720p VO");
            typeMap.Add("137", "MP4:Full High Quality - 1080p VO");
            typeMap.Add("139", "MP4:Low Quality - bitrate AO");
            typeMap.Add("140", "MP4:Medium Quality - bitrate AO");
            typeMap.Add("141", "MP4:High Quality - bitrate AO");
            typeMap.Add("160", "MP4:Low Quality - 144p VO");
            //typeMap.Add("171", "WebM:Medium Quality - bitrate AO");
            //typeMap.Add("172", "WebM:High Quality - bitrate AO");
            //typeMap.Add("242", "WebM:Low Quality - 240p VOX");
            //typeMap.Add("243", "WebM:Medium Quality - 360p VOX");
            //typeMap.Add("244", "WebM:Standard Definition - 480p VOX");
            //typeMap.Add("245", "WebM:Standard Definition - 480p VOX");
            //typeMap.Add("246", "WebM:Standard Definition - 480p VOX");
            //typeMap.Add("247", "WebM:High Quality - 720p VOX");
            //typeMap.Add("248", "WebM:Full High Quality - 1080p VOX");
            //typeMap.Add("249", "WebM:Low Quality - bitrate AOX");
            //typeMap.Add("250", "WebM:Medium Quality - bitrate AOX");
            //typeMap.Add("251", "WebM:High Quality - bitrate AOX");
            typeMap.Add("264", "MP4:Full High Quality - 1440p VO");
            typeMap.Add("266", "MP4:Full High Quality - 2160p VO");
            //typeMap.Add("271", "WebM:Full High Quality - 1440p VOX");
            //typeMap.Add("272", "WebM:Full High Quality - 2160p VOX");
            //typeMap.Add("278", "WebM:Low Quality - 144p VOX");
            typeMap.Add("298", "MP4:High Quality - 720p VO");
            //typeMap.Add("302", "WebM:High Quality - 720p VOX");

            return typeMap;
        }
    }
}
