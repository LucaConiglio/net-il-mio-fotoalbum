﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Migrations;
using net_il_mio_fotoalbum.Models;
using System.Data;

namespace net_il_mio_fotoalbum.Controllers
{
    public class PhotoController : Controller
    {

        public IActionResult Index(string SearchString)
        {
            using var db = new FotoContext();

            List<Photo> photos = db.Photos.Include(p => p.Categories).ToList();

            if(SearchString != null)
            {
                photos = photos.Where(t=> t.Title.ToLower().Contains(SearchString.ToLower())).ToList();
            }


            
            return View(photos);
        }

        


        //******************************************************************************** GET DETAILS
        // GET: PizzaController1/Details/5
        public ActionResult Details(int id)
        {
            using var db = new FotoContext();
            if (id == null)
                return NotFound();

            var photo = db.Photos.Include(c => c.Categories).FirstOrDefault(p => p.Id == id);
            if (photo == null)
                return NotFound();

            return View("ShowPhoto", photo);
        }
        //******************************************************************************** GET CREATE
        // GET: PizzaController1/Create
        [Authorize(Roles = "ADMIN")]
        public ActionResult Create()
        {
            //per utilizzare il db
            using FotoContext db = new FotoContext();

            //creo una lista dove schiaffo tutte le categorie
            List<Category> categories = db.Categories.ToList();           
            //creo una nuova istanza di PhotoModel
            PhotoFormModel model = new PhotoFormModel();

            //creo la lista da utilizzare di tipo SelectlistItem
            List<SelectListItem> listaCategorie = new List<SelectListItem>();

            //itero su ingredienti
            foreach (Category category in categories)
            {
                //ogni ingrediente lo aggiungo e creo una nuova istanza della classe SelectListItem dove specifico text e value
                listaCategorie.Add(new SelectListItem()
                { Text = category.Title, Value = category.Id.ToString() });
            }
            
            //passo al modello gli ingredienti
            model.Categories = listaCategorie;
            //creo una nuova istanza di photo
            model.Photo = new Photo();

            return View("CreatePhoto", model);
        }

        //// POST: PizzaController1/Create
        ////************************************************************************************POST CREATE
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(PhotoFormModel data)
        {
            using FotoContext db = new FotoContext();
            if (!ModelState.IsValid)
            {
                List<Category> categorie = db.Categories.ToList();
                List<SelectListItem> listaCategorieSelezionate = new List<SelectListItem>();
                foreach (Category category in categorie)
                {
                    listaCategorieSelezionate.Add(
                        new SelectListItem()
                        { Text = category.Title, Value = category.Id.ToString() });
                }
                data.Categories = listaCategorieSelezionate;

                return View("CreatePhoto", data);
            }

            Photo photo = new Photo();
            photo.Title = data.Photo.Title;
            photo.Description = data.Photo.Description;
            photo.Image = data.Photo.Image;
            photo.Visible = data.Photo.Visible;
            //sto inizializzando una lista di delle categorie se nò é nullo a questo punto!
            photo.Categories = new List<Category>();



            if (data.SelectedCategories != null)
            {
                foreach (string categorieselezionate in data.SelectedCategories)
                {
                    int categoriaId = int.Parse(categorieselezionate);
                    Category categoria = db.Categories
                        .Where(m => m.Id == categoriaId)
                        .FirstOrDefault();
                    photo.Categories.Add(categoria);
                }
            }

            db.Photos.Add(photo);
            db.SaveChanges();
            return RedirectToAction("Index", new { message = "Foto Aggiunta correttamente" });
        }

        //// GET: PizzaController1/Edit/5
        ////********************************************************************************************************************* GET UPDATE
        [Authorize(Roles = "ADMIN")]
        public ActionResult Update(int id)
        {
            using FotoContext db = new FotoContext();

            PhotoFormModel model = new PhotoFormModel();

            List<Category> CategorieDb = db.Categories.ToList();
            //creo la lista da utilizzare di tipo SelectlistItem
            List<SelectListItem> listaCategorie = new List<SelectListItem>();

            //itero su ingredienti
            foreach (Category categoria in CategorieDb)
            {
            //ogni categoria la aggiungo e creo una nuova istanza della classe SelectListItem dove specifico text e value
                listaCategorie.Add(new SelectListItem()
                {
                    Text = categoria.Title,
                    Value = categoria.Id.ToString(),
                    Selected = CategorieDb.Any(m => m.Id == categoria.Id)
                });
            }
            //passo al modello gli ingredienti
            model.Categories = listaCategorie;
            //includo tutte le categorie per quella foto
            model.Photo = db.Photos.Include(p => p.Categories).FirstOrDefault(p => p.Id == id);
            //trasformo in stringhe le categorie e le do in pasto alla lista di selectedCategories
            model.SelectedCategories = model.Photo.Categories.Select(x => x.Id.ToString()).ToList();

            return View("EditPhoto", model);
        }

        //// POST: PizzaController1/Edit/5
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int id, PhotoFormModel data)
        {
            using FotoContext db = new FotoContext();
            if (!ModelState.IsValid)
            {
                //assegno a ingredients la lista degli ingredienti che prendo dal db
                List<Category> categorieDb = db.Categories.ToList();
                //creo una nuova lista degli ingredienti selezionati
                List<SelectListItem> Categoriselezionate = new List<SelectListItem>();
                foreach (Category categoria in categorieDb)
                {
                    //aggiungo gli elementi selezionati alla lista e li converto in una stringa per manipolarla meglio
                    Categoriselezionate.Add(
                        new SelectListItem()
                        { Text = categoria.Title, Value = categoria.Id.ToString() });
                }
                data.Categories = Categoriselezionate;              
                return View("EditPhoto", data);
            }

            //assegno a pizzamodificata una pizza presa dal db che abbia lo stesso id e che include gli ingredienti
            Photo photoModificata = db.Photos.Where(photo => photo.Id == id)
                .Include(photo => photo.Categories).FirstOrDefault();
            //quello che passo tramide data lo do a pizza modificata e così via!
            photoModificata.Title = data.Photo.Title;
            photoModificata.Description = data.Photo.Description;
            photoModificata.Image = data.Photo.Image;
            photoModificata.Visible = data.Photo.Visible;
            //sto inizializzando una lista di ingredienti se nò é nullo a questo punto!
            photoModificata.Categories = new List<Category>();


            //se quello che passo come parametro, cioé gli ingredienti selezionati non sono nulli allora
            if (photoModificata.Categories != null)
            {
                //itero, ingredienteSelezionato in quello che mi hanno passato come parametro
                foreach (string categoriaSelezionata in data.SelectedCategories)
                {
                    //trasformo in ingredienteID cioé in intero ingredienteselezionato
                    int categoriaId = int.Parse(categoriaSelezionata);
                    //ingrediente é = a dentro al db id = allo stesso id di ingredienteselezionatoID e prendo il primo
                    Category categoria = db.Categories
                        .Where(m => m.Id == categoriaId)
                        .FirstOrDefault();
                    //passo a pizza da modificare alla lista degli ingredienti, gli ingredienti che hannoo lo stesso id
                    photoModificata.Categories.Add(categoria);
                }
            }


            db.SaveChanges();
            return RedirectToAction("Index", new { message = "Foto modificata correttamente" });
        }

        //// GET: PizzaController1/Delete/5
        [Authorize(Roles = "ADMIN")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PizzaController1/Delete/5
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {

            using FotoContext db = new FotoContext();

            if (id == null)
                return NotFound();

            var photo = db.Photos.FirstOrDefault(p => p.Id == id);
            if (photo == null)
                return NotFound();
            db.Photos.Remove(photo);
            db.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
