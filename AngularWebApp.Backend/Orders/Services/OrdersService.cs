using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularWebApp.Backend.Orders.Models;
using AngularWebApp.Backend.Orders.Repository;
using AngularWebApp.Infrastructure.DI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AngularWebApp.Backend.Orders.Services
{
    public class OrdersService : IApplicationService
    {
        private IConfiguration Configuration { get; }
        private string _connectionString { get { return Configuration.GetConnectionString("OrdersDbContext"); } }

        public OrdersService(IConfiguration _config)
        {
            this.Configuration = _config;
        }

        public async Task<List<GetOrdersOutputDto>> GetOrders()
        {
            OrdersDbContext dbContext = new OrdersDbContext() { ConnectionString = _connectionString };

            return await dbContext.Orders
                .Select(t => new GetOrdersOutputDto()
                {
                    OrderID = t.OrderID,
                    CustomerName = t.CustomerName,
                    ShipperCity = t.ShipperCity,
                    IsShipped = t.IsShipped
                })
                .ToListAsync();
        }
    }
}
