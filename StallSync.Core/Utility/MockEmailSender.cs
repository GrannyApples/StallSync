using Microsoft.AspNetCore.Identity.UI.Services;

namespace StallSync.Utility
{
    public class MockEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Mock email sending logic (you can log the email details if needed)
            Console.WriteLine($"Sending Email to: {email}, Subject: {subject}, Message: {htmlMessage}");
            return Task.CompletedTask;
        }
    }
}
