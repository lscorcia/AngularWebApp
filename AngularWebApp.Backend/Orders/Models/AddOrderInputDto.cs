using System;

namespace AngularWebApp.Backend.Orders.Models
{
    public class AddOrderInputDto
    {
        public string CustomerName { get; set; }
        public string ShipperCity { get; set; }
        public bool IsShipped { get; set; }
    }
}