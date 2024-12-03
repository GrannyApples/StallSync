using StallSync.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StallSync.Pages;
using StallSync.Utility;

namespace StallSync.Test
{
    public class CsvGenerationTests
    {
        [Fact]
        public void GenerateCsv_ShouldGenerateValidCsv()
        {
            // Arrange
            var tasks = new List<TaskItem>
        {
            new TaskItem { Title = "Task 1", Description = "Description 1", ResponsiblePerson = "Person 1", StartDate = DateTime.Now, IsCompleted = false },
            new TaskItem { Title = "Task 2", Description = "Description 2", ResponsiblePerson = "Person 2", StartDate = DateTime.Now.AddDays(1), IsCompleted = true }
        };

            // Act
            var csvContent = CsvHelper.GenerateCsv(tasks);

            // Assert
            Assert.Contains("Titel; Beskrivning; Ansvarig; Datum och tid; Klar?", csvContent);
            Assert.Contains("Task 1", csvContent);
            Assert.Contains("Task 2", csvContent);
        }

        [Fact]
        public void EscapeCsv_ShouldEscapeSpecialCharacters()
        {
            // Arrange
            var input = "A \"quoted\" text";

            // Act
            var escapedResult = CsvHelper.EscapeCsv(input);

            // Assert
            Assert.Equal("\"A \"\"quoted\"\" text\"", escapedResult);
        }
    }


}
