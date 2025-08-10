using System.Security;
using System.Security.Cryptography;
using System.Text;
using blazam.org.Data.Plugins.Models;
using blazam.org.Shared;
using Microsoft.EntityFrameworkCore;

namespace blazam.org.Data.Plugins
{
    public class PluginAuthService
    {
        private readonly IDbContextFactory<PluginsDbContext> _contextFactory;
        private readonly EmailService _emailService;

        public PluginAuthService(IDbContextFactory<PluginsDbContext> contextFactory, EmailService emailService)
        {
            _contextFactory = contextFactory;
            _emailService = emailService;
        }

        public async Task<(bool success, string message)> RegisterUserAsync(string email, string username, string password)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            // Check if email already exists
            if (await context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower()))
            {
                return (false, "Email is already registered");
            }

            // Check if username already exists
            if (await context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower()))
            {
                return (false, "Username is already taken");
            }

            // Create new user
            var user = new PendingPluginUser
            {
                Email = email,
                Username = username,
                PasswordHash = HashPassword(password),
                IsVerified = false
            };

            context.PendingUsers.Add(user);
            await context.SaveChangesAsync();

            // Create verification token
            var token = GenerateVerificationToken();
            var verification = new PluginVerification
            {
                UserId = user.Id,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddDays(1)
            };

            context.Verifications.Add(verification);
            await context.SaveChangesAsync();

            // Send verification email
            await _emailService.SendVerificationEmailAsync(email, token);

            return (true, "Registration successful. Please check your email to verify your account.");
        }

        public async Task<bool> VerifyEmailAsync(string token)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var verification = await context.Verifications
                .Include(v => v.PendingUser)
                .FirstOrDefaultAsync(v => v.Token == token);

            if (verification == null || verification.ExpiresAt < DateTime.UtcNow)
            {
                return false;
            }

            verification.PendingUser.IsVerified = true;
            PluginUser registeredUser = new PluginUser()
            {
                Email = verification.PendingUser.Email,
                PasswordHash = verification.PendingUser.PasswordHash,
                Username = verification.PendingUser.Username,
                IsVerified = true
            };
            context.Users.Add(registeredUser);
            context.PendingUsers.Remove(verification.PendingUser);
            context.Verifications.Remove(verification);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<PluginUser> AuthenticateAsync(string email, SecureString password)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var user = await context.Users.FirstOrDefaultAsync(
                u => u.Email.ToLower() == email.ToLower());

            if (user == null || !VerifyPassword(password.ToPlainText(), user.PasswordHash))
            {
                return null;
            }

            return user;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }

        private string GenerateVerificationToken()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}