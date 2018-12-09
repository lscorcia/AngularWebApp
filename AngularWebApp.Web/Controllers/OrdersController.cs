using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AngularWebApp.Backend.Orders.Models;
using AngularWebApp.Backend.Orders.Services;
using AngularWebApp.Infrastructure.Web.Binders;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IEnumerable<GetOrdersOutputDto>> List()
        {
            log.LogInformation("Retrieving orders...");
            return await ordersService.GetOrders();
        }

        [HttpGet]
        public LoadResult Orders(DataSourceLoadOptions loadOptions)
        {
            log.LogInformation("Retrieving orders...");
            var resp = ordersService.GetOrders(loadOptions);
            return resp;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetOrdersOutputDto>> Get(int id)
        {
            var item = await ordersService.Get(id);
            if (item == null)
                return NotFound();

            return item;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<int>> Add(AddOrderInputDto model)
        {
            log.LogInformation("Adding order for customer {0}", model.CustomerName);

            var orderId = await ordersService.AddOrder(model);

            return CreatedAtAction(nameof(Get), new { id = orderId }, new { id = orderId });
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Edit(EditOrderInputDto model)
        {
            log.LogInformation("Editing order for customer {0} - new customer = {1}", model.CustomerName, model.CustomerName);

            await ordersService.EditOrder(model);

            return Ok();
        }
        
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(int id)
        {
            log.LogInformation("Deleting order {0}", id);

            await ordersService.Delete(id);

            return Ok();
        }
    }
}