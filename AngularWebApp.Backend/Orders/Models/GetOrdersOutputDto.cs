using System;
using System.Collections.Generic;
using System.Text;

namespace AngularWebApp.Backend.Orders.Models
{
    public class GetOrdersOutputDto
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
        public string ShipperCity { get; set; }
        public Boolean IsShipped { get; set; }
    }
}
