using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using AIChatMessage = Microsoft.Extensions.AI.ChatMessage;


namespace AssistanService
{
    public interface IAssistanceService
    {
        Task<string> AskBot(string input);
    }


    public class AssistanService : IAssistanceService
    {
        private readonly HttpClient _httpClient;
        private readonly string _serverUrl;
        private readonly string _modelName;
        private readonly string _connectionString;
        private readonly Dictionary<string, List<AIChatMessage>> _conversationHistories;

        public AssistanService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _serverUrl = configuration["OllamaConfig:ServerUrl"]
                ?? throw new ArgumentNullException(nameof(_serverUrl), "OllamaConfig:ServerUrl is not configured.");
            _modelName = configuration["OllamaConfig:ModelName"]
                ?? throw new ArgumentNullException(nameof(_modelName), "OllamaConfig:ModelName is not configured.");
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString), "DefaultConnection is not configured.");
            _conversationHistories = new();
        }


        public async Task<string> AskBot(string input)
        {
            IChatClient chat = new OllamaChatClient(new Uri(_serverUrl), _modelName);

            ChatResponse response = await chat.GetResponseAsync(input);

            if (response is not null)
            {
                string assistantResponse = response.Text;

                return assistantResponse;

            }

            return "error";
        }
    }
}