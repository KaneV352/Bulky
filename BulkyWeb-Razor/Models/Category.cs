using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyWeb_Razor.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Category Name")]
        [MaxLength(30, ErrorMessage = "Name cannot have more than 30 characters")]
        public required string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Order must be between 1 and 100")]
        public int DisplayOrder { get; set; }
    }
}
