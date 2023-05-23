using DollarStoreAmazon.Data;
using DollarStoreAmazon.Models;
using Microsoft.AspNetCore.Mvc;

namespace DollarStoreAmazon.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DollarStoreAmazonDbContext dbContext;
        public CategoryController(DollarStoreAmazonDbContext DbContext)
        {
            this.dbContext = DbContext;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = dbContext.Categories;
            
            return View(objCategoryList);
        }
    }
}
