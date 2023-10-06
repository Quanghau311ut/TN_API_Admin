using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Menu
    {
        public Menu() { }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? Dated { get; set; } 
        public string Creator { get; set; }
        public string? Image { get;  set; }
        public string Content { get;  set; }
    }
}
