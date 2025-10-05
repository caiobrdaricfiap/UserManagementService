using System.Security.Cryptography;

namespace UserManagementService.Models.User
{
    public class UserModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string RawPassword { get; set; } = string.Empty; // input temporário
        public string PasswordHash { get; private set; } = string.Empty;
        public string Salt { get; private set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        public void SetPassword(string plainPassword)
        {
            if (!IsValidPassword(plainPassword))
                throw new ArgumentException("Senha não atende aos requisitos de segurança.");

            Salt = GenerateSalt();
            PasswordHash = HashPassword(plainPassword, Salt);
        }

        public void SetPasswordFromHash(string hash, string salt)
        {
            PasswordHash = hash;
            Salt = salt;
        }

        public bool VerifyPassword(string plainPassword)
        {
            var hash = HashPassword(plainPassword, Salt);
            return hash == PasswordHash;
        }

        private bool IsValidPassword(string password)
        {
            var regex = new System.Text.RegularExpressions.Regex(
                @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()_+{}\[\]:;<>,.?~\\|]).+$");
            return password.Length >= 8 && regex.IsMatch(password);
        }

        private string GenerateSalt()
        {
            var buffer = new byte[16];
            RandomNumberGenerator.Fill(buffer);
            return Convert.ToBase64String(buffer);
        }

        private string HashPassword(string password, string salt)
        {
            var sha256 = System.Security.Cryptography.SHA256.Create();
            var saltedPassword = $"{salt}{password}";
            var bytes = System.Text.Encoding.UTF8.GetBytes(saltedPassword);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
