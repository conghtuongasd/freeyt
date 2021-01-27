using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoLibrary;

namespace YoutubeGetLink.LIB
{
    public class YoutubeLink
    {
        public static string GetLink(string url)
        {
            var youTube = YouTube.Default;
            var video = youTube.GetVideo(url);
            return video.Uri;
        }

        public static IEnumerable<YouTubeVideo> GetLinks(string url)
        {
            var youTube = YouTube.Default;
            var video = youTube.GetAllVideos(url);
            return video?.Where(item => item.FileExtension.Equals(".mp4") && item.Resolution > 0 && item.AudioBitrate > 0)?.OrderByDescending(item => item.Resolution);
        }
    }
}
