using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StallSync.Data;
using StallSync.Models;

namespace StallSync.Pages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }

    public List<TaskItem> TaskItems { get; set; } = new();

    [BindProperty]
    public TaskItem? NewTask { get; set; }

    public async Task OnGetAsync()
    {
        await LoadTasksAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (NewTask == null)
        {
   
            ModelState.AddModelError(string.Empty, "NewTask is null.");
            await LoadTasksAsync();
            return Page();
        }

        if (!ModelState.IsValid)
        {
            await LoadTasksAsync();
            return Page();
        }

        _context.TaskItems.Add(NewTask);
        await _context.SaveChangesAsync();

        await LoadTasksAsync();
        return Page();
    }

    private async Task LoadTasksAsync()
    {
        var startOfWeek = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek);
        var endOfWeek = startOfWeek.AddDays(7);

        TaskItems = await _context.TaskItems
            .Where(t => t.StartDate >= startOfWeek && t.StartDate < endOfWeek)
            .OrderBy(t => t.StartDate)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {

        var taskToDelete = await _context.TaskItems.FindAsync(id);
        if (taskToDelete != null)
        {
            _context.TaskItems.Remove(taskToDelete);
            await _context.SaveChangesAsync();
        }

        await LoadTasksAsync();
        return RedirectToPage();
    }
}