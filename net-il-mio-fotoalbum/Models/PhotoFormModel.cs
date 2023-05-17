using Microsoft.AspNetCore.Mvc.Rendering;

namespace net_il_mio_fotoalbum.Models
{
    public class PhotoFormModel
    {
        public Photo Photo { get; set; }
        public List<SelectListItem>? Categories { get; set; } // tutte le categorie
        public List<string>? SelectedCategories { get; set; } // le categorie selezionate

    }
}
