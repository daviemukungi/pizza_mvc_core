using System;
using System.Collections.Generic;

namespace Core.Models
{
    public class OrderObject {
        public int status { get; set; }
        public double amount { get; set; }
        public List<Items> items { get; set; }
    }

    public class Items {
        public string pizza { get; set; }
        public int amount { get; set; }
        public List<Topping> basic { get; set; }
        public List<Topping> deluxe { get; set; }
    }

    public class Topping {
        public int id { get; set; }
        public double amount { get; set; }
    }
}
