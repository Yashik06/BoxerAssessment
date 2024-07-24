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


        [HttpGet]
        public async Task<ActionResult<List<Orders>>> GetAllOrders()
        {
            var ordersList = await _ordersService.GetAllOrders();
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
