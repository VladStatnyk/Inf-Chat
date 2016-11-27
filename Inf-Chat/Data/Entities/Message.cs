using System;

namespace Inf_Chat.Data.Entities
{
    public class Message
    {
        public Guid Id { get; set; }
        public DateTime AddedDate { get; set; }

        public string Text { get; set; }

        public virtual User Author { get; set; }

        public Message()
        {
            Id = Guid.NewGuid();
            AddedDate = DateTime.UtcNow;
        }
    }
}