using DollarStoreAmazon.DataAccess.Data;
using DollarStoreAmazon.DataAccess.Repository.IRepository;
using DollarStoreAmazon.Model;
using DollarStoreAmazon.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace DollarStoreAmazon.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.Product.GetAll().ToList();



            return View(products);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {

            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            ProductVM productVM = new()
            {
                CategoryList = categoryList,
                Product = new Product()
            };

            if (id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product =  _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
            //ViewBag.CategoryList = categoryList;
            //ViewData["CategoryList"] = categoryList;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj.Product);
                _unitOfWork.Save();
                TempData["success"] = "Category Created successfully";
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        //public IActionResult Edit(int? id)
        //{

        //    if (id == null && id == 0)
        //    {
        //        return NotFound();
        //    }


        //    var productFromDb = _unitOfWork.Product.Get(u => u.Id == id);

        //    if (productFromDb == null)
        //    {
        //        return NotFound();
        //    }


        //    return View(productFromDb);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(Product obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Category Edited successfully";

        //        return RedirectToAction("Index");
        //    }

        //    return View(obj);
        //}


        public IActionResult Delete(int? id)
        {

            if (id == null && id == 0)
            {
                return NotFound();
            }


            var productFromDb = _unitOfWork.Product.Get(u => u.Id == id);

            if (productFromDb == null)
            {
                return NotFound();
            }


            return View(productFromDb);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            if (productFromDb == null)
                return NotFound();

            _unitOfWork.Product.Remove(productFromDb);
            _unitOfWork.Save();
            TempData["success"] = "Category Deleted successfully";

            return RedirectToAction("Index");
        }
    }
}
