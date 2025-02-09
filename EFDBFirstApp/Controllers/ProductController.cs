using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EFDBFirstApp.Models;

namespace EFDBFirstApp.Controllers
{
    public class ProductsController : Controller
    {
        private EFDBFirstDatabaseEntities db = new EFDBFirstDatabaseEntities();

        // GET: Products (List all products)
        public ActionResult Index()
        {
            //var products = db.Products.Include(p => p.Category).Include(p => p.Brand).ToList();
            //return View(products);
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Auth");

            return View(db.Products.ToList());
        }

        // GET: Products/Details/5 (View product details)
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product product = db.Products.Find(id);
            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        // GET: Products/Create (Show form to create new product)
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "BrandName");
            return View();
        }

        // POST: Products/Create (Save new product)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, HttpPostedFileBase ProductImage)
        {
            if (ModelState.IsValid)
            {
                if (ProductImage != null && ProductImage.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(ProductImage.FileName);
                    string path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    ProductImage.SaveAs(path);
                    product.ImagePath = "/Images/" + fileName; // Save relative path to database
                }

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "BrandName", product.BrandID);
            return View(product);
        }


        // GET: Products/Edit/5 (Show edit form)
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product product = db.Products.Find(id);
            if (product == null)
                return HttpNotFound();

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "BrandName", product.BrandID);
            return View(product);
        }

        // POST: Products/Edit/5 (Update product)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, HttpPostedFileBase ProductImage)
        {
            if (ModelState.IsValid)
            {
                // Check if a new image is uploaded
                if (ProductImage != null && ProductImage.ContentLength > 0)
                {
                    // Define the folder path inside the solution (e.g., "/Images/")
                    string imagePath = "~/Images/";

                    // Generate a unique file name to prevent conflicts
                    string fileName = Path.GetFileNameWithoutExtension(ProductImage.FileName);
                    string extension = Path.GetExtension(ProductImage.FileName);
                    string newFileName = fileName + "_" + DateTime.Now.Ticks + extension;
                    string fullPath = Path.Combine(Server.MapPath(imagePath), newFileName);

                    // Save the image file
                    ProductImage.SaveAs(fullPath);

                    // Update the product image path in the database
                    product.ImagePath = imagePath + newFileName;
                }

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "BrandName", product.BrandID);
            return View(product);
        }

        // GET: Products/Delete/5 (Show delete confirmation page)
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product product = db.Products.Find(id);
            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        // POST: Products/Delete/5 (Delete product)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Auth");
        }

    }
}
