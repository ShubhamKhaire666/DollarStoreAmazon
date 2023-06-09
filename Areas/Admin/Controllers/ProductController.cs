﻿using DollarStoreAmazon.DataAccess.Data;
using DollarStoreAmazon.DataAccess.Repository.IRepository;
using DollarStoreAmazon.Model;
using DollarStoreAmazon.Model.ViewModels;
using DollarStoreAmazon.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Data;

namespace DollarStoreAmazon.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

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
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
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
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"Images\Products");

                    if (!string.IsNullOrEmpty(obj.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.Product.ImageUrl = @"\Images\Products\" + fileName;
                }
                else
                {
                    obj.Product.ImageUrl = "";
                    _unitOfWork.Product.Add(obj.Product);
                    _unitOfWork.Save();
                    TempData["success"] = "Category Created successfully";
                    return RedirectToAction("Index");
                }

                if (obj.Product.Id == 0 || obj.Product.Id == null)
                {
                    _unitOfWork.Product.Add(obj.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                }
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


        //public IActionResult Delete(int? id)
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

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeletePOST(int? id)
        //{
        //    var productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
        //    if (productFromDb == null)
        //        return NotFound();

        //    _unitOfWork.Product.Remove(productFromDb);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Category Deleted successfully";

        //    return RedirectToAction("Index");
        //}

        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProducts = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

            return Json(new { data = objProducts });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var objProduct = _unitOfWork.Product.Get(u => u.Id == id);

            if (objProduct == null)
                return Json(new { success = false, message = "Error While Detecting the Object" });


            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, objProduct.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(objProduct);
            _unitOfWork.Save();

            return Json(new { success= true, message="Delete was succesful" });
        }

        #endregion
    }
}
