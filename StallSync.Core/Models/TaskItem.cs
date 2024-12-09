using System.ComponentModel.DataAnnotations;
namespace StallSync.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ResponsiblePerson { get; set; } = string.Empty;

    [Required]
    [CustomDateTimeRange(ErrorMessage = "The StartDate must be between 06:00 and 19:59.")]
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

             // Define valid start time and end time
            var minAllowedTime = TimeSpan.FromHours(6); // 06:00
            var maxAllowedTime = TimeSpan.FromHours(19).Add(TimeSpan.FromMinutes(59)); // 19:59

            // Check time range
            var timeOnly = startDate.TimeOfDay;
            if (timeOnly < minAllowedTime || timeOnly > maxAllowedTime)
            {
                return new ValidationResult(ErrorMessage);
            }

            
            if (startDate < DateTime.Now.AddHours(-1))
            {
                return new ValidationResult("StartDate must be in the future.");
            }

            return ValidationResult.Success;
        }
    }
}
