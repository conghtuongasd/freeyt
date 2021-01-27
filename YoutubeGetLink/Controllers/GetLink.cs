using GetLinkYoutube;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YoutubeExtractorCore;
using YoutubeGetLink.LIB;
using YoutubeGetLink.Models;

namespace YoutubeGetLink.Controllers
{
    public class GetLink : Controller
    {
        public async Task<IActionResult> Index(string url = "")
        {
            try
            {
                var a = YoutubeLink.GetLink(url);
                return View(new VideoModel { 
                    Url = a,
                    IsSuccess = true
                });
            }
            catch (System.Exception e)
            {
                return View(new VideoModel
                {
                    IsSuccess = false
                });
            }
        }
    }
}
