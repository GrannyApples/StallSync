using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StallSync.Data;
using StallSync.Models;

namespace StallSync.Pages
{
    public class EditTaskPageModel(AppDbContext context) : PageModel
    {
        private readonly AppDbContext _context = context;


        [BindProperty]
        public TaskItem Task { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Task = await _context.TaskItems.FindAsync(id);

            if (Task == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var task = await _context.TaskItems.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            task.Title = Task.Title;
            task.Description = Task.Description;
            task.ResponsiblePerson = Task.ResponsiblePerson;
            task.StartDate = Task.StartDate;
            task.IsCompleted = Task.IsCompleted;

            await _context.SaveChangesAsync();

            return RedirectToPage("Index");

        }

    }
}
