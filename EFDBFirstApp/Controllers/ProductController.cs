using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
            public ActionResult Create(Product product)
            {
                if (ModelState.IsValid)
                {
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
            public ActionResult Edit(Product product)
            {
                if (ModelState.IsValid)
                {
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
        }
    }
