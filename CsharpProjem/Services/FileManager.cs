using System;
using System.IO;

namespace AuroraVault
{
    public static class FileManager
    {
        // subFolder parametresi eklendi: Notes, Trash veya Logs klasörlerine dinamik erişim sağlar
        public static string GetUserPath(string username, string subFolder = "Notes")
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{username}_{subFolder}");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path;
        }

        public static void SaveNote(string username, string title, string content)
        {
            string path = Path.Combine(GetUserPath(username), title + ".txt");
            File.WriteAllText(path, content);
        }

        public static string ReadNote(string username, string title)
        {
            string path = Path.Combine(GetUserPath(username), title + ".txt");
            return File.Exists(path) ? File.ReadAllText(path) : "";
        }

        // Loglama Sistemi
        public static void LogActivity(string username, string action)
        {
            string logPath = Path.Combine(GetUserPath(username, "Logs"), "activity.log");
            string logEntry = $"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}] {action}\n";
            File.AppendAllText(logPath, logEntry);
        }

        // Çöp Kutusuna Taşıma (Soft Delete)
        public static void MoveToTrash(string username, string title)
        {
            string oldPath = Path.Combine(GetUserPath(username, "Notes"), title + ".txt");
            string trashPath = Path.Combine(GetUserPath(username, "Trash"), title + ".txt");

            if (File.Exists(oldPath))
            {
                if (File.Exists(trashPath)) File.Delete(trashPath); // Üstüne yazmasın diye temizlik
                File.Move(oldPath, trashPath);
            }
        }
    }
}
