using GeekBurger.Products.Model;
using System;
using System.Collections.Generic;

namespace GeekBurger.Products.Repository
{
    public interface IProductsRepository
    {
        Product GetProductById(Guid productId);
        IEnumerable<Product> GetProductsByStoreName(string storeName);
        List<Item> GetFullListOfItems();
        bool Add(Product product);
        void Save();
    }
}
