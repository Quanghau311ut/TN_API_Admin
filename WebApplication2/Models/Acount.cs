using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Acount
    {
        public Acount() { }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address {  get; set; }
        public DateTime Dateofbirth {  get; set; }
        public string Email {  get; set; }
        public string Username {  get; set; }
        public string Password { get; set; }
    }
}
