using ECommerce.Data;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
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
            _db.Categories.Add(formData);
            _db.SaveChanges();
            return RedirectToAction("Index");
            /*reurn View("categoryCreate");*/
        }
    }
}
