namespace OnlineShop.Services.Models.BindingModels
{
    using System.Collections.Generic;

    public class AdBindingModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int TypeId { get; set; }

        public decimal Price { get; set; }

        public IEnumerable<int> Categories { get; set; }
    }
}