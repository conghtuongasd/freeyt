using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoutubeGetLink.Models.NhacCuaTui
{
    public class Singer
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class Song
    {
        [JsonProperty("singer")]
        public List<Singer> Singer { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("singer_string")]
        public string SingerString => string.Join(", ", Singer.Select(item => item.Name));
    }

    public class Data
    {
        [JsonProperty("song")]
        public List<Song> Song { get; set; }
    }

    public class NhacCuaTuiSearchResultModel
    {
        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("error_code")]
        public int ErrorCode { get; set; }
    }

}
