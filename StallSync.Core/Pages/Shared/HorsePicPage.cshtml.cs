using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StallSync.Data;
using StallSync.Models;
using System.IO;

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
            
            if (!Directory.Exists(_uploadFolderPath))
            {
                Directory.CreateDirectory(_uploadFolderPath);
            }
         
            var uniqueFileName = $"{Guid.NewGuid()}_{UploadedFile.FileName}";
            var filePath = Path.Combine(_uploadFolderPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await UploadedFile.CopyToAsync(stream);
            }

            var horse = new Horse
            {
                Name = HorseName,
                ImagePath = $"/uploads/{uniqueFileName}"
            };

            _context.Horses.Add(horse);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var horse = await _context.Horses.FindAsync(id);
            if (horse == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(_uploadFolderPath, Path.GetFileName(horse.ImagePath));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.Horses.Remove(horse);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}