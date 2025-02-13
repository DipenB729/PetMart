﻿using DotnetMastery.DataAccess.Repository.IRepository;
using Dotnet.Models;
using DotnetMastery.DataAccess.Data;
using DotnetMastery.DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace DotnetMastery.Areas.Admin.Controllers
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
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
           
            return View(objProductList);
        }
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            //ViewBag.CategoryList=CategoryList;
            ViewData["CategoryList"] = CategoryList;
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product obj)
        {           
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product added successfullly";
                return RedirectToAction("Index");
            }
            ViewData["CategoryList"] = _unitOfWork.Category.GetAll()
           .Select(u => new SelectListItem
           {
               Text = u.Name,
               Value = u.Id.ToString()
           });

            return View(obj);

        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product productfromdb = _unitOfWork.Product.Get(u => u.Id == id);
            if (productfromdb == null)
            {
                return NotFound();
            }
            return View(productfromdb);
        }
        [HttpPost]
        public IActionResult Edit(Product obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product Updated successfullly";
                return RedirectToAction("Index");
            }
            return View(obj);

        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? productfromdb = _unitOfWork.Product.Get(u => u.Id == id);
            if (productfromdb == null)
            {
                return NotFound();
            }
            return View(productfromdb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product Deleted successfullly";
            return RedirectToAction("Index");
            ;

        }
    }
}
