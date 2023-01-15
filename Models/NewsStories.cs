using System;
using System.Collections.Generic;
namespace Models
{
    public class Source
    {
        public object id { get; set; }
        public string name { get; set; }
    }

    public class Article
    {
        public Source source { get; set; }
        public string author { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string urlToImage { get; set; }
        public DateTime publishedAt { get; set; }
        public string content { get; set; }
    }

    public class ArticleList
    {
        public IList<Article> articles { get; set; }
    }
}
