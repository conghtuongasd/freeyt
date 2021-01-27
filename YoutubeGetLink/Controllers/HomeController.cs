using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using YoutubeGetLink.LIB;
using YoutubeGetLink.Models;

namespace YoutubeGetLink.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _env;
        public HomeController(IHostingEnvironment env)
        {
            _env = env;
        }
        public IActionResult Index(string url)
        {
            ViewBag.Url = url;
            return View();
        }
       

        public IActionResult GetLink(string url)
        {
            try
            {
                var a = YoutubeLink.GetLink(url);
                return Json(new VideoModel
                {
                    Url = a,
                    IsSuccess = true
                });
            }
            catch (System.Exception e)
            {
                return Json(new VideoModel
                {
                    IsSuccess = false,
                    Title = e.Message
                });
            }
        }
        
        public IActionResult GetLinks(string url)
        {
            try
            {
                var videoModel = new VideoModel();
                videoModel.IsSuccess = true;
                var videos = YoutubeLink.GetLinks(url);
                videoModel.Title = videos?.FirstOrDefault()?.Title;
                videoModel.YoutubeVideos = videos?.Select(item => new YoutubeVideoModel
                {
                    Resolution = item.Resolution,
                    Url = item.Uri
                }).ToList();
                return Json(videoModel);
            }
            catch (System.Exception e)
            {
                return Json(new VideoModel
                {
                    IsSuccess = false,
                    Title = e.Message
                });
            }
        }

        public IActionResult GetLinkss(string url)
        {
            try
            {
                var videos = YoutubeLink.GetLinks(url);
                return Json(videos, new JsonSerializerSettings { Formatting = Formatting.Indented });
            }
            catch (System.Exception e)
            {
                return Json(new VideoModel
                {
                    IsSuccess = false,
                    Title = e.Message
                });
            }
        }

        //public IActionResult SearchVideos(string key)
        //{
        //    var youtubeService = new YouTubeService(new BaseClientService.Initializer()
        //    {
        //        ApiKey = "AIzaSyAO_FJ2SlqU8Q4STEHLGCilw_Y9_11qcW8",
        //        ApplicationName = this.GetType().ToString()
        //    });
        //    var searchRequest = youtubeService.Search.List("snippet");
        //    searchRequest.Q = key;
        //    searchRequest.Type = "video";
        //    var searchResult = new List<SearchResult>();
        //    var i = 0;
        //    do
        //    {
        //        var response = searchRequest.Execute();
        //        if(string.IsNullOrWhiteSpace(response?.NextPageToken))
        //        {
        //            break;
        //        }
        //        searchRequest.PageToken = response.NextPageToken;
        //        searchResult.AddRange(response.Items);
        //        i++;
        //    } while (i < 5);
        //    return Json(searchResult, new JsonSerializerSettings { Formatting = Formatting.Indented });
        //}

        public IActionResult SearchVideos(string key, string continuation)
        {
            var httpRequest = new HttpRequest();
            //var response = httpRequest.Send("https://www.youtube.com/results?search_query=" + key);
            //var match = Regex.Match(response, "var ytInitialData = (.+?);", RegexOptions.Multiline);
            //var json = match.Success ? match.Groups[1].Value : string.Empty;
            var response = httpRequest.Send("https://www.youtube.com/youtubei/v1/search?key=AIzaSyAO_FJ2SlqU8Q4STEHLGCilw_Y9_11qcW8", false,
                JsonConvert.SerializeObject(
                    new YoutubeSearchModel
                    {
                        query = key,
                        continuation = !string.IsNullOrWhiteSpace(continuation) ? continuation : null
                    }
                    )
                , true);
            var youtubeSearch = JsonConvert.DeserializeObject<YoutubeSearchResultModel>(response);
            var videoContents = new List<Content2>();
            if(youtubeSearch?.contents != null)
            {
                videoContents = youtubeSearch?.contents?.twoColumnSearchResultsRenderer?
                    .primaryContents?.sectionListRenderer?.contents?.FirstOrDefault()?
                    .itemSectionRenderer?.contents?.Where(item => item.videoRenderer != null)?.ToList();
                
                continuation = youtubeSearch?.contents?.twoColumnSearchResultsRenderer?
                    .primaryContents?.sectionListRenderer?
                    .contents?.Where(item => item.continuationItemRenderer != null)?
                    .FirstOrDefault()?.continuationItemRenderer?
                    .continuationEndpoint?.continuationCommand?.token;
            }
            if(youtubeSearch?.onResponseReceivedCommands != null)
            {
                videoContents = youtubeSearch?.onResponseReceivedCommands?.FirstOrDefault()
                    .appendContinuationItemsAction?.continuationItems.FirstOrDefault().itemSectionRenderer?
                    .contents?.Where(item => item.videoRenderer != null)?.ToList();

                continuation = youtubeSearch?.onResponseReceivedCommands?.FirstOrDefault()
                    .appendContinuationItemsAction?.continuationItems?.Where(item => item.continuationItemRenderer != null)?
                    .FirstOrDefault()?.continuationItemRenderer?
                    .continuationEndpoint?.continuationCommand?.token;
            }
            return Json( 
                new { 
                    videos = videoContents, 
                    continuation
                }, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        public IActionResult GetTopTrendingVideos(string param)
        {
            var httpRequest = new HttpRequest();
            var response = httpRequest.Send("https://www.youtube.com/youtubei/v1/browse?key=AIzaSyAO_FJ2SlqU8Q4STEHLGCilw_Y9_11qcW8", false,
                                    @"{ ""context"": { ""client"": { ""hl"": ""vi"", ""clientName"": ""WEB"", ""clientVersion"": ""2.20201220.08.00"" } }, ""browseId"": ""FEtrending"", ""params"": """ + param + @""" }", true);
            var youtubeSearch = JsonConvert.DeserializeObject<YoutubeSearchResultModel>(response);
            var a = youtubeSearch.contents.twoColumnBrowseResultsRenderer.tabs.First;
            var videoContents = new List<Content2>();
            //if (youtubeSearch?.contents != null)
            //{
            //    videoContents = youtubeSearch?.contents?.twoColumnBrowseResultsRenderer?
            //        .tabs?.FirstOrDefault()?.tabRenderer?.content?.sectionListRenderer?.contents?
            //        .FirstOrDefault().itemSectionRenderer?.contents?.Where(item => item.videoRenderer != null)?.ToList();

            //}
            return Json(
                new
                {
                    videos = youtubeSearch.contents.twoColumnBrowseResultsRenderer.tabs[0].tabRenderer.content.sectionListRenderer.contents[0].itemSectionRenderer.contents[0].shelfRenderer.content.expandedShelfContentsRenderer.items
                }, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }
    }
}
