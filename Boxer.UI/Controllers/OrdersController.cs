using Boxer.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Boxer.UI.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrdersController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            if (TempData.ContainsKey("GeneratedOrderNumber"))
            {
                int generatedOrderNumber = (int)TempData["GeneratedOrderNumber"]!;
                ViewBag.GeneratedOrderNumber = generatedOrderNumber;
            }

            var client = _httpClientFactory.CreateClient("Boxer.API");

            try
            {
                var response = await client.GetFromJsonAsync<List<Orders>>("api/Orders");

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
                if (ex.InnerException is System.Net.Sockets.SocketException)
                {
                    TempData["ErrorMessage"] = "API is currently unavailable.";
                    return RedirectToAction("Error");
                }
                else
                {
                    TempData["ErrorMessage"] = ex.Message.ToString();
                    return RedirectToAction("Error");
                }
            }
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var client = _httpClientFactory.CreateClient("Boxer.API");

            try
            {
                var response = await client.GetFromJsonAsync<Orders>($"api/Orders/{id}");

                if (response != null)
                {
                    return View(response);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException is System.Net.Sockets.SocketException)
                {
                    TempData["ErrorMessage"] = "API is currently unavailable.";
                    return RedirectToAction("Error");
                }
                else
                {
                    TempData["ErrorMessage"] = ex.Message.ToString();
                    return RedirectToAction("Error");
                }
            }
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            var model = new Orders();
            return View(model);
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Orders orders)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("Boxer.API");

                try
                {
                    var response = await client.PostAsJsonAsync("api/Orders", orders);

                    if (response.IsSuccessStatusCode)
                    {
                        // Extract OrderNumber from the response
                        var createdOrder = await response.Content.ReadFromJsonAsync<Orders>();
                        if (createdOrder != null)
                        {
                            TempData["GeneratedOrderNumber"] = createdOrder.OrderNumber;
                        }
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Error creating Order. Please try again.";
                        return RedirectToAction("Error");
                    }
                }
                catch (HttpRequestException ex)
                {
                    if (ex.InnerException is System.Net.Sockets.SocketException)
                    {
                        TempData["ErrorMessage"] = "API is currently unavailable.";
                        return RedirectToAction("Error");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = ex.Message.ToString();
                        return RedirectToAction("Error");
                    }
                }
            }
            return View(orders);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient("Boxer.API");

            try
            {
                var response = await client.GetFromJsonAsync<Orders>($"api/Orders/{id}");

                if (response != null)
                {
                    return View(response);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException is System.Net.Sockets.SocketException)
                {
                    TempData["ErrorMessage"] = "API is currently unavailable.";
                    return RedirectToAction("Error");
                }
                else
                {
                    TempData["ErrorMessage"] = ex.Message.ToString();
                    return RedirectToAction("Error");
                }
            }
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Orders orders)
        {
            if (id != orders.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("Boxer.API");

                try
                {
                    var response = await client.PutAsJsonAsync($"api/Orders/{id}", orders);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Error updating Order. Please try again.";
                        return RedirectToAction("Error");
                    }
                }
                catch (HttpRequestException ex)
                {
                    if (ex.InnerException is System.Net.Sockets.SocketException)
                    {
                        TempData["ErrorMessage"] = "API is currently unavailable.";
                        return RedirectToAction("Error");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = ex.Message.ToString();
                        return RedirectToAction("Error");
                    }
                }
            }
            return View(orders);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("Boxer.API");

            try
            {
                var response = await client.GetFromJsonAsync<Orders>($"api/Orders/{id}");

                if (response != null)
                {
                    return View(response);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException is System.Net.Sockets.SocketException)
                {
                    TempData["ErrorMessage"] = "API is currently unavailable.";
                    return RedirectToAction("Error");
                }
                else
                {
                    TempData["ErrorMessage"] = ex.Message.ToString();
                    return RedirectToAction("Error");
                }
            }
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = _httpClientFactory.CreateClient("Boxer.API");

            try
            {
                var response = await client.DeleteAsync($"api/Orders/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Error deleting Order. Please try again.";
                    return RedirectToAction("Error");
                }
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException is System.Net.Sockets.SocketException)
                {
                    TempData["ErrorMessage"] = "API is currently unavailable.";
                    return RedirectToAction("Error");
                }
                else
                {
                    TempData["ErrorMessage"] = ex.Message.ToString();
                    return RedirectToAction("Error");
                }
            }
        }

        private async Task<bool> OrdersExistsAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("Boxer.API");

            try
            {
                var response = await client.GetFromJsonAsync<Orders>($"api/Orders/{id}");
                return response != null;
            }
            catch (HttpRequestException)
            {
                return false;
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
