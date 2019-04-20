using System;
namespace Core.Models
{
    public class Pizza
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        
        public Pizza() {
            Id = 0;
            Name = "";
            Price = 0;
        }
    }
}
