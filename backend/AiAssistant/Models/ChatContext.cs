using Microsoft.EntityFrameworkCore;

namespace AiAssistant.models

{
    public class ChatContext:DbContext{
        public ChatContext(DbContextOptions<ChatContext> options)
        :base(options)
    {

    }
        public DbSet<Chats> Chats { get; set; } = null!;
        public DbSet<DbChatMessages> Messages { get; set; } = null!;
    }
}