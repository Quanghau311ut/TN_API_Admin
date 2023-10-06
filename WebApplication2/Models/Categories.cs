using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Categories
    {
        public Categories()
        {
            this.NewActicles = new HashSet<NewActicle>();
        }
        [Key]
        public int Id { get; set; }
        public string name { get; set; }
        public string? description { get; set; }
        public DateTime? Dated { get; set; }
        public string Created { get; set; }
        public virtual ICollection<NewActicle> NewActicles { get; set; }
    }
}
