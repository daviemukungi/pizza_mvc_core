using System;
using Core.Services;

namespace Core.Models
{
    public class OrdersBasicToppings {
        public long Id { get; set; }
        public double Price { get; set; }

        public OrdersDetails Details { get; set; }
        public BasicToppings Topping { get; set; }

        public OrdersBasicToppings() {
            Id = 0;
            Price = 0;

            Details = new OrdersDetails();
            Topping = new BasicToppings();
        }

        public OrdersBasicToppings Save() {
            return new PizzaService().SaveOrdersBasicToppings(this);
        }
    }
}
