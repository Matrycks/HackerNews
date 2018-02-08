using HackerNews.Framework;
using HackerNews.Framework.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace HackerNews.Web.Controllers
{
    public class StoriesController : Controller
    {
        public ActionResult Index()
        {
            return View("List");
        }

        [HttpPost]
        public async Task<ActionResult> BestStories()
        {
            try
            {
                var stories = await GetBestStories();

                return Json(stories, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        
        protected async Task<List<Story>> GetBestStories()
        {
            List<Story> stories = null;

            if (System.Web.HttpContext.Current.Cache["BestStories"] == null)
            {
                stories = await HackerNewsLib.GetBestStories();

                System.Web.HttpContext.Current.Cache.Add("BestStories", stories, null,
                    DateTime.Now.AddMinutes(1), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            }
            else
                stories = (List<Story>)System.Web.HttpContext.Current.Cache["BestStories"];

            return stories;
        }

        [HttpPost]
        public async Task<ActionResult> Search(string value)
        {
            try
            {
                List<Story> stories = null;

                if (System.Web.HttpContext.Current.Cache["BestStories"] != null)
                    stories = (List<Story>)System.Web.HttpContext.Current.Cache["BestStories"];
                else
                    stories = await this.GetBestStories();

                var searchResults = stories.Where(s => s.Title.ToLower().Contains(value) || s.By.ToLower().Contains(value)).ToList();

                return Json(searchResults, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}