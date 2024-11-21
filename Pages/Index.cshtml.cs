using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StallSync.Data;
using StallSync.Models;
using System.Threading.Tasks;

namespace StallSync.Pages;

public class IndexModel : PageModel
{

    private readonly AppDbContext _context;

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }

    public List<TaskItem> TaskItems { get; set; } = [];

    public async Task OnGetAsync()
    {
        //TaskItems = await _context.TaskItems.ToListAsync();

        var startOfWeek = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek);
        var endOfWeek = startOfWeek.AddDays(7);

        TaskItems = await _context.TaskItems
            .Where(t => t.StartDate >= startOfWeek && t.StartDate < endOfWeek)
            .OrderBy(t => t.StartDate)
            .ToListAsync();
    }
}
