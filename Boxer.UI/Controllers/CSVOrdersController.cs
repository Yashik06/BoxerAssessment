using Boxer.UI.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Boxer.UI.Controllers
{
    public class CSVOrdersController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        // Constructor to initialize IHttpClientFactory
        public CSVOrdersController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Action to render the upload CSV view
        public IActionResult UploadCsv()
        {
            return View();
        }

        private CsvConfiguration GetCsvConfiguration()
        {
            return new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";", // Use semicolon as the delimiter
                HeaderValidated = null, // Disable header validation
                MissingFieldFound = null // Disable missing field validation
            };
        }

        //Method to handle the reading of the CSV file and converts it to a list of CsvOrders records
        private List<CsvOrders> ReadCsvOrders(IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, GetCsvConfiguration()))
            {
                return csv.GetRecords<CsvOrders>().ToList();
            }
        }

        // Action to handle the CSV file upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadCsv(IFormFile file)
        {
            // Check if a file was selected and is not empty
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "CSV is empty or not selected.");
                return View();
            }

            var validOrders = new List<CsvOrders>();
            var errors = new List<string>();

            try
            {
                //Read the CSV records
                var ordersCsv = ReadCsvOrders(file);

                foreach (var order in ordersCsv)
                {
                    //Validates each record and collects valid orders and errors
                    if (TryValidateModel(order))
                    {
                        validOrders.Add(order);
                    }
                    else
                    {
                        errors.AddRange(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    }
                }
            }
            catch (CsvHelperException ex)
            {
                // Handle exceptions during CSV reading and parsing
                ModelState.AddModelError("", $"There is an invalid field in this order: {ex.Context.Parser.RawRecord}");
                return View();
            }


            // If there are any validation errors, redirect to the error message
            if (errors.Any())
            {
                TempData["ErrorMessage"] = "Some records have errors. Please correct them and try again.";
                return RedirectToAction("UploadCsv");
            }

            var orderNumbers = new List<int>();

            // Submit each valid order and collect the generated OrderNumbers
            foreach (var order in validOrders)
            {
                var orderNumber = await SubmitOrderAsync(order);
                if (orderNumber.HasValue)
                {
                    orderNumbers.Add(orderNumber.Value);
                }
            }

            // Display the success message with the generated OrderNumbers
            TempData["SuccessMessage"] = "Orders successfully processed. Generated Order Numbers: " + string.Join(", ", orderNumbers);
            return RedirectToAction("UploadCsv");
        }


        // Method to submit an order and return the generated OrderNumber
        private async Task<int?> SubmitOrderAsync(CsvOrders order)
        {
            var client = _httpClientFactory.CreateClient("Boxer.API");

            try
            {
                var response = await client.PostAsJsonAsync("api/Orders", order);

                if (response.IsSuccessStatusCode)
                {
                    var createdOrder = await response.Content.ReadFromJsonAsync<Orders>();
                    return createdOrder?.OrderNumber;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = "An error occurred while submitting your order. Please try again later.";
                }

            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpRequestException: {ex.Message}");

                if (ex.InnerException is System.Net.Sockets.SocketException)
                {
                    TempData["ErrorMessage"] = "Unable to reach the server. Please check your internet connection and try again.";
                }
                else
                {
                    TempData["ErrorMessage"] = "An unexpected error occurred while submitting your order. Please try again later.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                TempData["ErrorMessage"] = "An unexpected error occurred. Please try again later.";
            }
            return null;
        }
    }
}
