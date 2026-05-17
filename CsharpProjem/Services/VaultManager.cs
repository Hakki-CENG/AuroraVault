using System;
using System.Collections.Generic;
using System.IO;

namespace AuroraVault
{
    public class VaultManager
    {
        public List<EncryptedNote> AllNotes = new List<EncryptedNote>();

        // Klasörden notları yükler (Varsayılan olarak "Notes" klasörü)
        public void LoadNotes(string username, string subFolder = "Notes")
        {
            AllNotes.Clear();
            string path = FileManager.GetUserPath(username, subFolder);
            if (Directory.Exists(path))
            {
                foreach (string file in Directory.GetFiles(path, "*.txt"))
                {
                    string title = Path.GetFileNameWithoutExtension(file);
                    string content = File.ReadAllText(file);
                    AllNotes.Add(new EncryptedNote(title, content));
                }
            }
        }

        public void AddNote(string username, string title, string content, string masterKey)
        {
            string secured = "[OK]" + content;
            string encrypted = CryptoService.EncryptDecrypt(secured, masterKey);
            FileManager.SaveNote(username, title, encrypted);
        }

        public void UpdateNote(string username, string title, string newContent, string masterKey, bool append)
        {
            string oldContent = "";
            if (append)
            {
                string encrypted = FileManager.ReadNote(username, title);
                string decrypted = CryptoService.EncryptDecrypt(encrypted, masterKey);
                if (decrypted.StartsWith("[OK]")) oldContent = decrypted.Substring(4);
            }

            string finalContent = append ? oldContent + "\n" + newContent : newContent;
            string secured = "[OK]" + finalContent;
            string encryptedFinal = CryptoService.EncryptDecrypt(secured, masterKey);
            FileManager.SaveNote(username, title, encryptedFinal);
        }

        // Gelişmiş Arama Fonksiyonu
        public List<string> SearchNotes(string username, string query, bool searchContent, string? masterKey)
        {
            List<string> results = new List<string>();
            string path = FileManager.GetUserPath(username);

            foreach (string file in Directory.GetFiles(path, "*.txt"))
            {
                string title = Path.GetFileNameWithoutExtension(file);
                // Başlıkta arama
                if (title.ToLower().Contains(query.ToLower()))
                {
                    results.Add("[BAŞLIK EŞLEŞTİ] " + title);
                    continue; // Başlıkta bulunduysa içeriğe bakmaya gerek yok
                }

                // İçerikte arama (Master Key gerektirir)
                if (searchContent && masterKey != null)
                {
                    string encrypted = File.ReadAllText(file);
                    string decrypted = CryptoService.EncryptDecrypt(encrypted, masterKey);
                    if (decrypted.StartsWith("[OK]") && decrypted.Contains(query))
                    {
                        results.Add("[İÇERİK EŞLEŞTİ] " + title);
                    }
                }
            }
            return results;
        }

        public void EmptyTrash(string username)
        {
            string path = FileManager.GetUserPath(username, "Trash");
            if (Directory.Exists(path))
            {
                foreach (string file in Directory.GetFiles(path)) File.Delete(file);
            }
        }
    }
}
