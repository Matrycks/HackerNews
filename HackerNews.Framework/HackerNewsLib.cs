using HackerNews.Framework.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HackerNews.Framework
{
    public class HackerNewsLib
    {
        private const string BaseUrl = "https://hacker-news.firebaseio.com/v0/{0}.json?print=pretty";

        public static async Task<List<int>> GetBestStoryIds()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(BaseUrl, "beststories"));

            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            string responseMessage = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                responseMessage = sr.ReadToEnd();
            }

            List<int> idList = new List<int>();
            dynamic ids = JsonConvert.DeserializeObject(responseMessage);

            foreach(int id in ids)
            {
                idList.Add(id);
            }

            return idList;
        }

        public static async Task<List<Story>> GetBestStories()
        {
            List<int> storyIds = await GetBestStoryIds();
            List<Story> stories = new List<Story>();

            //could use parallelism
            foreach(var id in storyIds)
            {
                var story = await GetStoryInfo(id);
                stories.Add(story);
            }

            return stories;
        }

        public static async Task<Story> GetStoryInfo(int Id)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(BaseUrl, "item/" + Id));

            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            string responseMessage = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                responseMessage = sr.ReadToEnd();
            }

            dynamic jsonObj = JsonConvert.DeserializeObject(responseMessage);
            
            int id = Convert.ToInt32(jsonObj.id);
            string by = jsonObj.by;
            int score = Convert.ToInt32(jsonObj.score);
            string title = jsonObj.title;
            string url = jsonObj.url;

            return new Story(id, by, score, title, url);
        }
    }
}
