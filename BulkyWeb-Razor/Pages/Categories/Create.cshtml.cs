using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BulkyWeb_Razor.Data;
using BulkyWeb_Razor.Models;

namespace BulkyWeb_Razor.Pages.Categories
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public Category Category { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Error creating category";
                return Page();
            }

            _db.Categories.Add(Category);
            _db.SaveChanges();
            TempData["success"] = "Category has been created successfully";

            return RedirectToPage("/categories/index");
        }
    }
}
