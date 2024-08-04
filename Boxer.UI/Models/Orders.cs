using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Boxer.UI.Models
{
    public class Orders
    {
        [Key]
        public int OrderID { get; set; }

        [Display(Name = "Order Number", Prompt = "Order Number")]
        public int OrderNumber { get; set; }

        [Display(Name = "Items", Prompt = "Items")]
        public string? Items { get; set; }

        [Display(Name = "Quantity", Prompt = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Date", Prompt = "Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime Date { get; set; }

        [Display(Name = "Price (R)", Prompt = "Price")]
        public double Price { get; set; }

        [Display(Name = "Supplier", Prompt = "Supplier")]
        public string? Supplier { get; set; }

        [Display(Name = "Delivery Date", Prompt = "Delivery Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime DeliveryDate { get; set; }


        [NotMapped]
        public List<SelectListItem> Suppliers { get; } = new List<SelectListItem>
        {
            new SelectListItem { Text = "Please select a supplier", Disabled = true, Selected = true },
            new SelectListItem { Value = "FreshFarm", Text = "FreshFarm" },
            new SelectListItem { Value = "FruitWorld", Text = "FruitWorld" },
            new SelectListItem { Value = "VeggieMart", Text = "VeggieMart" },
            new SelectListItem { Value = "MeatSupplier", Text = "MeatSupplier" },
            new SelectListItem { Value = "BakeryCo", Text = "BakeryCo" },
            new SelectListItem { Value = "DairyFarm", Text = "DairyFarm" },
            new SelectListItem { Value = "GardenFresh", Text = "GardenFresh" },
            new SelectListItem { Value = "FarmStand", Text = "FarmStand" },
            new SelectListItem { Value = "DairyDelights", Text = "DairyDelights" },
            new SelectListItem { Value = "FarmEggs", Text = "FarmEggs" }
        };
    }
}
