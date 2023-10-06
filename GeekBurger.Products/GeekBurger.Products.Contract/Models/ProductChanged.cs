using System;
using System.Collections.Generic;
using System.Text;

namespace GeekBurger.Products.Contract.Models
{
    public class ProductChanged
    {
        public ProductState State { get; set; }
        public Product Product { get; set; }
    }

}
