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


        public OrdersController(IOrdersService auditFindingsService)
        {
            _ordersService = auditFindingsService ?? throw new ArgumentNullException(nameof(auditFindingsService));
        }


        [HttpGet]
        public async Task<ActionResult<List<Orders>>> GetAllOrders()
        {
            var auditFindingsList = await _ordersService.GetAllOrders();
            return Ok(auditFindingsList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Orders>> GetOrdersById(int id)
        {
            var auditFindings = await _ordersService.GetOrderById(id);
            if (auditFindings == null)
            {
                return NotFound();
            }
            return Ok(auditFindings);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrders(Orders auditFindings)
        {
            await _ordersService.AddOrder(auditFindings);
            return Ok(auditFindings);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrders(int id, Orders auditFindings)
        {
            if (id != auditFindings.OrderID)
            {
                return BadRequest();
            }

            try
            {
                await _ordersService.UpdateOrder(auditFindings);
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
