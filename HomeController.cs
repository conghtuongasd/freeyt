using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YouYou.Models;

namespace YouYou
{
    public class HomeController : Controller
    {
        public IActionResult Index(string s)
        {
            List<VideoItemVM> videos = new List<VideoItemVM>();
            if (!string.IsNullOrEmpty(s))
            {
                string html = Connect.GetDataFromUrl(Connect.URL_YOUTUBE_SEARCH + s).Replace("\\u0026", "&");
                string match = "videoRenderer";
                int pos = 0;

                while ((pos = html.IndexOf(match, pos)) > 0)
                {
                    pos += match.Length;
                    pos = html.IndexOf("videoId", pos) + 10;
                    string id = html.Substring(pos, html.IndexOf("\"", pos) - pos);

                    pos = html.IndexOf("thumbnail", pos) + 10;
                    pos = html.IndexOf("url", pos) + 6;
                    string thumbnail = html.Substring(pos, html.IndexOf("\"", pos) - pos);

                    pos = html.IndexOf("title", pos) + 5;
                    pos = html.IndexOf("text", pos) + 7;
                    string title = html.Substring(pos, html.IndexOf("\"", pos) - pos);

                    pos = html.IndexOf("longBylineText", pos) + 15;
                    pos = html.IndexOf("text", pos) + 7;
                    string poster = html.Substring(pos, html.IndexOf("\"", pos) - pos);

                    pos = html.IndexOf("lengthText", pos) + 10;
                    pos = html.IndexOf("simpleText", pos) + 13;
                    string duration = html.Substring(pos, html.IndexOf("\"", pos) - pos);

                    pos = html.IndexOf("viewCountText", pos) + 13;
                    pos = html.IndexOf("simpleText", pos) + 13;
                    string views = html.Substring(pos, html.IndexOf("\"", pos) - pos);

                    videos.Add(new VideoItemVM { Id = id, Title = title, Thumbnail = thumbnail, Poster = poster, Duration = duration, Views = views });
                }
            }

            return View(videos);
        }

    }
}
