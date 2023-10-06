

using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Brand
    {
        public Brand() { }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image {  get; set; }
        public string Content {  get; set; }
        public DateTime? Dated { get; set; }
    }
}
