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
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public IList<Category> CategoryList { get; set; }

        public ApplicationDbContext Context => _db;

        public void OnGet()
        {
            CategoryList = _db.Categories.ToList();
        }
    }
}
