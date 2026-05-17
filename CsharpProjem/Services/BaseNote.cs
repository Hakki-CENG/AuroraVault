using System;

namespace AuroraVault
{
    public class BaseNote
    {
        public string Id { get; private set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; private set; }

        public BaseNote(string title)
        {
            Id = Guid.NewGuid().ToString().Substring(0, 8);
            Title = title;
            CreatedDate = DateTime.Now;
        }

        public virtual void ShowDetails()
        {
            Console.WriteLine($"[{Id}] {Title} - {CreatedDate.ToShortDateString()}");
        }
    }
}
