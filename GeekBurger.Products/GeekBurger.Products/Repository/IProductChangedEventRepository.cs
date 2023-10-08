using GeekBurger.Products.Model;
using System;

namespace GeekBurger.Products.Repository
{
    public interface IProductChangedEventRepository
    {
        ProductChangedEvent Get(Guid eventId);
        bool Add(ProductChangedEvent productChangedEvent);
        bool Update(ProductChangedEvent productChangedEvent);
        void Save();
    }
}
