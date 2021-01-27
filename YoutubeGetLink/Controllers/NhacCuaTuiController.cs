using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using YoutubeGetLink.LIB;
using YoutubeGetLink.Models.Kendo;
using YoutubeGetLink.Models.NhacCuaTui;

namespace YoutubeGetLink.Controllers
{
    public class NhacCuaTuiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Search([FromBody]FilterModel filter)
        {
            //https://regex101.com/r/XA1RvB/1
            string pattern = @"sn_search_single_song.+?<a href=""(.+?)"" title=""(.+?)"".+?<img class=""thumb"" src=""(.+?)"".+?data-src=""(.*?)"".+?class=""name_singer"".+?>(.+?)</a>";
            string patternSingerName = @"<span.+?"">|<\/span>";
            var httpRequest = new HttpRequest();
            //var result = JsonConvert.DeserializeObject<NhacCuaTuiSearchResultModel>(HttpRequest.Send("https://www.nhaccuatui.com/ajax/search?q=" + System.Net.WebUtility.UrlEncode(filter.Filter?.Filters?.FirstOrDefault().Value)));
            var response = httpRequest.Send($"https://www.nhaccuatui.com/tim-kiem/bai-hat?q={System.Net.WebUtility.UrlEncode(filter.Filter?.Filters?.FirstOrDefault()?.Value)}&b=song&l=tat-ca&s=default");
            var match = Regex.Match(response, pattern, RegexOptions.Singleline);
            var result = new List<SeachModel>();
            while(match.Success)
            {
                result.Add(
                    new SeachModel
                    {
                        Link = match.Groups[1].Value,
                        Name = match.Groups[2].Value,
                        Thumb = string.IsNullOrWhiteSpace(match.Groups[4].Value) ? match.Groups[3].Value : match.Groups[4].Value,
                        Singer = Regex.Replace(match.Groups[5].Value, patternSingerName, string.Empty)
                    }); ;
                match = match.NextMatch();    
            }
            return Json(result);
        }

        public IActionResult GetLink(string link)
        {
            //https://regex101.com/r/dNyKg0/1
            var httpRequest = new HttpRequest();
            var rp = httpRequest.Send(link, true);
            var m = Regex.Match(rp, "https://www.nhaccuatui.com/flash/xml.html5=true&key1=" + "(.+?)\"");
            var html5Response = httpRequest.Send("https://www.nhaccuatui.com/flash/xml?html5=false&key1=" + m.Groups[1].Value);
            string pattern = @"<title>\s*?<!\[CDATA\[(.+?)\]\]>\s*?<\/title>\s*?<creator>\s*?<!\[CDATA\[(.+?)\]\]>\s*?<\/creator>\s*?<location>\s*?<!\[CDATA\[(.+?)\]\]>\s*?<\/location>.+?<image>\s*?<!\[CDATA\[(.+?)\]\]>\s*?<\/image>";
            var songMatch = Regex.Match(html5Response, pattern, RegexOptions.Singleline);
            if(songMatch.Success)
            {
                var song = new SongModel
                {
                    Name = songMatch.Groups[1].Value,
                    Singer = songMatch.Groups[2].Value,
                    LinkMp3 = songMatch.Groups[3].Value,
                    Image = songMatch.Groups[4].Value
                };
                return Json(
                  new
                  {
                      data = song,
                      success = true
                  });
            }
            return Json(new { success = false});
        }

        public IActionResult Top100()
        {
            HttpRequest httpRequest = new HttpRequest();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(httpRequest.Send("https://www.nhaccuatui.com/top100/top-100-nhac-tre.m3liaiy6vVsF.html"));
            var htmlNodes = htmlDocument.DocumentNode.SelectNodes("//ul[@class='list_show_chart']/li");
            var listSong = new List<SeachModel>();
            foreach(var node in htmlNodes)
            {
                var inforField = node.SelectSingleNode("./div[@class='box_info_field']");
                var songAction = node.SelectSingleNode("./div[@class='box_song_action']");
                var title = inforField.ChildNodes["a"].GetAttributes("title").FirstOrDefault().Value;
                var defaultImage = inforField.ChildNodes["a"]?.ChildNodes["img"]?.GetAttributes("src")?.FirstOrDefault()?.Value;
                var image = inforField.ChildNodes["a"]?.ChildNodes["img"]?.GetAttributes("data-src")?.FirstOrDefault()?.Value;
                var id = songAction?.ChildNodes["a"]?.GetAttributes("id")?.FirstOrDefault()?.Value?.Split("_")[1];
                //var title = a.ChildNodes["img"].GetAttributes();
                listSong.Add(new SeachModel { 
                    Link = $"https://www.nhaccuatui.com/bai-hat/song.{id}.html",
                    Thumb = string.IsNullOrWhiteSpace(image) ? defaultImage : image,
                    Name = title
                });
            }
            return PartialView("Top100", listSong);
        }
    }
}