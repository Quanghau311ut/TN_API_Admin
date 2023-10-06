using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Hotline
    {
        public Hotline() { }
        [Key]
        public int Id { get; set; }

        public string Email {  get; set; }
        public string Tel {  get; set; }
        public string Content {  get; set; }
        public DateTime? Dated {  get; set; }
    }
}
