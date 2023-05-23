using Microsoft.AspNetCore.Mvc;

namespace DollarStoreAmazon.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
