
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class MoreImage
    {
        public MoreImage() { }

        [Key]
        public int Id { get; set; }
        public string Name_img { get; set; }
        public int? New_ID { get; set; }
        public DateTime Dated { get; set; }
        public string IMAGE { get; set; }
        public virtual NewActicle? NewActicle { get; set; }

    }
}
