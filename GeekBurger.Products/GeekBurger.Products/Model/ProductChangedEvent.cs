using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using GeekBurger.Products.Contract;

namespace GeekBurger.Products.Model
{
    public class ProductChangedEvent
    {
        [Key]
        public Guid EventId { get; set; }
        public ProductState State { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public bool MessageSent { get; set; }
    }
}
