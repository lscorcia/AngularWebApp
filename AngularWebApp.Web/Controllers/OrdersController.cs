using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AngularWebApp.Backend.Orders.Models;
using AngularWebApp.Backend.Orders.Services;
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
        private readonly ILogger<OrdersController> log;
        private readonly OrdersService ordersService;

        public OrdersController(ILogger<OrdersController> _log, 
            OrdersService _ordersService)
        {
            log = _log;
            ordersService = _ordersService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<GetOrdersOutputDto>> List()
        {
            log.LogInformation("Retrieving orders...");
            return await ordersService.GetOrders();
        }
    }
}