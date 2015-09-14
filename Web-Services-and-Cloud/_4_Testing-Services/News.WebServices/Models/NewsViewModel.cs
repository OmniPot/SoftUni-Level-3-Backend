namespace News.WebServices.Models
{
    using System;
    using System.Linq.Expressions;
    using News.Models;

    public class NewsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime PublishDate { get; set; }

        public static Expression<Func<News, NewsViewModel>> Create
        {
            get
            {
                return n => new NewsViewModel
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    PublishDate = n.PublishDate
                };
            }
        }

        public static NewsViewModel CreateMethod(News news)
        {
            return new NewsViewModel
            {
                Id = news.Id,
                Title = news.Title,
                Content = news.Content,
                PublishDate = news.PublishDate
            };
        }
    }
}