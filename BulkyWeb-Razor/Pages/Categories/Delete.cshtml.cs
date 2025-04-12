using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BulkyWeb_Razor.Data;
using BulkyWeb_Razor.Models;

namespace BulkyWeb_Razor.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        public void OnGet(int? id)
        {
            if (id == null || id == 0)
            {
                return;
            }
            Category = _db.Categories.Find(id);
        }

        public IActionResult OnPost()
        {
            _db.Categories.Remove(Category);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToPage("Index");
        }
    }
}
