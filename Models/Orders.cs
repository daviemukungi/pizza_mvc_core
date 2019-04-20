using System;
using Core.Services;

namespace Core.Models
{
    public class Orders
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public double SubTotal { get; set; }
        public double GST { get; set; }
        public double Total { get; set; }
        public string Narration { get; set; }

        public Orders() {
            Id = 0;
            Date = DateTime.Now;
            SubTotal = 0;
            GST = 0;
            Total = 0;
            Narration = "N/A";
        }

        public Orders Save() {
            return new PizzaService().SaveOrder(this);
        }
    }
}
