using ECommerce.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
namespace ECommerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductController :Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string? imagePath;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> data1 = _db.Products.Include(p => p.category).ToList();
            return View(data1);
        }
        public IActionResult Create() {
            List<SelectListItem> list = _db.Categories.Select(e => new SelectListItem
            {
                Text = e.Name,
                Value = e.CategoryId.ToString()
            }).ToList();
            ViewBag.categoryList = list;
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product, IFormFile file)
        {
            Console.WriteLine(file);
            Category c1 = _db.Categories
                .Where(c => c.CategoryId == (int)product.CategoryId)
                .FirstOrDefault();
            product.category = c1;
            string webRoot = _webHostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filepath = Path.Combine(webRoot,@"Images\Products");
            Console.WriteLine(Path.Combine(filepath,fileName));
            using (var fileStream = new FileStream(Path.Combine(filepath, fileName), FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            product.imageUrl = @"\Images\Products\"+fileName;
            _db.Products.Add(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id) {
            List<SelectListItem> list = _db.Categories.Select(e => new SelectListItem
            { 
                Text = e.Name,
                Value = e.CategoryId.ToString()
            }).ToList();
            Product data = _db.Products.Include(_ => _.category).Where(p=>p.BookId==id).FirstOrDefault();
            Console.WriteLine(data);
            ViewBag.categoryList = list;
            return View("Update",data);
        }

        [HttpPost]
        public IActionResult Edit(Product product, IFormFile file)
        {
            Category c1 = _db.Categories
                .Where(c => c.CategoryId == (int)product.CategoryId)
                .FirstOrDefault();
            Product p1 = _db.Products.Include(_ => _.category)
                .Where(_ => _.BookId == product.BookId)
                .FirstOrDefault();
            Console.WriteLine(file);

            if (file != null)

            {
                string webRoot = _webHostEnvironment.WebRootPath;
                string fileName1 = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string newPath = Path.Combine(webRoot, @"Images\Products");
                var oldPath = Path.Combine(webRoot,product.imageUrl.TrimStart('\\'));
              
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                    using (var fileStream = new FileStream(Path.Combine(newPath, fileName1), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                        p1.imageUrl = @"\Images\Products\" + fileName1;
                    }
                }
            }
            else
            {
                p1.imageUrl = product.imageUrl;
            }
            if (p1 != null)
            {
                p1.BookTitle = product.BookTitle;
                p1.ISBN = product.ISBN;
                p1.Description = product.Description;
                p1.Author = product.Author;
                p1.Price = product.Price;
                p1.category = c1;
                p1.CategoryId = product.CategoryId;
                _db.SaveChanges();
            }
            //Console.WriteLine(product);
            //_db.Products.Update(product);
            //_db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            Product product = _db.Products.Find(id);
            string webRoot = _webHostEnvironment.WebRootPath;
            string imagePath = Path.Combine(webRoot, product.imageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            _db.Products.Remove(product);
            _db.SaveChanges();
            return Json(new { success=true,message="Product Deleted"});
        }
    }
}
