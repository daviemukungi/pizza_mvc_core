using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Core.Models
{
    public class IndexViewModel
    {
        public Pizza Pizza { get; set; }
        public string Json { get; set; }

        public List<BasicToppings> BasicToppings { get; set; }
        public List<DeluxeToppings> DeluxeToppings { get; set; }

        public List<SelectListItem> Pizzas { get; set; }
        public List<OrdersViewModel> Orders { get; set; }

        public IndexViewModel() {
            Pizza = new Pizza();
            Json = "";

            Orders = new List<OrdersViewModel>();
            Pizzas = new List<SelectListItem>();

            BasicToppings = new List<BasicToppings>();
            DeluxeToppings = new List<DeluxeToppings>();
        }
    }
}
