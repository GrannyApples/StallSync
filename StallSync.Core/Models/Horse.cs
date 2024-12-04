using System.ComponentModel.DataAnnotations;

namespace StallSync.Models
{
    public class Horse
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string ImagePath { get; set; }
    }
}
