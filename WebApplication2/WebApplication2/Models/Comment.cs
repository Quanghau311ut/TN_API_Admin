using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Comment
    {
        public Comment() { }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email {  get; set; }
        public string Content {  get; set; }
        public DateTime? Dated {  get; set; }

    }
}
