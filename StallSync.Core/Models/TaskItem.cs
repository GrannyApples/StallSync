using System.ComponentModel.DataAnnotations;
namespace StallSync.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ResponsiblePerson { get; set; } = string.Empty;

    [Required]
    [CustomDateTimeRange(ErrorMessage = "The StartDate must be after 06:00 and before 20:00.")]
    public DateTime StartDate { get; set; } = DateTime.Now;
    public bool IsCompleted { get; set; } = false;
    public class CustomDateTimeRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not DateTime startDate)
            {
                return new ValidationResult("Invalid StartDate format.");
            }

            // Check time range
            var timeOnly = startDate.TimeOfDay;
            if (timeOnly < TimeSpan.FromHours(6) || timeOnly > TimeSpan.FromHours(20))
            {
                return new ValidationResult(ErrorMessage);
            }

            // Optional: Check if the date is in the future
            if (startDate < DateTime.Now.AddHours(-1))
            {
                return new ValidationResult("StartDate must be in the future.");
            }

            return ValidationResult.Success;
        }
    }
}