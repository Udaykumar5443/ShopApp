using System.Linq;
using System.Web.Mvc;
using EFDBFirstApp.Models;


namespace EFDBFirstApp.Controllers
    {
        public class AuthController : Controller
        {
            private EFDBFirstDatabaseEntities db = new EFDBFirstDatabaseEntities();

            // GET: Auth/Register
            public ActionResult Register()
            {
                return View();
            }

            // POST: Auth/Register
            [HttpPost]
            public ActionResult Register(UsersProduct user)
            {
                if (ModelState.IsValid)
                {
                    db.UsersProducts.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                }
                return View(user);
            }

            // GET: Auth/Login
            public ActionResult Login()
            {
                return View();
            }

            // POST: Auth/Login
            [HttpPost]
            public ActionResult Login(UsersProduct user)
            {
                var existingUser = db.UsersProducts.FirstOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);
                if (existingUser != null)
                {
                    Session["UserID"] = existingUser.UserID;
                    Session["Username"] = existingUser.UserName;
                    return RedirectToAction("Index", "Products");
                }

                ViewBag.Message = "Invalid credentials";
                return View();
            }

            // Logout
            public ActionResult Logout()
            {
                Session.Clear();
                return RedirectToAction("Login");
            }
        }
    }
