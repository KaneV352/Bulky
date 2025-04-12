using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            return View(_unitOfWork.Product.GetAll(includeProperties: "Category"));
        }

        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                ProductViewModel productVM = new()
                {
                    Product = new(),
                    CategoryList = _unitOfWork.Category.GetAll()
                        .Select(i => new SelectListItem
                        {
                            Text = i.Name,
                            Value = i.Id.ToString()
                        })
                };

                return View(productVM);
            }
            else 
            {
                var productFromDb = _unitOfWork.Product.Get(p => p.Id == id);
                if (productFromDb == null)
                {
                    return NotFound();
                }
                ProductViewModel productVM = new()
                {
                    Product = productFromDb,
                    CategoryList = _unitOfWork.Category.GetAll()
                        .Select(i => new SelectListItem
                        {
                            Text = i.Name,
                            Value = i.Id.ToString()
                        })
                };
                return View(productVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductViewModel productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string rootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(rootPath, @"images\products");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        string oldProductPath = Path.Combine(rootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldProductPath))
                        {
                            System.IO.File.Delete(oldProductPath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\images\products\" + fileName;
                }

                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                    TempData["success"] = "Product created successfully";
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                    TempData["success"] = "Product updated successfully";
                }
                _unitOfWork.Save();

                return RedirectToAction("Index", "Product");
            }
            else
            {
                IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll()
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
                productVM.CategoryList = categoryList;
                return View(productVM);
            }
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = products });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(p => p.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            string rootPath = _webHostEnvironment.WebRootPath;

            if (!string.IsNullOrEmpty(productToBeDeleted.ImageUrl))
            {
                string oldProductPath = Path.Combine(rootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldProductPath))
                {
                    System.IO.File.Delete(oldProductPath);
                }
            }
            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
