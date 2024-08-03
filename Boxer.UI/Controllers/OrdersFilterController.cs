using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Boxer.UI.Data;
using Boxer.UI.Models;

namespace Boxer.UI.Controllers
{
    public class OrdersFilterController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrdersFilterController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: Orders
        public async Task<IActionResult> Index(
            int? startOrderNumber = null,
            int? endOrderNumber = null,
            string? supplierName = null,
            DateTime? startDate = null,
            DateTime? endDate = null
        )
        {
            var client = _httpClientFactory.CreateClient("Boxer.API");

            try
            {
                var url = $"api/Orders?startOrderNumber={startOrderNumber}&endOrderNumber={endOrderNumber}&supplierName={supplierName}&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";
                var response = await client.GetFromJsonAsync<List<Orders>>(url);

                if (response != null)
                {
                    return View(response);
                }
                else
                {
                    TempData["ErrorMessage"] = "No data available.";
                    return RedirectToAction("Error");
                }
            }
            catch (HttpRequestException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error");
            }
        }

        // Error action to display error messages
        public IActionResult Error()
        {
            var errorMessage = TempData["ErrorMessage"] as string;
            // Creates an instance of ErrorViewModel and set the ErrorMessage property
            var errorViewModel = new ErrorViewModel { ErrorMessage = errorMessage };
            return View(errorViewModel);
        }
    }
}

