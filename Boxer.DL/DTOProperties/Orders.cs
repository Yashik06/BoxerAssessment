using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boxer.DL.DTOProperties
{
    public class Orders
    {
        [Key]
        public int OrderID { get; set; }

        [Display(Name = "Order Number", Prompt = "Order Number")]
        public int OrderNumber { get; set; }

        [Display(Name = "Items", Prompt = "Items")]
        public string Items { get; set; }

        [Display(Name = "Quantity", Prompt = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Date", Prompt = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Price", Prompt = "Price")]
        public double Price { get; set; }

        [Display(Name = "Supplier", Prompt = "Supplier")]
        public string Supplier { get; set; }

        [Display(Name = "Delivery Date", Prompt = "Delivery Date")]
        public DateTime DeliveryDate { get; set; }

    }
}
