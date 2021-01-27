namespace YoutubeGetLink.Models
{
    public class Client
    {
        public string hl { get; set; } = "vi";
        public string gl { get; set; } = "VN";
        public string clientName { get; set; } = "WEB";
        public string clientVersion { get; set; } = "2.20201030.01.00";
    }

    public class Context
    {
        public Client client { get; set; } = new Client();
    }


    public class YoutubeSearchModel
    {
        public Context context { get; set; } = new Context();
        public string query { get; set; }
        public string continuation { get; set; } = null;
    }
}
