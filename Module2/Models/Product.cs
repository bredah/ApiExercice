using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module2.Models
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [RegularExpression(@"[0-9]+,[0-9]{2}$", ErrorMessage = "Invalid price value")]
        public string Price { get; set; }
    }
}