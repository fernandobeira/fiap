using System;
using System.Collections.Generic;
using System.Text;

namespace GeekBurger.Products.Contract
{
    public class ItemToGet
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; }
    }

}
