using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = SD.Role_Admin)]
	public class CompanyController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public CompanyController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			return View(_unitOfWork.Company.GetAll().ToList());
		}

		public IActionResult Upsert(int? id)
		{
			if (id is null or 0)
			{
				var company = new Company();

				return View(company);
			}
			else
			{
				var companyFromDb = _unitOfWork.Company.Get(c => c.Id == id);

				if (companyFromDb == null)
				{
					return NotFound();
				}

				return View(companyFromDb);
			}
		}
		
		[HttpPost]
		public IActionResult Upsert(Company company)
		{
			if (ModelState.IsValid)
			{
				if (company.Id == 0)
				{
					_unitOfWork.Company.Add(company);
					TempData["success"] = "Company created successfully";
				}
				else
				{
					_unitOfWork.Company.Update(company);
					TempData["success"] = "Company updated successfully";
				}

				_unitOfWork.Save();
				return RedirectToAction("Index");
			}
			else
			{
				if (company.Id == 0)
				{
					TempData["error"] = "Error creating company";
				}
				else
				{
					TempData["error"] = "Error updating company";
				}
				return View(company);
			}
		}

		#region API Calls
		[HttpGet]
		public IActionResult GetAll()
		{
			var companies = _unitOfWork.Company.GetAll().ToList();
			return Json(new { data = companies });
		}
		
		[HttpDelete]
		public IActionResult Delete(int? id)
		{
			var companyToBeDelete = _unitOfWork.Company.Get(p => p.Id == id);
			if (companyToBeDelete == null)
			{
				return Json(new { success = false, message = "Error while deleting" });
			}
			
			_unitOfWork.Company.Remove(companyToBeDelete);
			_unitOfWork.Save();

			return Json(new { success = true, message = "Delete Successful" });
		}
		#endregion
	}
}