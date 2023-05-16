using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
       
        public string Mail { get; set; }
      
        public string Oggetto { get; set; }
       
        public string Messages { get; set; }
    }
}
