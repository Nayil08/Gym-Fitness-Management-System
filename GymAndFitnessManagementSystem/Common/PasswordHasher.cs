using System.Security.Cryptography;
using System.Text;
namespace GymAndFitnessManagementSystem.Common
{
    public static class PasswordHasher
    {
        public static string Hash(string value)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(value ?? ""));
                var sb = new StringBuilder();
                foreach (var b in bytes) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}
