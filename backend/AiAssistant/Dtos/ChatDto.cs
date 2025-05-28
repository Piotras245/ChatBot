using AiAssistant.models;

namespace AiAssistant.Dtos
{
    public class ChatDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<ChatMessageDto> Messages { get; set; }

    }
}
