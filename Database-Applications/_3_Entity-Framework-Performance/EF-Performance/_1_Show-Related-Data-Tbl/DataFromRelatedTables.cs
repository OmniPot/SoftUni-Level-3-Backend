namespace _1_Show_Related_Data_Tbl
{
    using System;
    using System.Linq;
    using Ads.Data;

    public class DataFromRelatedTables
    {
        public static void Main()
        {
            var ctx = new AdsEntities();

            // No Include
            GetAllAdsWithoutInclude(ctx);

            // With Include
            GetAllAdsWithInclude(ctx);
        }

        private static void GetAllAdsWithInclude(AdsEntities ctx)
        {
            var ads = ctx.Ads
                .Select(a => new
                {
                    a.Title,
                    TownName = a.Town.Name,
                    Category = a.Category.Name,
                    a.AdStatus.Status,
                    a.AspNetUser.UserName
                }).ToList();
        }

        private static void GetAllAdsWithoutInclude(AdsEntities ctx)
        {
            var ads = ctx.Ads.ToList();

            foreach (var ad in ads)
            {
                Console.WriteLine("Title: {0}\nStatus: {1}\nCategory: {2}\nTown: {3}\nUser: {4}\n",
                    ad.Title,
                    ad.AdStatus.Status,
                    ad.Category != null ? ad.Category.Name : "No category",
                    ad.Town != null ? ad.Town.Name : "No town",
                    ad.AspNetUser.UserName);
            }
        }
    }
}
