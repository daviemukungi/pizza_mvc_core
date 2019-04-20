using System;
namespace Core.Models
{
    public class OrdersViewModel
    {
        public Orders Order { get; set; }
        public string Pizza { get; set; }
        public string Topping { get; set; }

        public OrdersViewModel()
        {
            Order = new Orders();
            Pizza = "";
            Topping = "";
        }
    }
}
