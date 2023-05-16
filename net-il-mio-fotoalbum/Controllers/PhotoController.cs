using Microsoft.AspNetCore.Authorization;
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

        public IActionResult Index()
        {
            using var db = new FotoContext();

            List<Photo> photos = db.Photos.ToList();

            return View(photos);
        }

        //******************************************************************************** GET DETAILS
        // GET: PizzaController1/Details/5
        public ActionResult Details(int id)
        {
            using var db = new FotoContext();
            if (id == null)
                return NotFound();

            var photo = db.Photos.FirstOrDefault(p => p.Id == id);
            if (photo == null)
                return NotFound();

            return View("ShowPhoto", photo);
        }
        //******************************************************************************** GET CREATE
        // GET: PizzaController1/Create
        //[Authorize(Roles = "Admin")]
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
        //[Authorize(Roles = "Admin")]
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
            //sto inizializzando una lista di ingredienti se nò é nullo a questo punto!
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
        //[Authorize(Roles = "Admin")]
        //public ActionResult Update(int id)
        //{
        //    using PizzaContext db = new PizzaContext();

        //    PizzaFormModel model = new PizzaFormModel();
        //    List<Categories> categories = db.Categories.ToList();
        //    //creo una lista dove metto gli ingredienti
        //    List<Ingredient> ingredienti = db.ingredients.ToList();
        //    //creo la lista da utilizzare di tipo SelectlistItem
        //    List<SelectListItem> listIngredienti = new List<SelectListItem>();

        //    //itero su ingredienti
        //    foreach (Ingredient ingrediente in ingredienti)
        //    {
        //        //ogni ingrediente lo aggiungo e creo una nuova istanza della classe SelectListItem dove specifico text e value
        //        listIngredienti.Add(new SelectListItem()
        //        {
        //            Text = ingrediente.Name,
        //            Value = ingrediente.Id.ToString(),
        //            Selected = ingredienti.Any(m => m.Id == ingrediente.Id)
        //        });
        //    }
        //    //passo al modello gli ingredienti
        //    model.Ingredienti = listIngredienti;
        //    //dico che model.categ é = alla lista di categ
        //    model.Categories = categories;


        //    model.Pizza = db.Pizze.FirstOrDefault(p => p.Id == id);

        //    return View("EditPizza", model);
        //}

        //// POST: PizzaController1/Edit/5
        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Update(int id, PizzaFormModel data)
        //{
        //    using PizzaContext db = new PizzaContext();
        //    if (!ModelState.IsValid)
        //    {
        //        //assegno a ingredients la lista degli ingredienti che prendo dal db
        //        List<Ingredient> ingredients = db.ingredients.ToList();
        //        //creo una nuova lista degli ingredienti selezionati
        //        List<SelectListItem> IngredientSelected = new List<SelectListItem>();
        //        foreach (Ingredient ingrediente in ingredients)
        //        {
        //            //aggiungo gli elementi selezionati alla lista e li converto in una stringa per manipolarla meglio
        //            IngredientSelected.Add(
        //                new SelectListItem()
        //                { Text = ingrediente.Name, Value = ingrediente.Id.ToString() });
        //        }
        //        data.Ingredienti = IngredientSelected;

        //        List<Categories> categories = db.Categories.ToList();
        //        data.Categories = categories;
        //        return View("EditPizza", data);
        //    }

        //    //assegno a pizzamodificata una pizza presa dal db che abbia lo stesso id e che include gli ingredienti
        //    Pizza pizzaModificata = db.Pizze.Where(pizza => pizza.Id == id)
        //        .Include(pizza => pizza.Ingredients).FirstOrDefault();
        //    //quello che passo tramide data lo do a pizza modificata e così via!
        //    pizzaModificata.Name = data.Pizza.Name;
        //    pizzaModificata.Description = data.Pizza.Description;
        //    pizzaModificata.CategoryId = data.Pizza.CategoryId;
        //    pizzaModificata.image = data.Pizza.image;
        //    pizzaModificata.price = data.Pizza.price;
        //    //sto inizializzando una lista di ingredienti se nò é nullo a questo punto!
        //    pizzaModificata.Ingredients = new List<Ingredient>();


        //    //se quello che passo come parametro, cioé gli ingredienti selezionati non sono nulli allora
        //    if (data.SelectedIngredients != null)
        //    {
        //        //itero, ingredienteSelezionato in quello che mi hanno passato come parametro
        //        foreach (string ingredienteSelezionato in data.SelectedIngredients)
        //        {
        //            //trasformo in ingredienteID cioé in intero ingredienteselezionato
        //            int ingredienteSelezionatoId = int.Parse(ingredienteSelezionato);
        //            //ingrediente é = a dentro al db id = allo stesso id di ingredienteselezionatoID e prendo il primo
        //            Ingredient ingredient = db.ingredients
        //                .Where(m => m.Id == ingredienteSelezionatoId)
        //                .FirstOrDefault();
        //            //passo a pizza da modificare alla lista degli ingredienti, gli ingredienti che hannoo lo stesso id
        //            pizzaModificata.Ingredients.Add(ingredient);
        //        }
        //    }


        //    db.SaveChanges();
        //    return RedirectToAction("Index", new { message = "Pizza inserita correttamente" });
        //}

        //// GET: PizzaController1/Delete/5
        //[Authorize(Roles = "Admin")]
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: PizzaController1/Delete/5
        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{

        //    using PizzaContext db = new PizzaContext();

        //    if (id == null)
        //        return NotFound();

        //    var pizza = db.Pizze.FirstOrDefault(p => p.Id == id);
        //    if (pizza == null)
        //        return NotFound();
        //    db.Pizze.Remove(pizza);
        //    db.SaveChanges();

        //    return RedirectToAction("index");
        //}
    }
}
