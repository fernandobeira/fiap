using System;
using System.Collections.Generic;
using System.Text;

namespace GeekBurger.Products.Contract
{
    public class ProductChangedMessage
    {
        public ProductState State { get; set; }
        public ProductToGet Product { get; set; }
    }
}
