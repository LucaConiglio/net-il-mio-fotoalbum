using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }

        public string Image { get; set; }
        [DefaultValue(false)]
        public bool Visible { get; set; }


        public List<Category>? Categories { get; set; }
        

    }
}
