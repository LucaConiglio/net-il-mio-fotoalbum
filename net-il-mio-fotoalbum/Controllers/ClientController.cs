using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers
{
    public class ClientController : Controller
    {
        public ActionResult Details(int id)
        {
            using var db = new FotoContext();
            if (id == null)
                return NotFound();

            var photo = db.Photos.Include(c => c.Categories).FirstOrDefault(p => p.Id == id);
            if (photo == null)
                return NotFound();

            return View( photo);
        }
    }
}
