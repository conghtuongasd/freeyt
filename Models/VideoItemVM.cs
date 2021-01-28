using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoLibrary;

namespace YouYou.Models
{
    public class VideoItemVM
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        public string Poster { get; set; }
        public string Duration { get; set; }
        public string Views { get; set; }
        public List<YouTubeVideo> Qualities { get; set; }
        public List<VideoItemVM> NextVideos { get; set; }
    }
}
