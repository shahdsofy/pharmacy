using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Pharmacy.Models
{
    public class Medicine
    {
        public int Id { get; set; }

        [DisplayName("Medicine Name")]
        public string MedicineName { get; set; }
        public decimal Price { get; set; }

        [DisplayName("Quantity_In_Stock")]
        public int QuantityInStock { get; set; }
        [DisplayName("Expiration Date")]
        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }
        public string Description { get; set; }

        public string Manufacturer { get; set; }
        [DisplayName("Prescription Requirements")]
        public string PrescriptionRequirements { get; set; }
        public string Barcode { get; set; }
        [NotMapped]
        [DisplayName("Confirm Barcode")]
        [Compare("Barcode", ErrorMessage = "Barcode and Confirm Barcode  Do not match")]

        public string ConfirmBarcode { get; set; }

        [DisplayName("Storage Conditions")]
        public string StorageConditions { get; set; }

        [DisplayName("Category")]
        [Range(1, double.MaxValue, ErrorMessage = "Choose a Valid Category")]
        public int CategoryId { get; set; }

        //Navigation property
        [ValidateNever]
        public Category Categories { get; set; }

        [ValidateNever]
        public string ImagePath { get; set; }
        [NotMapped]
        [ValidateNever]
        [DisplayName("Image")]
        public IFormFile ImageFile { get; set; }


    }
}
