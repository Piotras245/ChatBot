using Microsoft.Extensions.AI;

public class ChatMessage
{
    private ChatRole user;
    private string input;

    public ChatMessage(ChatRole user, string input)
    {
        this.user = user;
        this.input = input;
    }
}
