using AiAssistant.models;

namespace AiAssistant.Dtos
{
    public class ChatMessageDto
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public string Date { get; set; }
        public ChatRole Role { get; set; }
        public string Text { get; set; } = string.Empty;
        public ChatDto Chat { get; set; }
    }
}
