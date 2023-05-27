using DollarStoreAmazon.DataAccess.Data;
using DollarStoreAmazon.DataAccess.Repository.IRepository;
using DollarStoreAmazon.Model;
using Microsoft.AspNetCore.Mvc;

namespace DollarStoreAmazon.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository DbContext)
        {
            this._categoryRepository = DbContext;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _categoryRepository.GetAll();
            
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
                _categoryRepository.Add(obj);
                _categoryRepository.Save();
                TempData["success"] = "Category Created successfully";
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

            
            var categoryFromDb = _categoryRepository.Get(u => u.Id == id);

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
                _categoryRepository.Add(obj);
                _categoryRepository.Save();
                TempData["success"] = "Category Edited successfully";

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


            var categoryFromDb = _categoryRepository.Get(u => u.Id == id);

            if (categoryFromDb==null)
            {
                return NotFound();
            }


            return View(categoryFromDb);
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var categoryFromDb = _categoryRepository.Get(u => u.Id == id);
            if (categoryFromDb == null)
                return NotFound();

            _categoryRepository.Remove(categoryFromDb);
            _categoryRepository.Save();
            TempData["success"] = "Category Deleted successfully";

            return RedirectToAction("Index");
        }
    }
}
