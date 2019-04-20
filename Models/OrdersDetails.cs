using System;
using Core.Services;

namespace Core.Models
{
    public class OrdersDetails
    {
        public long Id { get; set; }
        public Orders Order { get; set; }
        public Pizza Pizza { get; set; }
        public double Price { get; set; }

        public OrdersDetails() {
            Id = 0;
            Price = 0;

            Order = new Orders();
            Pizza = new Pizza();
        }

        public OrdersDetails Save() {
            return new PizzaService().SaveOrdersDetails(this);
        }
    }
}
