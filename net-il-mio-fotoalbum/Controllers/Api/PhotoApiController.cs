using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PhotoApiController : ControllerBase
    {
        // GET: api/<photoApi>
        [HttpGet]
        public IActionResult GetPhoto(string? search)
        {

            using var db = new FotoContext();
            IQueryable<Photo> listaPhoto = db.Photos;
            List<Photo> listaPhotoSing;

            if (search != null)
            {
                listaPhotoSing = db.Photos.Where(p => p.Title.ToLower().Contains(search.ToLower())).ToList();
                //prendo le pizze dal db li trasformo in caratteri minuscoli e gli dico la stringa che mi viene passata, la trasformo anchessa e vedo se il db ha la stessa stringa!
                return Ok(listaPhotoSing);
            }
            else
                return Ok(listaPhoto.ToList());


        }

        // GET api/<PizzaApi>/get/5
        [HttpGet("{id}")]
        public IActionResult Getphotosing(int? id)
        {
            using var db = new FotoContext();


            if (id != null)
            {

                var photo = db.Photos.Where(p => p.Id == id).FirstOrDefault();
                if (photo != null)
                    return Ok(photo);
                else return NotFound();
            }
            return NotFound();

        }


        [HttpGet]
        public IActionResult Form()
        {
            using var db = new FotoContext();
            var data = new Message();           
            return Ok(data);
        }


        [HttpPost]
        public IActionResult Form(Message data)
        {
            using var db = new FotoContext();
            Message message = new Message();
            message.Id = data.Id;
            message.Messages = data.Messages;
            message.Mail = data.Mail;
            message.Oggetto = data.Oggetto;
            db.Messages.Add(message);
            db.SaveChanges();

            return Ok(db);
        }
    }
}
