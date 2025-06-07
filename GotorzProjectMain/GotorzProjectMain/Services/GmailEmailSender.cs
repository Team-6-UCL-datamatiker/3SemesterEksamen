using GotorzProjectMain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace GotorzProjectMain.Services
{
    internal sealed class GmailEmailSender : IEmailSender<ApplicationUser>
    {
        private readonly IConfiguration _config;

        public GmailEmailSender(IConfiguration config)
        {
            _config = config;
        }

		// Sends a confirmation email with a clickable link
		public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink) =>
            SendEmailAsync(email, "Confirm your email", $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

		// Sends a password reset email with a clickable link
		public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink) =>
            SendEmailAsync(email, "Reset your password", $"You can reset your password by <a href='{resetLink}'>clicking here</a>.");

		// Sends a password reset code
		public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode) =>
            SendEmailAsync(email, "Reset your password", $"Reset code: {resetCode}");

		// Composes and sends an email using SMTP settings from config
		private Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            var emailSettings = _config.GetSection("Email");

            var smtpClient = new SmtpClient(emailSettings["SmtpServer"])
            {
                Port = int.Parse(emailSettings["Port"]),
                Credentials = new NetworkCredential(emailSettings["Username"], emailSettings["Password"]),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(emailSettings["SenderEmail"], emailSettings["SenderName"]),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            return smtpClient.SendMailAsync(mailMessage);
        }
    }
}
