﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NguyenKhanhMinhRazorPages.Entity;
using NguyenKhanhMinhRazorPages.Services;

namespace NguyenKhanhMinhRazorPages.Pages.CategoryPages
{
    public class DeleteModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public DeleteModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short id)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            Category = category;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(short id)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (category != null)
            {
                if (category.NewsArticles.Any())
                {
                    Category = category;
                    TempData["ErrorMessage"] = "Cannot delete this category because it is associated with news articles.";
                    return Page();
                }
                await _categoryService.RemoveCategory(id);
            }
            return RedirectToPage("./Index");
        }
    }
}
