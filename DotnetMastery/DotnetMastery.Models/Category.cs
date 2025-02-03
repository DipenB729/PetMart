using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Dotnet.Models;

namespace Dotnet.Models
{
    public class Category
    {

        [Key]
        public int Id {  get; set; }
        [Required]
        [MaxLength(32)]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1,100)]
        [Required]
        public int DisplayOrder { get; set; }
    }
}
