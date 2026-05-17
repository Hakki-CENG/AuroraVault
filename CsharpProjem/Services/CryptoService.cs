using System.Text;

namespace AuroraVault
{
    public static class CryptoService
    {
        public static string EncryptDecrypt(string text, string key)
        {
            if (string.IsNullOrEmpty(text)) return "";
            StringBuilder result = new StringBuilder();
            for (int c = 0; c < text.Length; c++)
            {
                result.Append((char)(text[c] ^ key[c % key.Length]));
            }
            return result.ToString();
        }
    }
}
