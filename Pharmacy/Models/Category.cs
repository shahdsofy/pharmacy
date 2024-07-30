using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Pharmacy.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [ValidateNever]
        public List<Medicine> Medicines { get; set; }
    }
}
