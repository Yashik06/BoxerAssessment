using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Boxer.DL.DTOProperties;
using Boxer.DL;
using Boxer.BL.Interfaces;

namespace Boxer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly IOrdersService _ordersService;


        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService ?? throw new ArgumentNullException(nameof(ordersService));
        }

        // Action to Get all orders
        // Using optional parameters depending on user input
        [HttpGet]
        public async Task<ActionResult<List<Orders>>> GetAllOrders
        (
            int? startOrderNumber = null,
            int? endOrderNumber = null,
            string? supplierName = null,
            DateTime? startDate = null,
            DateTime? endDate = null
        )
        {
            var ordersList = await _ordersService.GetAllOrders();

            // Apply order number range filtering
            if (startOrderNumber.HasValue && endOrderNumber.HasValue)
            {
                ordersList = ordersList.Where(o => o.OrderNumber >= startOrderNumber.Value && o.OrderNumber <= endOrderNumber.Value).ToList();
            }

            // Apply supplier name filtering
            if (!string.IsNullOrWhiteSpace(supplierName))
            {
                ordersList = ordersList.Where(o => o.Supplier.Equals(supplierName, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Apply date range filtering
            if (startDate.HasValue && endDate.HasValue)
            {
                ordersList = ordersList.Where(o => o.Date >= startDate.Value && o.Date <= endDate.Value).ToList();
            }

            // Apply date range filtering
            if (startDate.HasValue)
            {
                ordersList = ordersList.Where(o => o.Date >= startDate.Value).ToList();
            }

            return Ok(ordersList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Orders>> GetOrdersById(int id)
        {
            var orders = await _ordersService.GetOrderById(id);
            if (orders == null)
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrders(Orders orders)
        {
            await _ordersService.AddOrder(orders);
            return Ok(orders);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrders(int id, Orders orders)
        {
            if (id != orders.OrderID)
            {
                return BadRequest();
            }

            try
            {
                await _ordersService.UpdateOrder(orders);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrders(int id)
        {
            var deleted = await _ordersService.DeleteOrder(id);

            if (!deleted)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
