using System;
using System.Collections.Generic;
using System.Text;

namespace GeekBurger.Products.Contract.Models
{
    public class Product
    {
        public string StoreName { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public List<Item> Ingredients { get; set; }
        public decimal Price { get; set; }
    }

}
