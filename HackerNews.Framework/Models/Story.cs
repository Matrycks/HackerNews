using System;
using System.Collections.Generic;
using System.Text;

namespace HackerNews.Framework.Models
{
    public class Story
    {
        public int Id { get; set; }
        public string By { get; set; }
        public int Score { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }

        public Story() { }

        public Story(int id, string by, int score, string title, string url)
        {
            Id = id;
            By = by;
            Score = score;
            Title = title;
            Url = url;
        }
    }
}
