using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Core.Extensions;
using Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Core.Services
{
    public class PizzaService
    {
        public Pizza GetPizza(string name) {
            Pizza pizza = null;

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT pz_id, pz_name, pz_price FROM pizza WHERE pz_name LIKE '" + name + "'");
            if (dr.Read()) {
                pizza = new Pizza {
                    Id = Convert.ToInt64(dr[0]),
                    Name = dr[1].ToString(),
                    Price = Convert.ToDouble(dr[2])
                };
            }

            return pizza;
        }

        public List<SelectListItem> GetIEnumerable(string query) {
            List<SelectListItem> ienumarable = new List<SelectListItem>();
            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect(query);
            if (dr.HasRows) {
                while (dr.Read()) {
                    ienumarable.Add(new SelectListItem {
                        Value = dr[0].ToString(),
                        Text = dr[1].ToString()
                    });
                }
            }

            return ienumarable;
        }

        public List<SelectListItem> GetPizzaIEnumerable() {
            return GetIEnumerable("SELECT pz_price, pz_name FROM pizza ORDER BY pz_price");
        }

        public List<BasicToppings> GetBasicToppings() {
            List<BasicToppings> toppings = new List<BasicToppings>();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT tp_idnt, tp_name, tp_small, tp_medium, tp_large FROM toppings_basic ORDER BY tp_idnt");
            if (dr.HasRows) {
                while (dr.Read()) {
                    toppings.Add(new BasicToppings {
                        Id = Convert.ToInt64(dr[0]),
                        Name = dr[1].ToString(),
                        Small = Convert.ToDouble(dr[2]),
                        Medium = Convert.ToDouble(dr[3]),
                        Large = Convert.ToDouble(dr[4])
                    });
                }
            }

            return toppings;
        }

        public List<DeluxeToppings> GetDeluxeToppings() {
            List<DeluxeToppings> toppings = new List<DeluxeToppings>();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT dt_idnt, dt_name, dt_small, dt_medium, dt_large FROM toppings_deluxe ORDER BY dt_idnt");
            if (dr.HasRows) {
                while (dr.Read()) {
                    toppings.Add(new DeluxeToppings {
                        Id = Convert.ToInt64(dr[0]),
                        Name = dr[1].ToString(),
                        Small = Convert.ToDouble(dr[2]),
                        Medium = Convert.ToDouble(dr[3]),
                        Large = Convert.ToDouble(dr[4])
                    });
                }
            }

            return toppings;
        }

        public List<OrdersViewModel> GetOrders()
        {
            List<OrdersViewModel> orders = new List<OrdersViewModel>();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT ord_idnt, ord_date, ord_sub_total, ord_gst, ord_total, ord_notes, SUBSTRING ((SELECT ', ' + pz_name+CASE WHEN COUNT(*)>1 THEN '('+CAST(COUNT(*)AS NVARCHAR)+')' ELSE '' END FROM orders_details INNER JOIN pizza ON od_pizza=pz_id WHERE ord_idnt=od_order GROUP BY pz_name, od_order ORDER BY pz_name FOR XML PATH('')), 3, 8000) pizza ,SUBSTRING ((SELECT ', ' + tp_name+CASE WHEN COUNT(*)>1 THEN '('+CAST(COUNT(*)AS NVARCHAR)+')' ELSE '' END FROM vToppings INNER JOIN orders_details ON odtb_order_detail=od_idnt WHERE ord_idnt=od_order GROUP BY tp_name, od_order ORDER BY tp_name FOR XML PATH('')), 3, 8000) topping FROM orders");
            if (dr.HasRows) {
                while (dr.Read()) {
                    orders.Add(new OrdersViewModel {
                        Order = new Orders {
                            Id = Convert.ToInt64(dr[0]),
                            Date = Convert.ToDateTime(dr[1]),
                            SubTotal = Convert.ToDouble(dr[2]),
                            GST = Convert.ToDouble(dr[3]),
                            Total = Convert.ToDouble(dr[4]),
                            Narration = dr[5].ToString()
                        },
                        Pizza = dr[6].ToString(),
                        Topping = dr[7].ToString(),
                    });
                }
            }

            return orders;
        }


        public Orders SaveOrder(Orders order) {
            SqlServerConnection conn = new SqlServerConnection();
            order.Id = conn.SqlServerUpdate("DECLARE @idnt INT=" + order.Id + ", @date DATE='" + order.Date + "', @subs FLOAT=" + order.SubTotal + ", @gsts FLOAT=" + order.GST + ", @amts FLOAT=" + order.Total + ", @desc NVARCHAR(MAX)='" + order.Narration + "'; IF NOT EXISTS (SELECT ord_idnt FROM orders WHERE ord_idnt=@idnt) BEGIN INSERT INTO orders (ord_date, ord_sub_total, ord_gst, ord_total, ord_notes) output INSERTED.ord_idnt VALUES (@date, @subs, @gsts, @amts, @desc) END ELSE BEGIN UPDATE orders SET ord_date=@date, ord_sub_total=@subs, ord_gst=@gsts, ord_total=@amts, ord_notes=@desc output INSERTED.ord_idnt WHERE ord_idnt=@idnt END");

            return order;
        }

        public OrdersDetails SaveOrdersDetails(OrdersDetails detail) {
            SqlServerConnection conn = new SqlServerConnection();
            detail.Id = conn.SqlServerUpdate("DECLARE @idnt INT=" + detail.Id + ", @ords INT='" + detail.Order.Id + "', @pizz INT=" + detail.Pizza.Id + ", @amts FLOAT=" + detail.Price + "; IF NOT EXISTS (SELECT od_idnt FROM orders_details WHERE od_idnt=@idnt) BEGIN INSERT INTO orders_details (od_order, od_pizza, od_price) output INSERTED.od_idnt VALUES (@ords, @pizz, @amts) END ELSE BEGIN UPDATE orders_details SET od_order=@ords, od_pizza=@pizz, od_price=@amts output INSERTED.od_idnt WHERE od_idnt=@idnt END");

            return detail;
        }

        public OrdersBasicToppings SaveOrdersBasicToppings(OrdersBasicToppings topping)
        {
            SqlServerConnection conn = new SqlServerConnection();
            topping.Id = conn.SqlServerUpdate("DECLARE @idnt INT=" + topping.Id + ", @dtls INT='" + topping.Details.Id + "', @topp INT=" + topping.Topping.Id + ", @amts FLOAT=" + topping.Price + "; IF NOT EXISTS (SELECT odtb_idnt FROM orders_details_toppings_basic WHERE odtb_idnt=@idnt) BEGIN INSERT INTO orders_details_toppings_basic (odtb_order_detail, odtb_topping, odtb_price) output INSERTED.odtb_idnt VALUES (@dtls, @topp, @amts) END ELSE BEGIN UPDATE orders_details_toppings_basic SET odtb_order_detail=@dtls, odtb_topping=@topp, odtb_price=@amts output INSERTED.odtb_idnt WHERE odtb_idnt=@idnt END");

            return topping;
        }

        public OrdersDeluxeToppings SaveOrdersDeluxeToppings(OrdersDeluxeToppings topping)
        {
            SqlServerConnection conn = new SqlServerConnection();
            topping.Id = conn.SqlServerUpdate("DECLARE @idnt INT=" + topping.Id + ", @dtls INT='" + topping.Details.Id + "', @topp INT=" + topping.Topping.Id + ", @amts FLOAT=" + topping.Price + "; IF NOT EXISTS (SELECT odtd_idnt FROM orders_details_toppings_deluxe WHERE odtd_idnt=@idnt) BEGIN INSERT INTO orders_details_toppings_deluxe (odtd_order_detail, odtd_topping, odtd_price) output INSERTED.odtd_idnt VALUES (@dtls, @topp, @amts) END ELSE BEGIN UPDATE orders_details_toppings_deluxe SET odtd_order_detail=@dtls, odtd_topping=@topp, odtd_price=@amts output INSERTED.odtd_idnt WHERE odtd_idnt=@idnt END");

            return topping;
        }
    }
}
