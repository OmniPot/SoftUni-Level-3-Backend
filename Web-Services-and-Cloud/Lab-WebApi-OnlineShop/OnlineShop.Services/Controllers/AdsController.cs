namespace OnlineShop.Services.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Data;
    using Infrastructure;
    using Models.BindingModels;
    using Models.ViewModels;
    using OnlineShop.Models;

    [RoutePrefix("api/ads")]
    [Authorize]
    public class AdsController : BaseApiController
    {
        public AdsController(IOnlineShopData data, IUserIdProvider provider = null)
            : base(data, provider)
        {
        }

        // GET api/ads/all
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult All()
        {
            var ads = this.Data.Ads.All()
                .Where(a => a.ClosedOn == null)
                .OrderByDescending(a => a.Type.Index)
                .ThenByDescending(a => a.PostedOn)
                .Select(AdViewModel.ToViewModel);

            return this.Ok(ads);
        }

        // POST api/ads/create
        [HttpPost]
        public IHttpActionResult Create(AdBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("No data to create the ad with.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (model.Categories == null)
            {
                return this.BadRequest("Ad categories are required.");
            }

            if (model.Categories.Count() < 1 || model.Categories.Count() > 3)
            {
                return this.BadRequest("Ad categories count must be between 1 and 3.");
            }

            var currnetUserId = this.UserProvider.GetUserId();
            var newAd = new Ad
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                PostedOn = DateTime.Now,
                OwnerId = currnetUserId,
                Categories = new HashSet<Category>()
            };

            foreach (var categoryId in model.Categories)
            {
                var existingCategory = this.Data.Categories.Find(categoryId);
                if (existingCategory == null)
                {
                    return this.BadRequest("No category with the specified Id.");
                }

                newAd.Categories.Add(existingCategory);
            }

            var existingType = this.Data.AdTypes.Find(model.TypeId);
            if (existingType == null)
            {
                return this.BadRequest("No adtype with the specified id.");
            }

            newAd.Type = existingType;
            newAd.TypeId = existingType.Id;

            this.Data.Ads.Add(newAd);
            this.Data.SaveChanges();

            var adViewModel = this.Data.Ads.All()
                .Where(a => a.Id == newAd.Id)
                .Select(AdViewModel.ToViewModel)
                .FirstOrDefault();

            return this.Ok(adViewModel);
        }

        // PUT api/ads/{id}/close
        [HttpPut]
        public IHttpActionResult Close(int id)
        {
            var existingAd = this.Data.Ads.All().FirstOrDefault(a => a.Id == id);
            if (existingAd == null)
            {
                return this.BadRequest("No ad with the specified id.");
            }

            var userId = this.UserProvider.GetUserId();
            if (userId != existingAd.OwnerId)
            {
                return this.BadRequest("You do not have permission to close the specified ad.");
            }

            existingAd.Status = AdStatus.Closed;
            existingAd.ClosedOn = DateTime.Now;
            this.Data.SaveChanges();

            var adViewModel = this.Data.Ads.All()
                .Where(a => a.Id == existingAd.Id)
                .Select(AdViewModel.ToViewModel)
                .FirstOrDefault();

            return this.Ok(adViewModel);
        }
    }
}