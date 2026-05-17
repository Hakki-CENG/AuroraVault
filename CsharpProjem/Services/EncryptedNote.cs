using System;

namespace AuroraVault
{
    public class EncryptedNote : BaseNote
    {
        public string EncryptedContent { get; set; }

        public EncryptedNote(string title, string encryptedContent) : base(title)
        {
            EncryptedContent = encryptedContent;
        }

        public override void ShowDetails()
        {
            Console.WriteLine($"[KİLİTLİ] {Title} (İçeriği görmek için şifre girin)");
        }
    }
}
