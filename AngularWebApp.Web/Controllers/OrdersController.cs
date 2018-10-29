using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AngularWebApp.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<OrdersController> _log;

        public OrdersController(ILogger<OrdersController> log, IConfiguration config)
        {
            _config = config;
            _log = log;
        }

        [HttpGet]
        [Authorize]
        public IEnumerable<Order> List()
        {
            _log.LogWarning("Config key {0} = {1}", "ConfigKey1", _config.GetValue<string>("ConfigKey1"));
            _log.LogWarning("Config key {0} = {1}", "ConfigKey2", _config.GetValue<string>("ConfigKey2"));
            _log.LogInformation("Retrieving orders...");

            return Order.CreateOrders();
        }
    }

    #region Helpers
    public class Order
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
        public string ShipperCity { get; set; }
        public Boolean IsShipped { get; set; }

        public static List<Order> CreateOrders()
        {
            List<Order> OrderList = new List<Order>
            {
                new Order {OrderID = 10248, CustomerName = "Taiseer Joudeh", ShipperCity = "Amman", IsShipped = true },
                new Order {OrderID = 10249, CustomerName = "Ahmad Hasan", ShipperCity = "Dubai", IsShipped = false},
                new Order {OrderID = 10250,CustomerName = "Tamer Yaser", ShipperCity = "Jeddah", IsShipped = false },
                new Order {OrderID = 10251,CustomerName = "Lina Majed", ShipperCity = "Abu Dhabi", IsShipped = false},
                new Order {OrderID = 10252,CustomerName = "Yasmeen Rami", ShipperCity = "Kuwait", IsShipped = true}
            };

            return OrderList;
        }
    }
    #endregion
}