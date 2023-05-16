﻿using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        public List<Photo>? Photos { get; set; }
    }
}
