using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Boxer.UI.Models;
using CsvHelper.Configuration;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout;

namespace Boxer.UI.Controllers
{
    public class OrdersExportController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrdersExportController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public CsvConfiguration GetCsvConfiguration()
        {
            return new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";", // Use semicolon as the delimiter
                HeaderValidated = null, // Disable header validation
                MissingFieldFound = null // Disable missing field validation
            };
        }

        public async Task<IActionResult> ExportToCsv(
            int? startOrderNumber = null,
            int? endOrderNumber = null,
            string? supplierName = null,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var client = _httpClientFactory.CreateClient("Boxer.API");

            try
            {
                var url = $"api/Orders?startOrderNumber={startOrderNumber}&endOrderNumber={endOrderNumber}&supplierName={supplierName}&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";
                var response = await client.GetFromJsonAsync<List<Orders>>(url);

                if (response != null)
                {
                    using (var memoryStream = new MemoryStream())
                    using (var writer = new StreamWriter(memoryStream))
                    using (var csv = new CsvWriter(writer, GetCsvConfiguration()))
                    {
                        csv.Context.RegisterClassMap<OrdersMap>();
                        csv.WriteRecords(response);
                        writer.Flush();
                        var result = memoryStream.ToArray();
                        return File(result, "text/csv", "Orders.csv");
                    }
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

        public class OrdersMap : ClassMap<Orders>
        {
            public OrdersMap()
            {
                // Exclude OrderID field
                Map(m => m.OrderID).Ignore();
                Map(m => m.OrderNumber).Name("Order Number");
                Map(m => m.Items).Name("Items");
                Map(m => m.Quantity).Name("Quantity");
                Map(m => m.Date).Name("Date").Convert(args => args.Value.Date.ToString("yyyy-MM-dd"));
                Map(m => m.Price).Name("Price");
                Map(m => m.Supplier).Name("Supplier");
                Map(m => m.DeliveryDate).Name("DeliveryDate").Convert(args => args.Value.DeliveryDate.ToString("yyyy-MM-dd"));
            }
        }


        public async Task<IActionResult> ExportToPdf(int id)
        {
            var client = _httpClientFactory.CreateClient("Boxer.API");

            try
            {
                var response = await client.GetFromJsonAsync<Orders>($"api/Orders/{id}");

                if (response != null)
                {
                    var pdfData = GeneratePdf(response);
                    return File(pdfData, "application/pdf", $"Order_{response.OrderNumber}.pdf");
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

        private byte[] GeneratePdf(Orders order)
        {
            using (var memoryStream = new MemoryStream())
            {
                // Initialize PDF writer and document
                using (var writer = new PdfWriter(memoryStream))
                {
                    var pdf = new PdfDocument(writer);
                    var document = new Document(pdf);

                    // Add content to the PDF
                    document.Add(new Paragraph("Order Details")
                        .SetFontSize(20)
                        .SetBold());

                    document.Add(new Paragraph($"Order Number: {order.OrderNumber}"));
                    document.Add(new Paragraph($"Items: {order.Items}"));
                    document.Add(new Paragraph($"Quantity: {order.Quantity}"));
                    document.Add(new Paragraph($"Date: {order.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}"));
                    document.Add(new Paragraph($"Price: {order.Price}"));
                    document.Add(new Paragraph($"Supplier: {order.Supplier}"));
                    document.Add(new Paragraph($"Delivery Date: {order.DeliveryDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}"));

                    document.Close();
                }

                return memoryStream.ToArray();
            }
        }

    }
}
