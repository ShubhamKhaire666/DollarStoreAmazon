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

        [HttpGet]
        public IActionResult Create()
        {
             return View();
        }  
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if(obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The display order cant be same as the name.");
            }
            if (ModelState.IsValid)
            {
                dbContext.Categories.Add(obj);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obj);
        }
    }
}
