using StallSync.Models;

namespace StallSync.Utility
{
    public class CsvHelper
    {
        public static string EscapeCsv(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;


            return $"\"{input.Replace("\"", "\"\"")}\"";
        }

        public static string GenerateCsv(IEnumerable<TaskItem> tasks)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Titel; Beskrivning; Ansvarig; Datum och tid; Klar?");

            foreach (var task in tasks)
            {
                sb.AppendLine(
                $"{EscapeCsv(task.Title)};" +
                $"{EscapeCsv(task.Description)};" +
                $"{EscapeCsv(task.ResponsiblePerson)};" +
                $"{task.StartDate};" +
                $"{task.IsCompleted}"
            );
            }
            return sb.ToString();
        }
    }
}
