using Boxer.UI.Models;
using Microsoft.AspNetCore.Mvc;

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
            // Return the values that the user entered before pressing filter
            ViewData["StartOrderNumber"] = startOrderNumber;
            ViewData["EndOrderNumber"] = endOrderNumber;
            ViewData["StartDate"] = startDate?.ToString("yyyy-MM-dd");
            ViewData["EndDate"] = endDate?.ToString("yyyy-MM-dd");
            ViewData["SupplierName"] = supplierName;

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

        public IActionResult Error()
        {
            var errorMessage = TempData["ErrorMessage"] as string;
            var errorViewModel = new ErrorViewModel { ErrorMessage = errorMessage };
            return View(errorViewModel);
        }
    }
}

