using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StallSync.Data;
using StallSync.Models;

namespace StallSync.Pages.Shared
{
    public class HorsePicPageModel : PageModel
    {
        private readonly AppDbContext _context;

        public HorsePicPageModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Horse> Horses { get; set; }

        [BindProperty]
        public string HorseName { get; set; }

        [BindProperty]
        public IFormFile UploadedFile { get; set; }

        private readonly string _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        public void OnGet()
        {
            Horses = _context.Horses.ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)  
            {
                return Page(); 
            }

            if (string.IsNullOrWhiteSpace(HorseName) || UploadedFile == null)
            {
                if (string.IsNullOrWhiteSpace(HorseName))
                {
                    ModelState.AddModelError("HorseName", "Hästnamn är obligatoriskt.");
                }

                if (UploadedFile == null)
                {
                    ModelState.AddModelError("UploadedFile", "En bild måste laddas upp.");
                }

                return Page();
            }

            var filePath = Path.Combine(_uploadFolderPath, UploadedFile.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await UploadedFile.CopyToAsync(stream);
            }

            var horse = new Horse
            {
                Name = HorseName,
                ImagePath = $"/uploads/{UploadedFile.FileName}"
            };

            _context.Horses.Add(horse);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
