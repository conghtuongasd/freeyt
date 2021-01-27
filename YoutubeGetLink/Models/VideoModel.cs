using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoutubeGetLink.Models
{
    public class VideoModel
    {
        public bool IsSuccess { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public List<YoutubeVideoModel> YoutubeVideos { get; set; }
    }

    public class YoutubeVideoModel
    {
        public int Resolution { get; set; }
        public string Url { get; set; }
    }
}
