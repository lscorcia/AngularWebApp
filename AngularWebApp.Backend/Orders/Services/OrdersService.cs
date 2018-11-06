using System;
using System.Collections.Generic;
using AngularWebApp.Backend.Orders.Models;
using AngularWebApp.Infrastructure.DI;
using Microsoft.Extensions.Configuration;

namespace AngularWebApp.Backend.Orders.Services
{
    public class OrdersService : IApplicationService
    {
        private IConfiguration Configuration { get; }

        public OrdersService(IConfiguration _config)
        {
            this.Configuration = _config;
        }

        public List<GetOrdersOutputDto> GetOrders()
        {
            List<GetOrdersOutputDto> OrderList = new List<GetOrdersOutputDto>
            {
                new GetOrdersOutputDto {OrderID = 10248, CustomerName = "Taiseer Joudeh", ShipperCity = "Amman", IsShipped = true },
                new GetOrdersOutputDto {OrderID = 10249, CustomerName = "Ahmad Hasan", ShipperCity = "Dubai", IsShipped = false},
                new GetOrdersOutputDto {OrderID = 10250,CustomerName = "Tamer Yaser", ShipperCity = "Jeddah", IsShipped = false },
                new GetOrdersOutputDto {OrderID = 10251,CustomerName = "Lina Majed", ShipperCity = "Abu Dhabi", IsShipped = false},
                new GetOrdersOutputDto {OrderID = 10252,CustomerName = "Yasmeen Rami", ShipperCity = "Kuwait", IsShipped = true}
            };

            return OrderList;
        }
    }
}
