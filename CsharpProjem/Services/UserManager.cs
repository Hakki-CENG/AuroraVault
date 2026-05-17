using System;
using System.IO;

namespace AuroraVault
{
    public static class UserManager
    {
        private static string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Users");
        private static string systemKey = "aurora_master_2026";

        static UserManager() { if (!Directory.Exists(folder)) Directory.CreateDirectory(folder); }

        public static bool Register(string user, string pass, string masterKey)
        {
            string path = Path.Combine(folder, user + ".usr");
            if (File.Exists(path)) return false;

            string data = $"{pass}|{masterKey}|0";
            File.WriteAllText(path, CryptoService.EncryptDecrypt(data, systemKey));
            return true;
        }

        public static string[]? GetUserData(string user)
        {
            string path = Path.Combine(folder, user + ".usr");
            if (!File.Exists(path)) return null;
            return CryptoService.EncryptDecrypt(File.ReadAllText(path), systemKey).Split('|');
        }

        public static bool VerifyMasterKey(string user, string inputKey)
        {
            string[]? data = GetUserData(user);
            if (data != null && data.Length >= 2) return data[1] == inputKey;
            return false;
        }

        public static void SetRememberMe(string user, bool remember)
        {
            string[]? data = GetUserData(user);
            if (data == null) return;
            string newData = $"{data[0]}|{data[1]}|{(remember ? "1" : "0")}";
            File.WriteAllText(Path.Combine(folder, user + ".usr"), CryptoService.EncryptDecrypt(newData, systemKey));

            string settingsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.cfg");
            if (remember) File.WriteAllText(settingsFile, user);
            else if (File.Exists(settingsFile)) File.Delete(settingsFile);
        }

        public static string? GetRememberedUser()
        {
            string settingsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.cfg");
            if (File.Exists(settingsFile)) return File.ReadAllText(settingsFile);
            return null;
        }
    }
}
