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
        
        public IActionResult Edit(int? id)
        {

            if(id== null && id == 0)
            {
                return NotFound();
            }

            
            var categoryFromDb = dbContext.Categories.Find(id);

            if(categoryFromDb==null)
            {
                return NotFound();
            }


            return View(categoryFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The display order cant be same as the name.");
            }
            if (ModelState.IsValid)
            {
                dbContext.Categories.Update(obj);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obj);
        }  
        
        
        public IActionResult Delete(int? id)
        {

            if(id== null && id == 0)
            {
                return NotFound();
            }

            
            var categoryFromDb = dbContext.Categories.Find(id);

            if(categoryFromDb==null)
            {
                return NotFound();
            }


            return View(categoryFromDb);
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = dbContext.Categories.Find(id);
            if (obj == null)
                return NotFound();

                dbContext.Categories.Remove(obj);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
        }
    }
}
