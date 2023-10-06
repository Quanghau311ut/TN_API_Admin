using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Slider
    {
        public Slider() { }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; } 
        public string Content { get; set; }
        public DateTime? Dated { get; set; }= DateTime.Now;
        [NotMapped]
        public IFormFile file { get; set; }

    }
}
