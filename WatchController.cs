using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VideoLibrary;
using YouYou.Models;

namespace YouYou
{
    public class WatchController : Controller
    {
        public IActionResult Index(string v)
        {
            VideoItemVM video = new VideoItemVM();
            string html = Connect.GetDataFromUrl(Connect.URL_YOUTUBE_WATCH + v).Replace("\\u0026", "&");
            int pos = html.IndexOf("<meta name=\"title\" content=") + 28;
            video.Title = html.Substring(pos, html.IndexOf("\"", pos) - pos);

            var youTube = YouTube.Default;
            var urls = youTube.GetAllVideos(Connect.URL_YOUTUBE_WATCH + v);
            video.Qualities = urls?.Where(item => item.FileExtension.Equals(".mp4") && item.Resolution > 0 && item.AudioBitrate > 0)?.OrderByDescending(item => item.Resolution).ToList();
            video.NextVideos = new List<VideoItemVM>();
            string match = "compactVideoRenderer";
            pos = 0;

            while ((pos = html.IndexOf(match, pos)) > 0)
            {
                pos += match.Length;
                pos = html.IndexOf("videoId", pos) + 10;
                string id = html.Substring(pos, html.IndexOf("\"", pos) - pos);

                pos = html.IndexOf("thumbnail", pos) + 10;
                pos = html.IndexOf("url", pos) + 6;
                string thumbnail = html.Substring(pos, html.IndexOf("\"", pos) - pos);

                pos = html.IndexOf("title", pos) + 5;
                pos = html.IndexOf("simpleText", pos) + 13;
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

                video.NextVideos.Add(new VideoItemVM { Id = id, Title = title, Thumbnail = thumbnail, Poster = poster, Duration = duration, Views = views });
            }

            return View(video);
        }
    }
}
