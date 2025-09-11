using System;
using System.Security.Cryptography;
using System.Text;

namespace Code.Services.UniqueId
{
    public class UniqueIdService : IUniqueIdService
    {
        public string GenerateUniqueId() => GenerateUniqueId("ID", 8);

        public string GenerateUniqueId(string prefix) => GenerateUniqueId(prefix, 8);

        public string GenerateUniqueId(string prefix, int length)
        {
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            int randomValue = UnityEngine.Random.Range(1000, 9999);
            
            string dataToHash = $"{timestamp}_{randomValue}_{Environment.TickCount}";
            
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(dataToHash));
                string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                string uniquePart = hashString.Substring(0, Math.Min(length, hashString.Length));
                return $"{prefix}_{uniquePart}";
            }
        }
    }
}
