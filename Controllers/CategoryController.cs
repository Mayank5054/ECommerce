using ECommerce.Data;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
namespace ECommerce.Controllers
{
    public class CategoryController :Controller
    {
        public readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
            Console.WriteLine(_db);
        }
        public IActionResult Index()
        {
            List<Category> objList =  _db.Categories.ToList();
            Console.WriteLine("It's Demo");
            Console.Write(objList);
            return View("categoryIndex",objList);
        }
        public IActionResult Create() {
            return View("categoryCreate");
        }

        [HttpPost]
        public IActionResult Create(Category formData)
        {
            if (formData.Name == formData.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "*Name Should Not Be Same As Display Order");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Add(formData);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("categoryCreate");
        }

        public IActionResult Edit(int id) 
        {
            Category obj = _db.Categories.Find(id);
            return View("categoryEdit",obj);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["status"] = "Completed";
                return RedirectToAction("Index");
            }
        }

       
     
        public IActionResult Delete(int id)
       
        {
            Category obj = _db.Categories.Find(id);
            _db.Categories.Remove(obj);
            _db.SaveChanges();
            TempData["status"] = "Deleted";
            return RedirectToAction("Index");
        }
    }
}
