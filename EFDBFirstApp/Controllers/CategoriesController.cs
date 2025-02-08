using System.Linq;
using System.Web.Mvc;
using EFDBFirstApp.Models;

namespace EFDBFirstApp.Controllers
{
    public class CategoriesController : Controller
    {
        private EFDBFirstDatabaseEntities db = new EFDBFirstDatabaseEntities();

        // GET: Categories (List all categories)
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Auth");

            var categories = db.Categories.ToList(); // Fetch all categories
            return View(categories);
        }
    }
}
