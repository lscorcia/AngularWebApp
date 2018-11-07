using System;
using System.ComponentModel.DataAnnotations;

namespace AngularWebApp.Backend.Orders.Repository
{
    class Order
    {
        [Key()]
        public int OrderID { get; set; }

        [Required()]
        [MaxLength(255)]
        public string CustomerName { get; set; }

        [MaxLength(255)]
        public string ShipperCity { get; set; }

        public bool IsShipped { get; set; }
    }
}