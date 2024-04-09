﻿using ECommerce.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult Create(Product product)
        {
            Category c1 = _db.Categories
                .Where(c => c.CategoryId == (int)product.CategoryId)
                .FirstOrDefault();
            product.category = c1;
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
        public IActionResult Edit(Product product)
        {
            Category c1 = _db.Categories
                .Where(c => c.CategoryId == (int)product.CategoryId)
                .FirstOrDefault();
            Product p1 = _db.Products.Include(_ => _.category)
                .Where(_ => _.BookId == product.BookId)
                .FirstOrDefault();



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
            _db.Products.Remove(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
