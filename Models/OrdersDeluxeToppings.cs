using System;
using Core.Services;

namespace Core.Models
{
    public class OrdersDeluxeToppings
    {
        public long Id { get; set; }
        public double Price { get; set; }

        public OrdersDetails Details { get; set; }
        public DeluxeToppings Topping { get; set; }

        public OrdersDeluxeToppings() {
            Id = 0;
            Price = 0;

            Details = new OrdersDetails();
            Topping = new DeluxeToppings();
        }

        public OrdersDeluxeToppings Save() {
            return new PizzaService().SaveOrdersDeluxeToppings(this);
        }
    }
}
