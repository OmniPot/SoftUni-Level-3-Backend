namespace News.WebServices.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using Data;
    using Models;
    using News.Models;

    [RoutePrefix("api/news")]
    public class NewsController : ApiController
    {
        public NewsController()
            : this(new NewsData(new NewsContext()))
        {
        }

        public NewsController(INewsData data)
        {
            this.Data = data;
        }

        private INewsData Data { get; set; }

        [HttpGet]
        [Route]
        public IHttpActionResult GetAllNews()
        {
            var news = this.Data.News.All()
                .OrderByDescending(n => n.PublishDate)
                .Select(NewsViewModel.Create);

            if (!news.Any())
            {
                return this.Ok("No news to preview.");
            }

            return this.Ok(news);
        }

        [HttpPost]
        [Route]
        public IHttpActionResult CreateNews(NewsBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("No data provide to create news.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest("Invalid news info.");
            }

            var newsToAdd = new News
            {
                Title = model.Title,
                Content = model.Content,
                PublishDate = DateTime.Now
            };

            this.Data.News.Add(newsToAdd);
            this.Data.SaveChanges();

            var newsView = NewsViewModel.CreateMethod(newsToAdd);

            return this.Ok(newsView);
        }

        [HttpPut]
        [Route("{newsId}")]
        public IHttpActionResult UpdateNews(int newsId, NewsBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("No data provide to update news.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest("Invalid news info.");
            }

            var newsToUpdate = this.Data.News.Find(newsId);

            if (newsToUpdate == null)
            {
                return this.BadRequest("No such news.");
            }

            newsToUpdate.Title = model.Title;
            newsToUpdate.Content = model.Content;
            if (model.PublishDate.HasValue)
            {
                newsToUpdate.PublishDate = model.PublishDate.Value;
            }


            this.Data.News.Update(newsToUpdate);
            this.Data.SaveChanges();

            var newsView = NewsViewModel.CreateMethod(newsToUpdate);

            return this.Ok(newsView);
        }

        [HttpDelete]
        [Route("{newsId}")]
        public IHttpActionResult DeleteNews(int newsId)
        {
            var newsToDelete = this.Data.News.Find(newsId);

            if (newsToDelete == null)
            {
                return this.BadRequest("No such news.");
            }

            this.Data.News.Delete(newsToDelete);
            this.Data.SaveChanges();

            return this.Ok("Successfully delete news.");
        }
    }
}