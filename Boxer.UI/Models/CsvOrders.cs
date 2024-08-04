using System.ComponentModel.DataAnnotations;

namespace Boxer.UI.Models
{
    public class CsvOrders : IValidatableObject
    {
        [Required(ErrorMessage = "Items field is required.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Items must contain only alphabetic characters.")]
        public string Items { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive integer.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Date must be a valid date.")]
        public DateTime Date { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Supplier field is required.")]
        public string Supplier { get; set; }

        [Required(ErrorMessage = "Delivery Date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Delivery Date must be a valid date.")]
        public DateTime DeliveryDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            // Validation to make sure that Delivery date is after Date of order
            if (DeliveryDate < Date)
            {
                results.Add(new ValidationResult("Delivery Date cannot be earlier than the Order Date.", new[] { nameof(DeliveryDate) }));
            }

            return results;
        }
    }
}
