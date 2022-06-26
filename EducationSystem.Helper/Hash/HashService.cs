
using System.Security.Cryptography;
using System.Text;

namespace EducationSystem.Helper.Hash
{
    public class HashService
    {
        public async Task<string> HashMD5Async(string text)
        {
            return await Task.Run(() =>
            {
                using (var hashAlg = MD5.Create())
                {
                    byte[] hash = hashAlg.ComputeHash(Encoding.UTF8.GetBytes(text));
                    var builder = new StringBuilder(hash.Length * 2);
                    for (int i = 0; i < hash.Length; i++)
                        builder.Append(hash[i].ToString("X2"));

                    return builder.ToString(); // Возвращаем значение хеша
                }
            });
        }
    }
}
