using ECommerce.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
namespace ECommerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductController :Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
          List<Product> data =   _db.Products.ToList();
            return View(data);
        }
        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            _db.Products.Add(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id) {
            Product data =  _db.Products.Find(id);
            return View("Update",data);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            _db.Products.Update(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            Product product = _db.Products.Find(id);
            _db.Products.Remove(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
