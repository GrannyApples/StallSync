using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StallSync.Data;
using StallSync.Models;
using StallSync.Utility;

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

    public int WeekOffSet { get; set; }

    public DateTime StartOfWeek { get; private set; }
    public DateTime EndOfWeek { get; set; }

    public async Task OnGetAsync(int? weekOffSet = 0)
    {
        WeekOffSet = weekOffSet ?? 0;
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
        return RedirectToPage(new {weekOffSet = WeekOffSet});
    }

    private async Task LoadTasksAsync()
    {
        StartOfWeek = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek + (WeekOffSet*7));
        EndOfWeek = StartOfWeek.AddDays(7);

        TaskItems = await _context.TaskItems
            .Where(t => t.StartDate >= StartOfWeek && t.StartDate < EndOfWeek)
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

    public async Task<IActionResult> OnPostToggleCompleteAsync(int id)
    {
        var task = await _context.TaskItems.FindAsync(id);
        if (task != null)
        {
            task.IsCompleted = !task.IsCompleted;
            await _context.SaveChangesAsync();
        }

        await LoadTasksAsync();
        return RedirectToPage(new { weekOffSet = WeekOffSet });
    }

    public async Task<IActionResult> OnPostDownloadSnapshotAsync(int weekOffset)
    {
        StartOfWeek = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek + (weekOffset * 7));
        EndOfWeek = StartOfWeek.AddDays(7);  // Veckans slutdatum

        var tasks = await _context.TaskItems
            .Where(t => t.StartDate >= StartOfWeek && t.StartDate < EndOfWeek)
            .OrderBy(t => t.StartDate)
            .ToListAsync();

        var csvContent = CsvHelper.GenerateCsv(tasks);

        var fileName = $"Schedule_Snapshot{DateTime.Now:yyyyMMdd}.csv";
        return File(new System.Text.UTF8Encoding().GetBytes(csvContent), "text/csv", fileName);
    }


}