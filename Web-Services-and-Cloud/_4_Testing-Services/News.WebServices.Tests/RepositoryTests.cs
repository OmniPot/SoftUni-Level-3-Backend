namespace News.WebServices.Tests
{
    using System;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Transactions;
    using Data;
    using Data.Repositories;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using News.Models;

    [TestClass]
    public class RepositoryTests
    {
        private static TransactionScope tran;

        [TestInitialize]
        public void TestInit()
        {
            tran = new TransactionScope();
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void GetAllNews_Should_Return_All_News_From_The_Repository()
        {
            var repository = new GenericRepository<News>(new NewsContext());

            var newsCount = repository.All().Count();
            var news = repository.All();
            var actualResult = news.Count();

            Assert.AreEqual(newsCount, actualResult);
        }

        [TestMethod]
        public void AddNews_Should_Successfully_Add_Specified_News_To_The_Repository()
        {
            var repository = new GenericRepository<News>(new NewsContext());
            var news = new News
            {
                Title = "Sample Title",
                Content = "Sample Content",
                PublishDate = DateTime.Now
            };

            repository.Add(news);
            repository.SaveChanges();

            var newsFromDb = repository.Find(news.Id);

            Assert.IsNotNull(newsFromDb);
            Assert.AreEqual(news.Content, newsFromDb.Content);
            Assert.AreEqual(news.Title, newsFromDb.Title);
            Assert.AreEqual(news.PublishDate, newsFromDb.PublishDate);
            Assert.IsTrue(newsFromDb.Id != 0);
        }

        [TestMethod]
        [ExpectedException(typeof (DbEntityValidationException))]
        public void AddNews_Should_Throw_Exception_If_Data_Is_Invalid()
        {
            var repository = new GenericRepository<News>(new NewsContext());
            var news = new News
            {
                Title = "shor",
                Content = "Sample Content",
                PublishDate = DateTime.Now
            };

            repository.Add(news);
            repository.SaveChanges();
        }

        [TestMethod]
        public void UpdateNews_Should_Successfully_Update_News()
        {
            var repository = new GenericRepository<News>(new NewsContext());

            var newsToUpdate = repository.All().FirstOrDefault();
            if (newsToUpdate == null)
            {
                Assert.Fail("Cannot run test - no news in the repository");
            }

            var newsModel = new NewsBindingModel
            {
                Title = "Updated Sample title.",
                Content = "Updated Sample content",
                PublishDate = DateTime.Now
            };

            newsToUpdate.Title = newsModel.Title;
            newsToUpdate.Content = newsModel.Content;
            if (newsModel.PublishDate.HasValue)
            {
                newsToUpdate.PublishDate = newsModel.PublishDate.Value;
            }

            repository.Update(newsToUpdate);
            repository.SaveChanges();
        }

        [TestMethod]
        [ExpectedException(typeof (DbEntityValidationException))]
        public void UpdateNews_Should_Throw_Exeption_If_Data_Is_Invalid()
        {
            var repository = new GenericRepository<News>(new NewsContext());

            var newsToUpdate = repository.All().FirstOrDefault();
            if (newsToUpdate == null)
            {
                Assert.Fail("Cannot run test - no news in the repository");
            }

            var newsModel = new NewsBindingModel
            {
                Title = "Upda",
                Content = "Upda"
            };

            newsToUpdate.Title = newsModel.Title;
            newsToUpdate.Content = newsModel.Content;
            if (newsModel.PublishDate.HasValue)
            {
                newsToUpdate.PublishDate = newsModel.PublishDate.Value;
            }

            repository.Update(newsToUpdate);
            repository.SaveChanges();
        }

        [TestMethod]
        [ExpectedException(typeof (DbUpdateConcurrencyException))]
        public void UpdateNews_Should_Throw_Exeption_If_Object_Of_Update_Is_Invalid()
        {
            var repository = new GenericRepository<News>(new NewsContext());

            var newsModel = new News
            {
                Title = "Update aaa",
                Content = "Update aaa",
                PublishDate = DateTime.Now
            };

            repository.Update(newsModel);
            repository.SaveChanges();
        }

        [TestMethod]
        public void DeleteNews_Should_Successfully_Delete_Object_If_It_Exists()
        {
            var repository = new GenericRepository<News>(new NewsContext());

            var newsModel = repository.All().FirstOrDefault();
            if (newsModel == null)
            {
                Assert.Fail("Cannot perform test - Unable to delete unexisting object.");
            }

            repository.Delete(newsModel);
            repository.SaveChanges();
        }

        [TestMethod]
        [ExpectedException(typeof (DbUpdateConcurrencyException))]
        public void DeleteNews_Should_Throw_Exeption_If_Object_Of_Deletion_Is_Invalid()
        {
            var repository = new GenericRepository<News>(new NewsContext());

            var newsToDelete = new News
            {
                Title = "Update aaa",
                Content = "Update aaa",
                PublishDate = DateTime.Now
            };

            repository.Delete(newsToDelete);
            repository.SaveChanges();
        }
    }
}