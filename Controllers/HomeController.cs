using System.Diagnostics;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Core.Services;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Core.Controllers
{
    public class HomeController : Controller
    {
        [BindProperty]
        public IndexViewModel Order { get; set; }

        public IActionResult Index(IndexViewModel model, PizzaService service) {
            model.Pizzas = service.GetPizzaIEnumerable();
            model.Orders = service.GetOrders();

            model.BasicToppings = service.GetBasicToppings();
            model.DeluxeToppings = service.GetDeluxeToppings();

            return View(model);
        }

        [HttpPost]
        public IActionResult SaveOrder() {
            OrderObject obj = JsonConvert.DeserializeObject<OrderObject>(Order.Json);
            Orders order = new Orders {
                SubTotal = obj.amount,
                GST = obj.amount * 0.05
            };

            order.Total = order.GST + order.SubTotal;
            order.Save();

            foreach (var item in obj.items) {
                OrdersDetails details = new OrdersDetails {
                    Order = order,
                    Pizza = new PizzaService().GetPizza(item.pizza),
                    Price = item.amount
                };

                details.Save();

                foreach (var topping in item.basic) {
                    OrdersBasicToppings basic = new OrdersBasicToppings { 
                        Details = details,
                        Topping = new BasicToppings(topping.id),
                        Price = topping.amount
                    };

                    basic.Save();
                }

                foreach (var topping in item.deluxe) {
                    OrdersDeluxeToppings deluxe = new OrdersDeluxeToppings {
                        Details = details,
                        Topping = new DeluxeToppings(topping.id),
                        Price = topping.amount
                    };

                    deluxe.Save();
                }
            }


            return LocalRedirect("/");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
