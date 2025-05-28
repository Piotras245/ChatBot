using System.ComponentModel.DataAnnotations.Schema;

namespace AiAssistant.models
{
    public enum ChatRole
    {
        User = 0,
        Assistant = 1
    }
    public class DbChatMessages
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public string Date { get; set; }
        public ChatRole Role { get; set; }
        public string Text { get; set; } = string.Empty;
        public Chats Chat { get; set; }
    }
}
