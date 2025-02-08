using System.Linq;
using System.Web.Mvc;
using EFDBFirstApp.Models;

namespace EFDBFirstApp.Controllers
{
    public class BrandsController : Controller
    {
        private EFDBFirstDatabaseEntities db = new EFDBFirstDatabaseEntities();

        // GET: Brands (List all brands)
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Auth");

            var brands = db.Brands.ToList(); // Fetch all brands
            return View(brands);
        }
    }
}
