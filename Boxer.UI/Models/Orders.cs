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
        [Required(ErrorMessage = "Items field cannot be blank.")]
        public string? Items { get; set; }

        [Display(Name = "Quantity", Prompt = "Quantity")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
        public int Quantity { get; set; }

        [Display(Name = "Date", Prompt = "Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Order date is required.")]
        public DateTime Date { get; set; }

        [Display(Name = "Price (R)", Prompt = "Price")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [Display(Name = "Supplier", Prompt = "Supplier")]
        [Required(ErrorMessage = "Supplier is required.")]
        public string? Supplier { get; set; }

        [Display(Name = "Delivery Date", Prompt = "Delivery Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Delivery date is required.")]
        [CustomValidation(typeof(Orders), nameof(ValidateDeliveryDate))]
        public DateTime DeliveryDate { get; set; }

        // Custom validation method for Delivery Date
        public static ValidationResult? ValidateDeliveryDate(DateTime deliveryDate, ValidationContext context)
        {
            var instance = context.ObjectInstance as Orders;
            if (instance != null && deliveryDate < instance.Date)
            {
                return new ValidationResult("Delivery date cannot be before the order date.");
            }
            return ValidationResult.Success;
        }

        [NotMapped]
        public List<SelectListItem> Suppliers { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = null, Text = "Please select a supplier", Disabled = true, Selected = true },
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
