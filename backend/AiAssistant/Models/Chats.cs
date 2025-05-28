namespace AiAssistant.models
{
    public class Chats
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<DbChatMessages> Messages { get; set; }
    }
}
