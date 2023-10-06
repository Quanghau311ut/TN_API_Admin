using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class NewActicle
    {
        public NewActicle()
        {
            this.MoreImages = new HashSet<MoreImage>();
        }
        [Key]
        public int ID { get; set; }
        public int? Categories_ID { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public DateTime? Dated { get; set; }
        public string Created { get; set; }
        public virtual Categories? Categories { get; set; }

        public virtual ICollection<MoreImage> MoreImages { get; set; }
        //public virtual MoreImage MoreImage { get; set; }
    }
}
