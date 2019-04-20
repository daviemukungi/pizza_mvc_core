using System;
namespace Core.Models
{
    public class BasicToppings
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Small { get; set; }
        public double Medium { get; set; }
        public double Large { get; set; }

        public BasicToppings() {
            Id = 0;
            Name = "";
            Small = 0;
            Medium = 0;
            Large = 0;
        }

        public BasicToppings(long idnt) : this() {
            Id = idnt;
        }
    }
}
