using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Hosting;
using GeekBurger.Products.Model;

namespace GeekBurger.Products.Service
{
    public interface IProductChangedService : IHostedService
    {
        void SendMessagesAsync();
        void AddToMessageList(IEnumerable<EntityEntry<Product>> changes);
    }
}
