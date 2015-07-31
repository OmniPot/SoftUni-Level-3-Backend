namespace ProductsShop.Client
{
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Data;
    using Models;
    using Newtonsoft.Json;

    public class ProductsShopStart
    {
        public static void Main()
        {
            var context = new ProductsShopContext();

            ExportFirstQueryResult(context, 500, 1000);

            ExportSuccessfullySoldProducts(context);

            CategoriesByProductsCount(context);

            UsersBySoldProducts(context);
        }

        private static void ExportFirstQueryResult(ProductsShopContext context, decimal minPrice, decimal maxPrice)
        {
            var result =
                from product in context.Products
                where product.Price <= maxPrice && product.Price >= minPrice
                orderby product.Price
                select new
                {
                    product.Name,
                    product.Price,
                    FullName = product.Seller.FirstName + " " + product.Seller.LastName
                };

            var resultText = JsonConvert.SerializeObject(result.ToList());
            File.WriteAllText("../../../_1_Products-in-price-range.json", resultText);
        }

        private static void ExportSuccessfullySoldProducts(ProductsShopContext context)
        {
            var result =
                from user in context.Users
                where user.ProductsSold.Any(sp => sp.Buyer != null)
                orderby user.LastName, user.FirstName
                select new
                {
                    user.FirstName,
                    user.LastName,
                    SoldProducts = user.ProductsSold
                        .Select(p => new
                        {
                            p.Name,
                            p.Price,
                            p.Buyer.FirstName,
                            p.Buyer.LastName
                        })
                };

            var resultText = JsonConvert.SerializeObject(result.ToList());
            File.WriteAllText("../../../_2_Successfully-sold-products.json", resultText);
        }

        private static void CategoriesByProductsCount(ProductsShopContext context)
        {
            var result =
                from category in context.Categories
                orderby category.Products.Count
                select new
                {
                    category.Name,
                    category.Products.Count,
                    AveragePrice = category.Products.Average(p => p.Price),
                    TotalRavenue = category.Products.Sum(p => p.Price)
                };

            var resultText = JsonConvert.SerializeObject(result.ToList());
            File.WriteAllText("../../../_3_Categories=by-products-count.json", resultText);
        }

        private static void UsersBySoldProducts(ProductsShopContext context)
        {
            var result =
                from user in context.Users
                where user.ProductsSold.Count >= 2
                orderby user.ProductsSold.Count descending, user.LastName ascending
                select new
                {
                    user.FirstName,
                    user.LastName,
                    user.Age,
                    SoldProducts = user.ProductsSold.Select(p => new
                    {
                        p.Name,
                        p.Price
                    })
                };

            var doc = new XDocument(new XElement("users", new XAttribute("count", result.Count())));
            doc.Declaration = new XDeclaration("1.0", "utf-8", null);

            foreach (var user in result)
            {
                var userEl = new XElement("user", new XAttribute("last-name", user.LastName));
                if (user.FirstName != null)
                {
                    userEl.Add(new XAttribute("first-name", user.FirstName));
                }
                if (user.Age != null)
                {
                    userEl.Add(new XAttribute("age", user.Age));
                }

                var userSoldProducts = new XElement("sold-products",
                    new XAttribute("count", user.SoldProducts.Count()));

                foreach (var product in user.SoldProducts)
                {
                    userSoldProducts.Add(new XElement("product",
                            new XAttribute("name", product.Name),
                            new XAttribute("price", product.Price)));
                }

                userEl.Add(userSoldProducts);
                doc.Root.Add(userEl);
            }

            doc.Save("../../../_4_Users-by-products-sold.xml");
        }
    }
}
