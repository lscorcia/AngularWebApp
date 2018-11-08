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
            using (OrdersDbContext dbContext = new OrdersDbContext() { ConnectionString = _connectionString })
            { 
                return await dbContext.Orders
                    .Select(t => new GetOrdersOutputDto()
                    {
                        OrderId = t.OrderID,
                        CustomerName = t.CustomerName,
                        ShipperCity = t.ShipperCity,
                        IsShipped = t.IsShipped
                    })
                    .ToListAsync();
            }
        }

        public async Task<GetOrdersOutputDto> Get(int id)
        {
            using (OrdersDbContext dbContext = new OrdersDbContext() { ConnectionString = _connectionString })
            {
                var entity = await dbContext.Orders
                    .FirstOrDefaultAsync(t => t.OrderID == id);
                if (entity == null)
                    return null;

                return new GetOrdersOutputDto()
                {
                    OrderId = entity.OrderID,
                    CustomerName = entity.CustomerName,
                    ShipperCity = entity.ShipperCity,
                    IsShipped = entity.IsShipped
                };
            }
        }

        public async Task<int> AddOrder(AddOrderInputDto dto)
        {
            Order order = new Order()
            {
                CustomerName = dto.CustomerName,
                IsShipped = dto.IsShipped,
                ShipperCity = dto.ShipperCity
            };

            using (OrdersDbContext dbContext = new OrdersDbContext() {ConnectionString = _connectionString})
            {
                dbContext.Orders.Add(order);

                await dbContext.SaveChangesAsync();

                return order.OrderID;
            }
        }

        public async Task Delete(int id)
        {
            using (OrdersDbContext dbContext = new OrdersDbContext() { ConnectionString = _connectionString })
            {
                var order = await dbContext.Orders.FindAsync(id);
                if (order != null)
                    dbContext.Orders.Remove(order);

                await dbContext.SaveChangesAsync();
            }
        }

        public async Task EditOrder(EditOrderInputDto model)
        {
            using (OrdersDbContext dbContext = new OrdersDbContext() { ConnectionString = _connectionString })
            {
                var order = await dbContext.Orders.FindAsync(model.OrderId);
                if (order == null)
                    throw new Exception(String.Format("Order ID '{0}' not found!", model.OrderId));

                order.CustomerName = model.CustomerName;
                order.ShipperCity = model.ShipperCity;
                order.IsShipped = model.IsShipped;

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
